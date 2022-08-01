USE Justific_DB
GO

-- cria��o da tabela de organiza��o
IF OBJECT_ID('Organizacao', 'U') IS NULL
	CREATE TABLE Organizacao (
		Id BIGINT PRIMARY KEY IDENTITY,
		Nome VARCHAR(500) NOT NULL,
		Cnpj CHAR(14) NOT NULL,
		Data_Criacao DATETIME NOT NULL DEFAULT GETDATE(),
		Alterado_Em DATETIME DEFAULT NULL,
		Excluido BIT NOT NULL DEFAULT 'False'
	)
GO

-- cria��o da tabela de rela��o entre usu�rios e organiza��es
IF OBJECT_ID('Usuario_Organizacao', 'U') IS NULL
	CREATE TABLE Usuario_Organizacao (
		Usuario_Id BIGINT NOT NULL REFERENCES Usuario(Id) ON DELETE CASCADE,
		Organizacao_Id BIGINT NOT NULL REFERENCES Organizacao(Id) ON DELETE CASCADE,
		Excluido BIT NOT NULL DEFAULT 'False',
		CONSTRAINT PK_Usuario_Organizacao PRIMARY KEY (Usuario_Id, Organizacao_Id)
	)
GO

-- cria��o da fun��o para desprezar caracteres do CNPJ
CREATE OR ALTER FUNCTION F_CONSIDERAR_SOMENTE_DIGITOS (@Cnpj CHAR(20))
	RETURNS CHAR(14)
AS
BEGIN
	RETURN TRIM(REPLACE(REPLACE(REPLACE(@Cnpj, '.', ''), '/', ''), '-', ''))
END
GO

-- cria��o da fun��o para validar o cnpj informado
CREATE OR ALTER FUNCTION F_VALIDAR_CNPJ(@Cnpj CHAR(20))
	RETURNS BIT
AS
BEGIN	
	RETURN CASE WHEN LEN(dbo.F_CONSIDERAR_SOMENTE_DIGITOS(@Cnpj)) = 14 THEN 'True' ELSE 'False' END
END
GO

-- SP para incluir/alterar organiza��o
CREATE OR ALTER PROC SP_INCLUIR_ALTERAR_ORGANIZACAO
	@Nome VARCHAR(500), @Cnpj CHAR(14)
AS
BEGIN
	DECLARE @Id_Organizacao BIGINT

	IF dbo.F_VALIDAR_CNPJ(@Cnpj) = 'False'
		RAISERROR('O CNPJ est� no formato inv�lido.', 0, 0)

	SELECT @Id_Organizacao = Id
		FROM Organizacao
	WHERE Cnpj = @Cnpj

	IF @Id_Organizacao > 0
	BEGIN
		UPDATE Organizacao
		SET	Nome = @Nome,
			Alterado_Em = GETDATE()
		WHERE Id = @Id_Organizacao

		SELECT @Id_Organizacao Id 
		RETURN
	END

	INSERT INTO Organizacao (Nome, Cnpj)
		VALUES (@Nome, @Cnpj)

	SELECT @@IDENTITY Id
END
GO

-- SP para excluir logicamente uma organiza��o
CREATE OR ALTER PROC SP_EXCLUIR_ORGANIZACAO
	@Id_Organizacao BIGINT
AS
BEGIN
	UPDATE Usuario_Organizacao
	SET Excluido = 'True'
	WHERE Organizacao_Id = @Id_Organizacao

	UPDATE Organizacao
	SET Excluido = 'True',
		Alterado_Em = GETDATE()
	WHERE Id = @Id_Organizacao AND
		Excluido = 'False'

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @Mensagem VARCHAR(100) = CONCAT('A organiza��o com o id ', @Id_Organizacao, ' n�o foi localizada.')
		RAISERROR (@Mensagem, 0, 0)
	END
END
GO

-- view para listagem de organiza��es
CREATE OR ALTER VIEW VW_LISTAR_ORGANIZACOES AS
	SELECT *
		FROM Organizacao
	WHERE Excluido = 'False'
GO

-- SP para obter a organiza��o por cnpj
CREATE OR ALTER PROC SP_OBTER_ORGANIZACAO
	@Cnpj CHAR(14)
