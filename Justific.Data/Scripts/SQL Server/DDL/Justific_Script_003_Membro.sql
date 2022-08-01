USE Justific_DB
GO

-- criação da tabela de membro
IF OBJECT_ID('Membro', 'U') IS NULL
	CREATE TABLE Membro
	(
		Id BIGINT PRIMARY KEY IDENTITY,
		Codigo_Registro VARCHAR(50) NOT NULL,
		Nome VARCHAR(500) NOT NULL,
		Organizacao_Id BIGINT NOT NULL FOREIGN KEY REFERENCES Organizacao(Id),
		Data_Criacao DATETIME NOT NULL DEFAULT GETDATE(),
		Alterado_Em DATETIME DEFAULT NULL,
		Excluido BIT DEFAULT NULL,
		UNIQUE (Organizacao_Id, Codigo_Registro)
	)
GO

-- SP para incluir/ alterar um membro
CREATE OR ALTER PROC SP_INCLUIR_ALTERAR_MEMBRO
	@Codigo_Registro VARCHAR(50), @Nome VARCHAR(500), @Cnpj_Organizacao CHAR(20)
AS
BEGIN
	DECLARE @Id_Organizacao BIGINT,
			@Id_Membro BIGINT

	IF LEN(@Codigo_Registro) < 1
	BEGIN
		RAISERROR ('O Código de registro deve ser informado', 0, 0)
	END

	IF dbo.F_VALIDAR_CNPJ(@Cnpj_Organizacao) = 'False'
	BEGIN
		RAISERROR ('O CNPJ informado é inválido', 0, 0)
	END

	SET @Cnpj_Organizacao = dbo.F_CONSIDERAR_SOMENTE_DIGITOS(@Cnpj_Organizacao)

	SELECT @Id_Organizacao = Id
		FROM VW_LISTAR_ORGANIZACOES
	WHERE Cnpj = @Cnpj_Organizacao

	IF @@ROWCOUNT = 0
	BEGIN
		RAISERROR ('A organização não foi localizada com o CNPJ informado', 0, 0)
	END

	SELECT @Id_Membro = Id
		FROM Membro
	WHERE Organizacao_Id = @Id_Organizacao AND
		  Codigo_Registro = @Codigo_Registro

	IF @@ROWCOUNT = 0
	BEGIN
		UPDATE Membro
		SET Nome = @Nome,
			Alterado_Em = GETDATE()
		WHERE ID = @Id_Membro

		SELECT @Id_Membro Id
		RETURN
	END

	INSERT INTO Membro (Codigo_Registro, Nome,	Organizacao_Id)
		VALUES (@Codigo_Registro, @Nome, @Id_Organizacao)

	SELECT @@IDENTITY Id
END
GO

-- SP para a exclusão de um membro
CREATE OR ALTER PROC SP_EXCLUIR_MEMBRO
	@Membro_Id BIGINT
AS
BEGIN
	UPDATE Membro
	SET Excluido = 'True',
		Alterado_Em = GETDATE()
	WHERE Id = @Membro_Id
END
GO

-- view para listar os membros e organização atrelada
CREATE OR ALTER VIEW VW_LISTAR_MEMBROS
	AS
SELECT m.id MembroId,
	   m.codigo_registro CodigoRegistro,
	   m.nome NomeMembro,
	   m.data_criacao DataCriacaoMembro,
	   m.alterado_em MembroAlteradoEm,
	   o.id OrganizacaoId,
	   o.nome NomeOrganizacao,
	   o.cnpj
	FROM Membro m
		INNER JOIN VW_LISTAR_ORGANIZACOES o
			ON m.Organizacao_Id = o.Id
WHERE m.Excluido = 'False'
GO

-- SP para obter um membro através do código de registro
CREATE OR ALTER PROC SP_OBTER_MEMBRO
	@Codigo_Registro VARCHAR(50), @Organizacao_Id BIGINT
AS
BEGIN
	SELECT *
		FROM VW_LISTAR_MEMBROS
	WHERE OrganizacaoId = @Organizacao_Id AND
		CodigoRegistro = @Codigo_Registro
END
GO