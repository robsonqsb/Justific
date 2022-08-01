IF DB_ID('Justific_DB') IS NULL
	CREATE DATABASE Justific_DB
GO

USE Justific_DB
GO

-- criação da tabela de usuário
IF OBJECT_ID('Usuario', 'U') IS NULL
	CREATE TABLE Usuario (
		Id BIGINT PRIMARY KEY IDENTITY,
		[Login] VARCHAR(100) not null,
		Senha VARCHAR(20) not null,
		Data_Criacao DATETIME NOT NULL DEFAULT GETDATE(),
		Alterado_Em DATETIME DEFAULT NULL,
		Excluido BIT DEFAULT 'False'
	)
GO

-- view para listagem de usuários
CREATE OR ALTER VIEW VW_LISTAR_USUARIOS AS
	SELECT Id,
		   [Login],
		   Senha,
		   Data_Criacao,
		   Alterado_Em
		FROM Usuario
	WHERE Excluido = 'False';
GO

-- procedure para inserir ou alterar um usuário
CREATE OR ALTER PROC SP_INCLUIR_ALTERAR_USUARIO 
	@Login VARCHAR(100), @Senha VARCHAR(20)
AS
BEGIN
	DECLARE @Id_Usuario BIGINT

	SELECT TOP 1 @Id_Usuario = Id
		FROM VW_Listar_Usuarios
	WHERE [Login] = @Login

	IF @Id_Usuario > 0
	BEGIN		
		UPDATE Usuario
		SET Senha = @Senha,
			Alterado_Em = GETDATE()
		WHERE Id = @Id_Usuario	
		SELECT @Id_Usuario Id
		RETURN
	END

	INSERT INTO Usuario ([Login], Senha) 
		VALUES (@Login, @Senha)
	SELECT @@IDENTITY Id
	RETURN
END
GO

-- procedure para excluir logicamente um usuário
CREATE OR ALTER PROC SP_EXCLUIR_USUARIO
	@Id_Usuario BIGINT
AS
BEGIN
	UPDATE Usuario
	SET Excluido = 'True',
		Alterado_Em = GETDATE()
	WHERE Id = @Id_Usuario AND
		Excluido = 'False'

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @Texto_Retorno VARCHAR(100) = 'Usuário com id ' + CONVERT(VARCHAR, @Id_Usuario) + ' não foi localizado.'
		RAISERROR (@Texto_Retorno, 1, 1)
	END
END
GO

-- função para confirmação do login do usuário
CREATE OR ALTER PROC SP_CONFIRMAR_LOGIN_USUARIO
	@Login VARCHAR(100), @Senha VARCHAR(20)
AS
BEGIN
	SELECT CONVERT(BIT, 'True')
		FROM VW_Listar_Usuarios
	WHERE [Login] = TRIM(@Login) AND
		  Senha = @Senha
END
GO

-- função para obter o usuário por login
CREATE OR ALTER PROC SP_OBTER_USUARIO
	@Login VARCHAR(100)
AS
BEGIN
	SELECT TOP 1 *
		FROM VW_Listar_Usuarios
	WHERE [Login] = @Login
END