AS
BEGIN
	SELECT *
		FROM VW_LISTAR_ORGANIZACOES
	WHERE Cnpj = @Cnpj
END
GO

-- cria��o da SP para vincular organiza��o ao usu�rio
CREATE OR ALTER PROC SP_VINCULAR_ORGANIZACAO_USUARIO
	@Login_Usuario VARCHAR(100), @Cnpj_Organizacao CHAR(14), @Desfazer BIT = 'False'
AS
BEGIN
	DECLARE @Id_Usuario BIGINT,
		    @Id_Organizacao BIGINT,
			@Existe_Registro_Excluido BIT,
			@Mensagem_Erro VARCHAR(100),
			@Registros_Afetados BIT

	SELECT @Id_Usuario = Id
		FROM VW_LISTAR_USUARIOS
	WHERE [Login] = @Login_Usuario

	IF @@ROWCOUNT = 0
	BEGIN
		SET @Mensagem_Erro = CONCAT('O usu�rio com o login ', @Login_Usuario, ' n�o foi localizado.')
		RAISERROR (@Mensagem_Erro, 0, 0)
		RETURN
	END

	SELECT @Id_Organizacao = Id
		FROM VW_LISTAR_ORGANIZACOES
	WHERE Cnpj = @Cnpj_Organizacao

	IF @@ROWCOUNT = 0
	BEGIN
		SET @Mensagem_Erro = CONCAT('A Organiza��o com o CNPJ ', @Cnpj_Organizacao, ' n�o foi localizada.')
		RAISERROR (@Mensagem_Erro, 0, 0)
		RETURN
	END

	SELECT @Existe_Registro_Excluido = Excluido
		FROM Usuario_Organizacao
	WHERE Usuario_Id = @Id_Usuario AND
		  Organizacao_Id = @Id_Organizacao

	SET @Registros_Afetados = CASE WHEN @@ROWCOUNT > 0 THEN 'True' ELSE 'False' END

	IF @Registros_Afetados = 'True' AND @Existe_Registro_Excluido = 'True' AND @Desfazer = 'False'
	BEGIN
		UPDATE Usuario_Organizacao
		SET Excluido = 'False'
		WHERE Usuario_Id = @Id_Usuario AND
			  Organizacao_Id = @Id_Organizacao

		SELECT CONVERT(BIT, 'True')
		RETURN
	END
	ELSE IF @Registros_Afetados = 'True' AND @Existe_Registro_Excluido = 'False' AND @Desfazer = 'True'
	BEGIN
		UPDATE Usuario_Organizacao
		SET Excluido = 'True'
		WHERE Usuario_Id = @Id_Usuario AND
			  Organizacao_Id = @Id_Organizacao

		SELECT CONVERT(BIT, 'True')
		RETURN
	END
	ELSE IF @Registros_Afetados = 'True'
	BEGIN
		SELECT CONVERT(BIT, 'True')
		RETURN
	END

	INSERT INTO Usuario_Organizacao (Usuario_Id, Organizacao_Id)
		VALUES (@Id_Usuario, @Id_Organizacao)

	SELECT CONVERT(BIT, 'True')
		RETURN
END
GO

-- cria��o da SP para listar as associa��es entre organiza��es e usu�rios
CREATE OR ALTER PROC SP_LISTAR_ORGANIZACOES_USUARIOS
	@Cnpj_Organizacao CHAR(14)
AS
BEGIN
	IF dbo.F_VALIDAR_CNPJ(@Cnpj_Organizacao) = 'False'
		RAISERROR('O CNPJ est� no formato inv�lido.', 0, 0)

	SELECT o.Id Organizacao_Id,
		   o.Nome Nome_Organizacao,
		   u.Id Usuario_Id,
		   u.[Login] Login_Usuario
		FROM VW_LISTAR_ORGANIZACOES o
			INNER JOIN Usuario_Organizacao uo
				ON o.Id = uo.Organizacao_Id
			INNER JOIN VW_Listar_Usuarios u
				ON uo.Usuario_Id = u.Id
	WHERE o.Cnpj = @Cnpj_Organizacao
END