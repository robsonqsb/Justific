USE Justific_DB
GO

-- criação da tabela de justificativa
IF OBJECT_ID('Justificativa', 'U') IS NULL
	CREATE TABLE Justificativa
	(
		Id BIGINT PRIMARY KEY IDENTITY,
		Membro_Id BIGINT NOT NULL FOREIGN KEY REFERENCES Membro(Id),
		Data_Ocorrencia DATE NOT NULL DEFAULT GETDATE(),
		Possui_Comprovante BIT NOT NULL DEFAULT 'True',
		Comentarios VARCHAR(1000),
		Data_Criacao DATETIME NOT NULL DEFAULT GETDATE(),
		Alterado_Em DATETIME DEFAULT NULL,
		Excluido BIT NOT NULL DEFAULT 'False'
	)
GO

-- SP para incluir/alterar uma justificativa
CREATE OR ALTER PROC SP_INCLUIR_ALTERAR_JUSTIFICATIVA
	@Codigo_Registro_Membro VARCHAR(50), @Cnpj_Organizacao VARCHAR(20), @Comentarios VARCHAR(1000), 
	@Data_Ocorrencia DATE, @Possui_Comprovante BIT = 'False'
AS
BEGIN
	DECLARE @Id_Membro BIGINT,
		    @Id_Justificativa BIGINT

	IF dbo.F_VALIDAR_CNPJ(@Cnpj_Organizacao) = 'False'
		RAISERROR ('O CNPJ informado é inválido', 0, 0)

	SET @Cnpj_Organizacao = dbo.F_CONSIDERAR_SOMENTE_DIGITOS(@Cnpj_Organizacao)

	SELECT @Id_Membro = Id
		FROM VW_LISTAR_MEMBROS m
			INNER JOIN VW_LISTAR_ORGANIZACOES o
				ON m.OrganizacaoId = o.Id
	WHERE m.CodigoRegistro = @Codigo_Registro_Membro AND
		  o.Cnpj = @Cnpj_Organizacao
	
	IF @@ROWCOUNT = 0
		RAISERROR ('Não foi encontrado o membro com o código de registro e CNPJ da organização informados não foi localizado', 0, 0)

	SELECT @Id_Justificativa = justificativa_id
		FROM VW_LISTAR_JUSTIFICATIVAS
	WHERE Membro_Id = @Id_Membro AND
		  Data_Ocorrencia = @Data_Ocorrencia

	IF @@ROWCOUNT > 0
	BEGIN
		UPDATE Justificativa
		SET Possui_Comprovante = @Possui_Comprovante,
			Comentarios = @Comentarios,
			Alterado_Em = GETDATE()
		WHERE Id = @Id_Justificativa

		SELECT @Id_Justificativa Id
		RETURN
	END

	INSERT INTO Justificativa (Membro_Id, Data_Ocorrencia, Possui_Comprovante, Comentarios)
		VALUES (@Id_Membro, @Data_Ocorrencia, @Possui_Comprovante, @Comentarios)

	SELECT @@IDENTITY Id
END
GO

-- SP para excluir logicamente uma justificativa
CREATE OR ALTER PROC SP_EXCLUIR_JUSTIFICATIVA
	@Id_Justificativa BIGINT
AS
BEGIN
	UPDATE Justificativa
	SET Excluido = 'True',
		Alterado_Em = GETDATE()
	WHERE Id = @Id_Justificativa

	IF @@ROWCOUNT = 0
		RAISERROR ('Não foi localizada a justificativa com o id informado', 0, 0)
END
GO

-- criação da view para a listagem de justificativas
CREATE OR ALTER VIEW VW_LISTAR_JUSTIFICATIVAS
AS
	SELECT j.id justificativa_id,
		   j.data_ocorrencia,
		   j.possui_comprovante,
		   j.comentarios,
		   j.data_criacao,
		   j.alterado_em,
		   j.membro_id,
		   m.CodigoRegistro,
		   m.NomeMembro,
		   m.OrganizacaoId,
		   o.nome nome_organizacao,
		   o.cnpj
		FROM Justificativa j
			INNER JOIN VW_LISTAR_MEMBROS m
				ON j.Membro_Id = m.MembroId
			INNER JOIN VW_LISTAR_ORGANIZACOES o
				ON m.OrganizacaoId = o.Id
	WHERE j.Excluido = 'False'
GO

-- criação da proc para obter a justificativa
CREATE OR ALTER PROC SP_OBTER_JUSTIFICATIVA
	@Codigo_Registro_Membro VARCHAR(50),
	@Cnpj_Organizacao CHAR(14),
	@Data_Ocorrencia DATE
AS
BEGIN
	SELECT *
		FROM VW_LISTAR_JUSTIFICATIVAS
	WHERE CodigoRegistro = @Codigo_Registro_Membro
		AND cnpj = @Cnpj_Organizacao
		AND data_ocorrencia = @Data_Ocorrencia
END