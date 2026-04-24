USE [master];
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'M3_ProjectII')
BEGIN
    CREATE DATABASE [M3_ProjectII];
END
GO

ALTER DATABASE [M3_ProjectII] SET COMPATIBILITY_LEVEL = 160;
GO

USE [M3_ProjectII];
GO
-- Criar o procedimento que cria as tabelas
CREATE OR ALTER PROCEDURE [dbo].[Criar_Tabelas]	
AS
BEGIN
		PRINT 'A criar as tabelas...';
		-- Assuntos
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'Assuntos'
		)
		BEGIN
			CREATE TABLE [dbo].[Assuntos](
				[ID_Assunto] [tinyint] IDENTITY(1,1) NOT NULL,
				[Assunto] [nvarchar](50) NOT NULL,
			 CONSTRAINT [PK_Assuntos] PRIMARY KEY CLUSTERED 
			(
				[ID_Assunto] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
		END
		-- Exemplares
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'Exemplares'
		)
		BEGIN
			CREATE TABLE [dbo].[Exemplares](
				[ID_Exemplar] [bigint] IDENTITY(1,1) NOT NULL,
				[ID_Obra] [bigint] NOT NULL,
			 CONSTRAINT [PK_Exemplares] PRIMARY KEY CLUSTERED 
			(
				[ID_Exemplar] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
		END
		-- Exemplares_Nucleo
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'Exemplares_Nucleo'
		)
		BEGIN
			CREATE TABLE [dbo].[Exemplares_Nucleo](
				[ID_Nucleo] [bigint] NOT NULL,
				[ID_Exemplar] [bigint] NOT NULL,
			 CONSTRAINT [PK_Exemplares_Nucleo] PRIMARY KEY CLUSTERED 
			(
				[ID_Nucleo] ASC,
				[ID_Exemplar] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
		END
		-- TipoUtilizadores
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'TipoUtilizadores'
		)
		BEGIN
			CREATE TABLE [dbo].[TipoUtilizadores](
				[ID_TipoUtilizador] [int] IDENTITY(1,1) NOT NULL,
				[Perfil] [nvarchar](50) NOT NULL,
			 CONSTRAINT [PK_TipoUtilizadores] PRIMARY KEY CLUSTERED 
			(
				[ID_TipoUtilizador] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
		END
		-- Utilizadores
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'Utilizadores'
		)
		BEGIN
			CREATE TABLE [dbo].[Utilizadores](
				[ID_Utilizador] [bigint] IDENTITY(1,1) NOT NULL,
				[UserName] [nvarchar](50) NULL,
				[PassWord] [nvarchar](50) NULL,
				[Nome] [nvarchar](50) NULL,
				[Email] [nvarchar](255) NULL,
				[ID_TipoUtilizador] [int] NOT NULL,
				[Ativo] [bit] NOT NULL,
			 CONSTRAINT [PK_Utilizadores] PRIMARY KEY CLUSTERED 
			(
				[ID_Utilizador] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
		END
		-- Infracoes
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'Infracoes'
		)
		BEGIN
			CREATE TABLE [dbo].[Infracoes](
				[ID_Utilizador] [bigint] NOT NULL,
				[InfracoesTotal] [int] NOT NULL,
				[InfracoesAtuais] [int] NOT NULL,
			CONSTRAINT [PK_Infracoes] PRIMARY KEY CLUSTERED 
			(
				[ID_Utilizador] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
		END
		-- Nucleos
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'Nucleos'
		)
		BEGIN
			CREATE TABLE [dbo].[Nucleos](
				[ID_Nucleo] [bigint] IDENTITY(1,1) NOT NULL,
				[Nome] [nvarchar](50) NOT NULL,
				[Local] [nvarchar](50) NOT NULL,
				[ID_TipoNucleo] [tinyint] NOT NULL,
			 CONSTRAINT [PK_Nucleos] PRIMARY KEY CLUSTERED 
			(
				[ID_Nucleo] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
		END
		-- Obras
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'Obras'
		)
		BEGIN
			CREATE TABLE [dbo].[Obras](
				[ID_Obra] [bigint] IDENTITY(1,1) NOT NULL,
				[Autor] [nvarchar](50) NOT NULL,
				[ISBN] [nvarchar](50) NOT NULL,
				[Titulo] [nvarchar](50) NOT NULL,
				[Capa] [varbinary](max) NULL,
				[ID_Assunto] [tinyint] NOT NULL,
			 CONSTRAINT [PK_Obras] PRIMARY KEY CLUSTERED 
			(
				[ID_Obra] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
		END
		-- Requisicoes
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'Requisicoes'
		)
		BEGIN
			CREATE TABLE [dbo].[Requisicoes](
				[ID_Requisicao] [bigint] IDENTITY(1,1) NOT NULL,
				[ID_Utilizador] [bigint] NOT NULL,
				[ID_Exemplar] [bigint] NOT NULL,
				[DataRequisicao] [datetime] NOT NULL,
				[DataEntrega] [datetime] NULL,
			 CONSTRAINT [PK_Requisicoes] PRIMARY KEY CLUSTERED 
			(
				[ID_Requisicao] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
		END
		-- TipoNucleos
		IF NOT EXISTS ( SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
		  AND TABLE_NAME = 'TipoNucleos'
		)
		BEGIN
			CREATE TABLE [dbo].[TipoNucleos](
				[ID_TipoNucleo] [tinyint] IDENTITY(1,1) NOT NULL,
				[Descricao] [nvarchar](50) NOT NULL,
			 CONSTRAINT [PK_TipoNucleos] PRIMARY KEY CLUSTERED 
			(
				[ID_TipoNucleo] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY]
		END
END
GO

-- Chamar o procedimento que criar as tabelas
EXEC Criar_Tabelas
GO

-- Tabelas criadas podemos criar as relações sem receio de dar erro
-- Criar Procedimento de criar as relações
CREATE OR ALTER PROCEDURE [dbo].[Criar_Relacoes_Tabelas]
AS
BEGIN
PRINT 'A criar relacoes entre as tabelas...';
	
	-- FK_Infracoes_Utilizadores
	IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Infracoes_Utilizadores'
	)
	BEGIN
	ALTER TABLE [dbo].[Infracoes]  WITH CHECK ADD  CONSTRAINT [FK_Infracoes_Utilizadores] FOREIGN KEY([ID_Utilizador])
	REFERENCES [dbo].[Utilizadores] ([ID_Utilizador])
	ALTER TABLE [dbo].[Infracoes] CHECK CONSTRAINT [FK_Infracoes_Utilizadores]
	ALTER TABLE [dbo].[Infracoes] ADD  CONSTRAINT [DF_Infracoes_InfracoesTotal]  DEFAULT ((0)) FOR [InfracoesTotal]
	ALTER TABLE [dbo].[Infracoes] ADD  CONSTRAINT [DF_Infracoes_InfracoesAtuais]  DEFAULT ((0)) FOR [InfracoesAtuais]
	END

	-- FK_Utilizadores_TipoUtilizadores
	IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Utilizadores_TipoUtilizadores'
	)
	BEGIN
		ALTER TABLE [dbo].[Utilizadores]  WITH CHECK ADD  CONSTRAINT [FK_Utilizadores_TipoUtilizadores] FOREIGN KEY([ID_TipoUtilizador])
		REFERENCES [dbo].[TipoUtilizadores] ([ID_TipoUtilizador])
		ALTER TABLE [dbo].[Utilizadores] CHECK CONSTRAINT [FK_Utilizadores_TipoUtilizadores]
	END
	
	-- FK_Exemplares_Obras
	IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Exemplares_Obras'
	)
	BEGIN
		ALTER TABLE [dbo].[Exemplares]  WITH CHECK ADD  CONSTRAINT [FK_Exemplares_Obras] FOREIGN KEY([ID_Obra])
		REFERENCES [dbo].[Obras] ([ID_Obra])
		ALTER TABLE [dbo].[Exemplares] CHECK CONSTRAINT [FK_Exemplares_Obras]
	END

	-- FK_Exemplares_Nucleo_Exemplares
	IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Exemplares_Nucleo_Exemplares'
	)
	BEGIN
		ALTER TABLE [dbo].[Exemplares_Nucleo]  WITH CHECK ADD  CONSTRAINT [FK_Exemplares_Nucleo_Exemplares] FOREIGN KEY([ID_Exemplar])
		REFERENCES [dbo].[Exemplares] ([ID_Exemplar])
		ALTER TABLE [dbo].[Exemplares_Nucleo] CHECK CONSTRAINT [FK_Exemplares_Nucleo_Exemplares]

	END

	-- FK_Exemplares_Nucleo_Nucleos
	IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Exemplares_Nucleo_Nucleos'
	)
	BEGIN
		ALTER TABLE [dbo].[Exemplares_Nucleo]  WITH CHECK ADD  CONSTRAINT [FK_Exemplares_Nucleo_Nucleos] FOREIGN KEY([ID_Nucleo])
		REFERENCES [dbo].[Nucleos] ([ID_Nucleo])
		ALTER TABLE [dbo].[Exemplares_Nucleo] CHECK CONSTRAINT [FK_Exemplares_Nucleo_Nucleos]
	END
	
	-- FK_Nucleos_TipoNucleos
	IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Nucleos_TipoNucleos'
	)
	BEGIN
		ALTER TABLE [dbo].[Nucleos]  WITH CHECK ADD  CONSTRAINT [FK_Nucleos_TipoNucleos] FOREIGN KEY([ID_TipoNucleo])
		REFERENCES [dbo].[TipoNucleos] ([ID_TipoNucleo])
		ALTER TABLE [dbo].[Nucleos] CHECK CONSTRAINT [FK_Nucleos_TipoNucleos]
	END

	-- FK_Obras_Assuntos
	IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Obras_Assuntos'
	)
	BEGIN
		ALTER TABLE [dbo].[Obras]  WITH CHECK ADD  CONSTRAINT [FK_Obras_Assuntos] FOREIGN KEY([ID_Assunto])
		REFERENCES [dbo].[Assuntos] ([ID_Assunto])
		ALTER TABLE [dbo].[Obras] CHECK CONSTRAINT [FK_Obras_Assuntos]
	END

	-- FK_Requisicoes_Exemplares
	IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Requisicoes_Exemplares'
	)
	BEGIN
		ALTER TABLE [dbo].[Requisicoes]  WITH CHECK ADD  CONSTRAINT [FK_Requisicoes_Exemplares] FOREIGN KEY([ID_Exemplar])
		REFERENCES [dbo].[Exemplares] ([ID_Exemplar])
		ALTER TABLE [dbo].[Requisicoes] CHECK CONSTRAINT [FK_Requisicoes_Exemplares]
	END
	
	-- FK_Requisicoes_Utilizadores
	IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Requisicoes_Utilizadores'
	)
	BEGIN
		ALTER TABLE [dbo].[Requisicoes]  WITH CHECK ADD  CONSTRAINT [FK_Requisicoes_Utilizadores] FOREIGN KEY([ID_Utilizador])
		REFERENCES [dbo].[Utilizadores] ([ID_Utilizador])
		ALTER TABLE [dbo].[Requisicoes] CHECK CONSTRAINT [FK_Requisicoes_Utilizadores]
	END
END
GO

-- Chamar procedimento de criar as relações
EXEC Criar_Relacoes_Tabelas
GO

-- Com tabelas e relações presentes podemos popular com dados de amostragem
-- A ordem não é independente já que depende das relações criadas.
-- ainda assim a ordem deve ser mantida: Criar relações primeiro -> criar dados em segundo
-- para garantir que os dados seguem o modelo relacional
CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_Nucleos]
AS
BEGIN
	PRINT 'A criar amostra de nucleos...'
	IF EXISTS (SELECT 1 FROM [TipoNucleos]
	WHERE [Descricao] IN ('Sede Central','Núcleo Regional'))
		BEGIN
			PRINT 'As amostras de nucleos já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[TipoNucleos] ([Descricao]) VALUES 
			('Sede Central'),
			('Núcleo Regional');
		END
END
GO

EXEC Amostra_Criar_Nucleos
GO

CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_Assuntos]
AS
BEGIN
	PRINT 'A criar amostra de assuntos...'
	IF EXISTS (SELECT 1 FROM [Assuntos]
	WHERE [Assunto] IN ('Informática','Literatura', 'História', 'Ciências'))
		BEGIN
			PRINT 'As amostras de assuntos já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Assuntos] ([Assunto]) VALUES 
			('Informática'),
			('Literatura'),
			('História'),
			('Ciências');
		END
END
GO

EXEC Amostra_Criar_Assuntos
GO

CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_Nucleos]
AS
BEGIN
	PRINT 'A criar amostra de Nucleos...'
	IF EXISTS (SELECT 1 FROM [Nucleos]
	WHERE ([Nome] = 'Biblioteca Central' AND [Local] = 'Lisboa' AND [ID_TipoNucleo] = 1)
	OR ([Nome] = 'Núcleo Norte' AND [Local] = 'Porto' AND [ID_TipoNucleo] = 2)
	OR ([Nome] = 'Núcleo Sul' AND [Local] = 'Faro' AND [ID_TipoNucleo] = 2))
		BEGIN
			PRINT 'As amostras de Nucleos já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Nucleos] ([Nome], [Local], [ID_TipoNucleo]) VALUES 
			('Biblioteca Central', 'Lisboa', 1),
			('Núcleo Norte', 'Porto', 2),
			('Núcleo Sul', 'Faro', 2);
		END
END
GO

EXEC Amostra_Criar_Nucleos
GO

CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_Obras]
AS
BEGIN
	PRINT 'A criar amostra de Obras...'
	IF EXISTS (SELECT 1 FROM [Obras]
	WHERE [ISBN] IN ('978-1484230176', '978-0747532743', '978-9722100268', '978-0062316097'))
		BEGIN
			PRINT 'As amostras de Obras já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Obras] ([Autor], [ISBN], [Titulo], [Capa], [ID_Assunto]) VALUES 
			('Andrew Troelsen', '978-1484230176', 'Pro C# 10 with .NET 6', NULL, 1),
			('J.K. Rowling', '978-0747532743', 'Harry Potter e a Pedra Filosofal', NULL, 2),
			('Fernando Pessoa', '978-9722100268', 'Mensagem', NULL, 2),
			('Yuval Noah Harari', '978-0062316097', 'Sapiens', NULL, 3);
		END
END
GO

EXEC Amostra_Criar_Obras
GO

CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_Exemplares]
AS
BEGIN
	PRINT 'A criar amostra de Exemplares...'
	IF EXISTS (SELECT 1 FROM [Exemplares]
	WHERE [ID_Obra] IN (1, 2, 3))
		BEGIN
			PRINT 'As amostras de Exemplares já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Exemplares] ([ID_Obra]) VALUES (1), (1), (1), (1), (1), (1),
			(2), (2), (2), (2), (2), (2), (2),
			(3), (3), (3), (3), (3), (3), (3);
		END
END
GO

EXEC Amostra_Criar_Exemplares
GO

CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_Exemplares_Nucleos]
AS
BEGIN
	PRINT 'A criar amostra de Exemplares_Nucleos...'
	IF EXISTS (SELECT 1 FROM [Exemplares_Nucleo]
	WHERE ([ID_Nucleo] = 1 AND [ID_Exemplar] = 1)
	OR ([ID_Nucleo] = 1 AND [ID_Exemplar] = 4)
	OR ([ID_Nucleo] = 1 AND [ID_Exemplar] = 7)
	OR ([ID_Nucleo] = 2 AND [ID_Exemplar] = 2)
	OR ([ID_Nucleo] = 2 AND [ID_Exemplar] = 5)
	OR ([ID_Nucleo] = 2 AND [ID_Exemplar] = 8)
	OR ([ID_Nucleo] = 3 AND [ID_Exemplar] = 3)
	OR ([ID_Nucleo] = 3 AND [ID_Exemplar] = 6)
	)
		BEGIN
			PRINT 'As amostras de Exemplares_Nucleos já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Exemplares_Nucleo] ([ID_Nucleo], [ID_Exemplar]) VALUES 
			
			(1, 1), (2, 2), (3, 3), (1, 4), (2, 5), (3, 6),					-- Obra 1 distribuída
			(1, 7), (1, 8), (2, 9), (2, 10), (2, 11), (3, 12), (3, 13),		-- Obra 2 distribuída
			(1, 14), (1, 15), (2, 16), (2, 17), (2, 18), (3, 19), (3, 20)	-- Obra 3 distribuída
		END
END
GO

EXEC Amostra_Criar_Exemplares_Nucleos
GO

CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_TipoUtilizadores]
AS
BEGIN
PRINT 'A criar amostra de TipoUtilizadores...'
	IF EXISTS (SELECT 1 FROM TipoUtilizadores
	WHERE ID_TipoUtilizador IN (1, 2)
	)
		BEGIN
			PRINT 'As amostras de TipoUtilizadores já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
	BEGIN
	INSERT INTO [dbo].[TipoUtilizadores] ([Perfil]) VALUES 
			('Leitor'), ('Admin')
	END
END
GO

EXEC Amostra_Criar_TipoUtilizadores
GO


CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_Utilizadores]
AS
BEGIN
	PRINT 'A criar amostra de Utilizadores...'
	IF EXISTS (SELECT 1 FROM [Utilizadores]
	WHERE [Nome] IN ('João Silva', 'Maria Santos', 'Ricardo Sousa', 'Ana Oliveira', 'João Tomas', 'Jose Borrego')
	)
		BEGIN
			PRINT 'As amostras de Utilizadores já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Utilizadores] ([Nome], [ID_TipoUtilizador], [Ativo]) VALUES 
			('João Silva', 1, 1),    -- Utilizador Leitor regular
			('Maria Santos', 1, 1),  -- Utilizador Leitor regular
			('Ricardo Sousa', 1, 0), -- Suspenso (mais de 3 infrações) 
			('Ana Oliveira', 1, 1),  -- Utilizador Leitor para teste de inatividade
			('João Tomas', 1, 1), -- Utilizador Leitor com ultima requisição há mais de um ano
			('José Borrego', 2, 1);  -- Utilizador Admin
		END
END
GO

EXEC Amostra_Criar_Utilizadores
GO

CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_Infracoes]
AS
BEGIN
	PRINT 'A criar amostra de Infracoes...'
	IF EXISTS (SELECT 1 FROM Infracoes
	WHERE ID_Utilizador IN (1, 2, 3, 4, 5, 6)
	)
		BEGIN
			PRINT 'As amostras de Infracoes já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Infracoes] ([ID_Utilizador], InfracoesTotal, InfracoesAtuais) VALUES 
			(1, 0, 0),    
			(2, 0, 0),  
			(3, 0, 4), 
			(4, 0, 0), 
			(5, 0, 0)
		END
END
GO

EXEC Amostra_Criar_Infracoes
GO

CREATE OR ALTER PROCEDURE [dbo].[Amostra_Criar_Requisicoes]
AS
BEGIN
	PRINT 'A criar amostra de Requisicoes...'
	IF EXISTS (SELECT 1 FROM [Requisicoes]
	WHERE ([ID_Utilizador] = 1 AND [ID_Exemplar] = 1 AND [DataRequisicao] = '2026-01-24'
	OR [ID_Utilizador] = 1 AND [ID_Exemplar] = 4 AND [DataRequisicao] = '2026-01-22'
	OR [ID_Utilizador] = 2 AND [ID_Exemplar] = 2 AND [DataRequisicao] = '2026-01-16'
	OR [ID_Utilizador] = 2 AND [ID_Exemplar] = 5 AND [DataRequisicao] = '2025-12-01')
	)
		BEGIN
			PRINT 'As amostras de Requisicoes já se encontram na base de dados. Nenhum Registo adicionado.'
		END
	ELSE
		BEGIN
			INSERT INTO [dbo].[Requisicoes] ([ID_Utilizador], [ID_Exemplar], [DataRequisicao], [DataEntrega]) VALUES 
			-- Leitor 1: Requisitado há 12 dias (Devolver em breve - falta < 5 dias)
			(1, 1, '2026-01-24', NULL), 
			-- Leitor 1: Requisitado há 14 dias (Devolução URGENTE - falta < 3 dias)
			(1, 4, '2026-01-22', NULL),
			-- Leitor 2: Requisitado há 20 dias (ATRASO - passou dos 15 dias) 
			(2, 2, '2026-01-16', NULL),
			-- Histórico: Obra já entregue
			(2, 5, '2025-12-01', '2025-12-10'),
			-- Leitor com ultima requisição há mais de 10 anos
			(5, 1, '2000-12-01', '2000-12-31');

		END
END
GO

EXEC Amostra_Criar_Requisicoes
GO

-- Fim da criação de procedimentos de amostra que devem ser todos passiveis de serem 
-- corridos em segurança novamente sem introduzir novos dados (caso existam os dados de amostra)

-- Inicio de criação de procedimento adicionais que respondem explicitamente ou implicitamente
-- ao exigido no subject

CREATE OR ALTER PROCEDURE [dbo].[Obras_Remover] @ID_Obra BIGINT 
AS BEGIN 
SET NOCOUNT ON; -- Validação de integridade referencial 
IF EXISTS (SELECT 1 FROM Exemplares WHERE ID_Obra = @ID_Obra) 
BEGIN PRINT 'ERRO: Não é possível eliminar a obra. Existem exemplares físicos associados a esta obra.'; 
RETURN; 
END 
IF NOT EXISTS (SELECT 1 FROM Obras WHERE ID_Obra = @ID_Obra) 
BEGIN PRINT 'ERRO: A obra não existe.'; 
RETURN; 
END 
DELETE FROM Obras WHERE ID_Obra = @ID_Obra; PRINT 'Obra eliminada com sucesso.'; 
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Obras_Atualizar] 
@ID_Obra BIGINT, 
@Autor NVARCHAR(50), 
@ISBN NVARCHAR(50), 
@Titulo NVARCHAR(50), 
@ID_Assunto TINYINT, 
@Capa VARBINARY(MAX) = NULL -- Opcional, para não apagar a capa se não for enviada 
AS BEGIN SET NOCOUNT ON; 
IF NOT EXISTS (SELECT 1 FROM Obras WHERE ID_Obra = @ID_Obra) BEGIN PRINT 'ERRO: A obra com o ID ' + CAST(@ID_Obra AS VARCHAR) + ' não existe.'; 
RETURN; 
END 
UPDATE Obras SET Autor = @Autor, ISBN = @ISBN, Titulo = @Titulo, ID_Assunto = @ID_Assunto, Capa = ISNULL(@Capa, Capa) -- Mantém a imagem antiga se a nova for NULL 
WHERE ID_Obra = @ID_Obra; 
PRINT 'Obra ID ' + CAST(@ID_Obra AS VARCHAR) + ' atualizada com sucesso.'; 
END 
GO

CREATE OR ALTER PROCEDURE [dbo].[Obra_Pesquisar] @Termo NVARCHAR(100)
AS
BEGIN
    SELECT 
	o.ID_Obra,
	o.ISBN,
        O.Autor, 
        O.Titulo, 
        o.ID_Assunto
    FROM Obras O 
    JOIN Assuntos A ON O.ID_Assunto = A.ID_Assunto
    WHERE O.Titulo LIKE '%' + @Termo + '%' OR O.Autor LIKE '%' + @Termo + '%';
END
GO

CREATE OR ALTER   PROCEDURE [dbo].[Obras_ConsultarTotal] 
AS
BEGIN
/*
PARTE I. 1. Consultar o numero total de obras existentes
*/
	SET NOCOUNT ON;
	SELECT COUNT(*) AS [Número de Obras] 
	FROM Obras;
END
GO

CREATE OR ALTER   PROCEDURE [dbo].[Obras_TotalPorAssunto]
AS
BEGIN
	/*
	PARTE.I 2. Consultar o numero de obras existentes por tipo de assunto
	*/
	SET NOCOUNT ON;
	SELECT Assuntos.Assunto, COUNT(*) AS [Obras com este Assunto:]
	FROM Obras
	JOIN Assuntos ON Obras.ID_Assunto = Assuntos.ID_Assunto
	GROUP BY Assuntos.Assunto;
END
GO

CREATE OR ALTER   PROCEDURE [dbo].[Obras_Top10MaisRequisitadas]
	@DataInicio DATE,
	@DataFim DATE
AS
BEGIN
/*
PARTE.I 2. Consultar o numero de obras existentes por tipo de assunto 3. Consultar a lista das dez obras mais requisitadas num intervalo de tempo
por ordem descrescente
*/
	SET NOCOUNT ON;

	IF @DataFim < @DataInicio PRINT 'Erro: Datas inválidas'

	IF NOT EXISTS (SELECT TOP 10 Obras.Titulo, COUNT(*) AS [Requisições no Período:] 
	FROM Requisicoes
	JOIN Exemplares ON Requisicoes.ID_Exemplar = Exemplares.ID_Exemplar
	JOIN Obras ON Exemplares.ID_Obra = Obras.ID_Obra
	WHERE Requisicoes.DataRequisicao BETWEEN @DataInicio AND @DataFim
	GROUP BY Obras.Titulo
	ORDER BY COUNT(*) DESC)
    PRINT 'Atenção: Não foram encontradas requisições para o período selecionado.';
	
	ELSE
	SELECT TOP 10 Obras.Titulo, COUNT(*) AS [Requisições no Período:] 
	FROM Requisicoes
	JOIN Exemplares ON Requisicoes.ID_Exemplar = Exemplares.ID_Exemplar
	JOIN Obras ON Exemplares.ID_Obra = Obras.ID_Obra
	WHERE Requisicoes.DataRequisicao BETWEEN @DataInicio AND @DataFim
	GROUP BY Obras.Titulo
	ORDER BY COUNT(*) DESC;

END
GO

CREATE OR ALTER   PROCEDURE [dbo].[Nucleos_MostrarRequisicoes]
	@DataInicio DATE,
	@DataFim DATE
AS
BEGIN
	/*
	PARTE I. 4. Consultar a lista dos nucleos, por ordem decrescente de requisições num
	intervalo de tempo
	*/
	SET NOCOUNT ON;

	IF @DataFim < @DataInicio PRINT 'Erro: Datas inválidas'

	IF NOT EXISTS (SELECT Nucleos.Nome, Nucleos.Local, COUNT(*) AS [Requisições no Período:] 
	FROM Requisicoes
	JOIN Exemplares ON Requisicoes.ID_Exemplar = Exemplares.ID_Exemplar
	JOIN Exemplares_Nucleo ON Exemplares.ID_Exemplar = Exemplares_Nucleo.ID_Exemplar
	JOIN Nucleos ON Exemplares_Nucleo.ID_Nucleo = Nucleos.ID_Nucleo
	WHERE Requisicoes.DataRequisicao BETWEEN @DataInicio AND @DataFim
	GROUP BY Nucleos.Nome, Nucleos.Local)
    PRINT 'Atenção: Não foram encontradas requisições para o período selecionado.';
	

	ELSE
	SELECT Nucleos.Nome, Nucleos.Local, COUNT(*) AS [Requisições no Período:] 
	FROM Requisicoes
	JOIN Exemplares ON Requisicoes.ID_Exemplar = Exemplares.ID_Exemplar
	JOIN Exemplares_Nucleo ON Exemplares.ID_Exemplar = Exemplares_Nucleo.ID_Exemplar
	JOIN Nucleos ON Exemplares_Nucleo.ID_Nucleo = Nucleos.ID_Nucleo
	WHERE Requisicoes.DataRequisicao BETWEEN @DataInicio AND @DataFim
	GROUP BY Nucleos.Nome, Nucleos.Local
	ORDER BY COUNT(*) DESC;
END
GO

CREATE OR ALTER   PROCEDURE [dbo].[Obras_InserirNova]
	@Autor NVARCHAR(50), 
	@ISBN NVARCHAR(50), 
	@Titulo NVARCHAR(50), 
	@ID_Assunto TINYINT
AS
BEGIN
/*
PARTE I. 5. Acrescentar novas obras
*/
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM Obras WHERE ISBN = @ISBN)
	BEGIN
		PRINT 'ERRO: Já existe uma obra registada com o ISBN ' + @ISBN;
		RETURN;
	END

	INSERT INTO Obras (Autor, ISBN, Titulo, ID_Assunto) 
	VALUES (@Autor, @ISBN, @Titulo, @ID_Assunto);

	DECLARE @NovoID BIGINT = SCOPE_IDENTITY();
	PRINT 'Obra "' + @Titulo + '" registada com sucesso com o ID: ' + CAST(@NovoID AS VARCHAR);

END
GO

CREATE OR ALTER   PROCEDURE [dbo].[Obras_AdicionarExemplares]
    @ID_Obra BIGINT,
    @ID_Nucleo BIGINT,
    @Quantidade INT = 1  -- Valor padrão é 1, caso não seja especificado
AS
BEGIN
/*
PARTE.I 6. Atualizar o numero de exemplares de determinada obra
*/
    SET NOCOUNT ON;

    -- 1. Validação de Existência da Obra (Retirado da SP 1)
    IF NOT EXISTS (SELECT 1 FROM Obras WHERE ID_Obra = @ID_Obra)
    BEGIN
        PRINT 'ERRO: A Obra com ID ' + CAST(@ID_Obra AS VARCHAR) + ' não existe.';
        RETURN;
    END

    -- 2. Validação de Existência do Núcleo
    IF NOT EXISTS (SELECT 1 FROM Nucleos WHERE ID_Nucleo = @ID_Nucleo)
    BEGIN
        PRINT 'ERRO: O Núcleo com ID ' + CAST(@ID_Nucleo AS VARCHAR) + ' não existe.';
        RETURN;
    END

    -- 3. Validação da Quantidade
    IF @Quantidade <= 0
    BEGIN
        PRINT 'AVISO: A quantidade deve ser superior a zero.';
        RETURN;
    END

    -- 4. Ciclo de Inserção (Retirado da SP 2)
    DECLARE @Contador INT = 1;
    DECLARE @NovoExemplarID BIGINT;
    DECLARE @Titulo NVARCHAR(100) = (SELECT Titulo FROM Obras WHERE ID_Obra = @ID_Obra);

    WHILE @Contador <= @Quantidade
    BEGIN
        -- Inserir o exemplar "pai"
        INSERT INTO Exemplares (ID_Obra) VALUES (@ID_Obra);
        
        -- Capturar o ID gerado automaticamente
        SET @NovoExemplarID = SCOPE_IDENTITY();

        -- Associar ao núcleo na tabela de ligação (Crucial para o enunciado!)
        INSERT INTO Exemplares_Nucleo (ID_Exemplar, ID_Nucleo)
        VALUES (@NovoExemplarID, @ID_Nucleo);

        SET @Contador = @Contador + 1;
    END

    -- 5. Feedback detalhado (Estilo SP 1)
    DECLARE @TotalNoNucleo INT;
    SELECT @TotalNoNucleo = COUNT(*) 
    FROM Exemplares_Nucleo EN
    JOIN Exemplares E ON EN.ID_Exemplar = E.ID_Exemplar
    WHERE E.ID_Obra = @ID_Obra AND EN.ID_Nucleo = @ID_Nucleo;

    PRINT 'Sucesso: Foram adicionados ' + CAST(@Quantidade AS VARCHAR) + ' exemplares da obra "' + @Titulo + '".';
    PRINT 'Este núcleo possui agora um total de ' + CAST(@TotalNoNucleo AS VARCHAR) + ' exemplares desta obra.';
END
GO

CREATE OR ALTER   PROCEDURE [dbo].[Nucleos_TransferirExemplares]
	@ListaIDsExemplares NVARCHAR(MAX),
	@ID_NucleoDestino BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	/*
	PARTE I. 7. Transferir um ou mais exemplares de uma obra, de um nucleo para outro
	*/
	IF NOT EXISTS (SELECT 1 FROM Nucleos WHERE ID_Nucleo = @ID_NucleoDestino)
	BEGIN
		PRINT 'ERRO: O núcleo de destino (ID ' + CAST(@ID_NucleoDestino AS VARCHAR) + ') não existe.';
		RETURN;
	END

	DECLARE @ExemplaresATransferir TABLE (ID_Ex_Trans BIGINT);
	
	INSERT INTO @ExemplaresATransferir (ID_Ex_Trans)
	SELECT value FROM STRING_SPLIT(@ListaIDsExemplares, ',');

	DECLARE @ID_Atual BIGINT;
	DECLARE @ID_ObraAtual BIGINT;
	DECLARE @TituloObra NVARCHAR(100);
	DECLARE @ID_NucleoOrigem BIGINT;
	DECLARE @CountStock BIGINT;

	WHILE EXISTS (SELECT * FROM @ExemplaresATransferir)
	BEGIN
		SELECT TOP 1 @ID_Atual = ID_Ex_Trans FROM @ExemplaresATransferir;

		SELECT 
			@ID_ObraAtual = Exemplares.ID_Obra, 
			@ID_NucleoOrigem = Exemplares_Nucleo.ID_Nucleo,
			@TituloObra = O.Titulo
		FROM Exemplares
		JOIN Exemplares_Nucleo ON Exemplares.ID_Exemplar = Exemplares_Nucleo.ID_Exemplar
		JOIN Obras O ON Exemplares.ID_Obra = O.ID_Obra
		WHERE Exemplares.ID_Exemplar = @ID_Atual;

		IF @ID_NucleoOrigem IS NULL
		BEGIN
			PRINT 'AVISO: Exemplar ' + CAST(@ID_Atual AS VARCHAR) + ' não encontrado em nenhum núcleo. Ignorado.';
		END
		
		ELSE IF @ID_NucleoOrigem = @ID_NucleoDestino
		BEGIN
			PRINT 'AVISO: Exemplar ' + CAST(@ID_Atual AS VARCHAR) + ' já se encontra no núcleo de destino.';
		END
		ELSE
		BEGIN
			
			SELECT @CountStock = COUNT(*) 
			FROM Exemplares_Nucleo
			JOIN Exemplares ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
			WHERE Exemplares_Nucleo.ID_Nucleo = @ID_NucleoOrigem 
			  AND Exemplares.ID_Obra = @ID_ObraAtual;

			IF @CountStock > 1
			BEGIN
				
				UPDATE Exemplares_Nucleo
				SET ID_Nucleo = @ID_NucleoDestino
				WHERE ID_Exemplar = @ID_Atual;
				
				PRINT 'Sucesso: Exemplar ' + CAST(@ID_Atual AS VARCHAR) + ' ("' + @TituloObra + '") transferido para o núcleo ' + CAST(@ID_NucleoDestino AS VARCHAR) + '.';
			END
			ELSE
			BEGIN
				
				PRINT 'BLOQUEADO: O exemplar ' + CAST(@ID_Atual AS VARCHAR) + ' não pode sair do núcleo ' + CAST(@ID_NucleoOrigem AS VARCHAR) + 
					  '. É o último exemplar da obra "' + @TituloObra + '" disponível para consulta presencial.';
			END
		END

		DELETE FROM @ExemplaresATransferir WHERE ID_Ex_Trans = @ID_Atual;
	END
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Utilizador_CriarPorAdmin]
    @UserName NVARCHAR(50) = NULL,
    @PassWord NVARCHAR(50) = NULL,
    @Nome NVARCHAR(50),
    @Email    NVARCHAR(255) = NULL,
	@ID_TipoUtilizador INT

AS
BEGIN
/*
PARTE I. 8. Registar novos leitores
*/
    SET NOCOUNT ON;
	DECLARE @ID_Utilizador BIGINT

    IF @Nome IS NULL OR @Email IS NULL
    BEGIN
        PRINT 'Nome é obrigatório.'
    END
	ELSE
	BEGIN
    INSERT INTO Utilizadores (UserName, [PassWord], [Nome], Email, ID_TipoUtilizador, Ativo)
    VALUES (@Username, @Password, @Nome, @Email, @ID_Utilizador, 1);
	
	SET @ID_Utilizador = SCOPE_IDENTITY()

	INSERT INTO Infracoes (ID_Utilizador)
	VALUES (@ID_Utilizador)
	PRINT 'Utilizador adicionado'
	END
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Utilizadores_DesativarPorInfracoes]
AS 
BEGIN
/*
Parte I. 9.
Suspender o acesso a requisicões a leitores que tenham procedido a
devoluções atrasadas em mais que três ocasiões
*/
SELECT Utilizadores.ID_Utilizador
INTO #Alvos
FROM Utilizadores
JOIN Infracoes 
    ON Infracoes.ID_Utilizador = Utilizadores.ID_Utilizador
WHERE Utilizadores.Ativo <> 0
  AND Infracoes.InfracoesAtuais > 3;

-- Update users
UPDATE Utilizadores
SET Ativo = 0
FROM Utilizadores
JOIN #Alvos A
    ON A.ID_Utilizador = Utilizadores.ID_Utilizador;

-- Update infractions
UPDATE Infracoes
SET 
    InfracoesTotal = ISNULL(InfracoesTotal,0) + InfracoesAtuais,
    InfracoesAtuais = 0
FROM Infracoes
JOIN #Alvos A 
    ON A.ID_Utilizador = Infracoes.ID_Utilizador;

DROP TABLE #Alvos;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Utilizadores_Reativar]
@ID_Utilizador BIGINT
AS 
BEGIN
/*
Parte I. 10. 
Reativar o acesso a um leitor suspenso
*/
UPDATE Utilizadores
SET 
    Ativo = 1
WHERE Utilizadores.Ativo <> 0 
  AND Utilizadores.ID_Utilizador = @ID_Utilizador;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Utilizadores_Delete]
    @ID_Utilizador BIGINT
AS
BEGIN
/*
Esta stored procedure realiza a eliminação de um leitor com base no @ID_Utilizador.

Foi intencionalmente isolada como ponto central de DELETE para:
- Garantir a ordem correta de eliminação entre tabelas relacionadas
- Permitir reutilização por outras SPs
- Criar um ponto único de manutenção para futuras extensões
  (ex.: histórico, logging, estatísticas ou anonimização)

Nota:
- Esta SP executa apenas a eliminação física dos dados.
- A validação de regras de negócio deve ser feita pela SP chamadora.
*/
    SET NOCOUNT ON;

    DELETE FROM Infracoes
    WHERE [ID_Utilizador] = @ID_Utilizador;
    PRINT '@ID_Utilizador ' + CAST(@ID_Utilizador AS NVARCHAR(MAX)) + ' foi removido de Infracoes.'
    DELETE FROM Requisicoes
    WHERE [ID_Utilizador] = @ID_Utilizador;
    PRINT '@ID_Utilizador ' + CAST(@ID_Utilizador AS NVARCHAR(MAX)) + ' foi removido de Requisiçoes.'
    DELETE FROM Utilizadores
    WHERE [ID_Utilizador] = @ID_Utilizador;
    PRINT '@ID_Utilizador ' + CAST(@ID_Utilizador AS NVARCHAR(MAX)) + ' foi removido de Utilizadores.'
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Utilizador_ApagarInativos]
AS
BEGIN
/*
Parte I. 11. Eliminar leitores que estejam há mais de um ano sem fazer qualquer
requisição, desde que não tenham nenhuma requisição ativa nesse
momento
*/
    SET NOCOUNT ON;

    DECLARE @ID_Utilizador BIGINT;

    DECLARE utilizador_cursor CURSOR FOR
        SELECT R.ID_Utilizador
		FROM Requisicoes R
		INNER JOIN Utilizadores U
			ON R.ID_Utilizador = U.ID_Utilizador
		WHERE U.ID_TipoUtilizador = 1
		GROUP BY R.ID_Utilizador
		HAVING 
		-- No requisicoes in the last year
		SUM(CASE WHEN R.DataRequisicao >= DATEADD(YEAR, -1, GETDATE()) THEN 1 ELSE 0 END) = 0
		-- No requisicoes with DataEntrega IS NULL
		AND SUM(CASE WHEN R.DataEntrega IS NULL THEN 1 ELSE 0 END) = 0;

    OPEN utilizador_cursor;
    FETCH NEXT FROM utilizador_cursor INTO @ID_Utilizador;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC Utilizadores_Delete @ID_Utilizador;
        FETCH NEXT FROM utilizador_cursor INTO @ID_Utilizador;
    END

    CLOSE utilizador_cursor;
    DEALLOCATE utilizador_cursor;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Utilizador_CriarPorLeitor]
    @UserName NVARCHAR(50),
    @PassWord NVARCHAR(50),
    @Nome NVARCHAR(50) = NULL,
    @Email    NVARCHAR(255) = NULL
AS
BEGIN
	/*
	PARTE II 1. Fazer o registo de leitor
	*/
    SET NOCOUNT ON;
	DECLARE @ID_Utilizador BIGINT

    IF @UserName IS NULL OR @PassWord IS NULL
    BEGIN
        PRINT 'Nome de utilizador e password são necessários.'
    END
	ELSE
	BEGIN
    INSERT INTO Utilizadores (UserName, [PassWord], [Nome], Email, ID_TipoUtilizador, Ativo)
    VALUES (@Username, @Password, @Nome, @Email, 1, 1);
	
	SET @ID_Utilizador = SCOPE_IDENTITY()

	INSERT INTO Infracoes (ID_Utilizador)
	VALUES (@ID_Utilizador)
	PRINT 'Utilizador adicionado'
	END
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Utilizador_Cancelar]
@ID_Utilizador BIGINT
AS
BEGIN
/*
PARTE II. 2. Cancelar a inscrição, devendo assumir-se que, nesse caso, é feita a
devolução de todos os exemplares que possa ter requisitado e não tenha
ainda devolvido
*/
DECLARE @CurrentDate datetime = GETDATE() 


-- RECEIVE ALL OPEN BOOKS
UPDATE Requisicoes SET DataEntrega = @CurrentDate
WHERE ID_Utilizador = @ID_Utilizador 
AND DataEntrega IS NULL

-- COUNT INFRAÇÕES
UPDATE Infracoes 
SET InfracoesTotal += (
    SELECT COUNT(*) 
    FROM Requisicoes
    WHERE ID_Utilizador = @ID_Utilizador 
      AND DataEntrega = @CurrentDate
      AND DATEDIFF(DAY, DataRequisicao, DataEntrega) >= 15
)
WHERE ID_Utilizador = @ID_Utilizador;

UPDATE Utilizadores SET Ativo = 0 
WHERE Utilizadores.ID_Utilizador = @ID_Utilizador
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Disponibilidade_Exemplares_NucleoAssunto] 	
AS
BEGIN
/*
PARTE II. 3. Pesquisar as obras disponiveis, em geral ou por nucleo e/ou por tema
Esta SP deve ser chamada pela SP orquestradora Disponibilidade_Exemplares
consoante o valor dos argumentos passados a essa SP
*/
	SET NOCOUNT ON;
    -- Grouped by Núcleo + Assunto
    -------------------------------------------------------------------
    -- 1. Total exemplares por obra
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra, Nome, Assunto,
        COUNT(*) AS Total
    INTO #TotalExemplares
    FROM Exemplares_Nucleo
    INNER JOIN Exemplares
        ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Nucleos ON Nucleos.ID_Nucleo = Exemplares_Nucleo.ID_Nucleo
    INNER JOIN Obras ON Obras.ID_Obra = Exemplares.ID_Obra
    INNER JOIN Assuntos ON Assuntos.ID_Assunto = Obras.ID_Assunto
    GROUP BY Exemplares.ID_Obra, Nome,Assunto

    -- SELECT * FROM #TotalExemplares
    -------------------------------------------------------------------
    -- 2. Exemplares atualmente requisitados (não entregues)
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra, Nome, Assunto,
        COUNT(*) AS Requisitadas
    INTO #Requisitadas
    FROM Requisicoes
    INNER JOIN Exemplares
        ON Requisicoes.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Exemplares_Nucleo ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Nucleos ON Nucleos.ID_Nucleo = Exemplares_Nucleo.ID_Nucleo
    INNER JOIN Obras ON Obras.ID_Obra = Exemplares.ID_Obra
    INNER JOIN Assuntos ON Assuntos.ID_Assunto = Obras.ID_Assunto
    WHERE
        DataRequisicao IS NOT NULL
        AND DataEntrega IS NULL
    GROUP BY Exemplares.ID_Obra, Nome, Assunto;

    -- SELECT * FROM #Requisitadas
    -------------------------------------------------------------------
    -- 3. Exemplares reservados para presença (1 por núcleo)
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra, Nome, Assunto,
        COUNT(DISTINCT Exemplares_Nucleo.ID_Nucleo) AS PresencaObrigatoria
    INTO #Presenca
    FROM Exemplares_Nucleo
    INNER JOIN Exemplares
        ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Obras ON Exemplares.ID_Obra = Obras.ID_Obra
    INNER JOIN Nucleos ON Nucleos.ID_Nucleo = Exemplares_Nucleo.ID_Nucleo
    INNER JOIN Assuntos ON Assuntos.ID_Assunto = Obras.ID_Assunto
    GROUP BY Exemplares.ID_Obra, Nome, Assunto;

    -- Grouped by Núcleo + Assunto
SELECT
    o.Titulo,
    o.Autor,
    t.Nome AS Nucleo,
    t.Assunto,
    SUM(t.Total) AS Total,
    SUM(ISNULL(r.Requisitadas, 0)) AS Requisitadas,
    SUM(p.PresencaObrigatoria) AS PresencaObrigatoria,
    SUM(
        CASE 
            WHEN t.Total 
                 - ISNULL(r.Requisitadas, 0) 
                 - p.PresencaObrigatoria < 0 
            THEN 0
            ELSE t.Total 
                 - ISNULL(r.Requisitadas, 0) 
                 - p.PresencaObrigatoria
        END
    ) AS DisponiveisParaRequisicao
FROM Obras o
INNER JOIN #TotalExemplares t
    ON t.ID_Obra = o.ID_Obra
LEFT JOIN #Requisitadas r
    ON r.ID_Obra = t.ID_Obra
   AND r.Nome = t.Nome
   AND r.Assunto = t.Assunto
INNER JOIN #Presenca p
    ON p.ID_Obra = t.ID_Obra
   AND p.Nome = t.Nome
   AND p.Assunto = t.Assunto
GROUP BY
    o.Titulo,
    o.Autor,
    t.Nome,
    t.Assunto
ORDER BY
    t.Nome,
    t.Assunto,
    o.Titulo;

    DROP TABLE #Presenca
    DROP TABLE #Requisitadas
    DROP TABLE #TotalExemplares
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Disponibilidade_Exemplares_Assunto] 
AS
BEGIN
/*
PARTE II. 3. Pesquisar as obras disponiveis, em geral ou por nucleo e/ou por tema
Esta SP deve ser chamada pela SP orquestradora Disponibilidade_Exemplares
consoante o valor dos argumentos passados a essa SP
*/
	SET NOCOUNT ON;

    -- Grouped by Assunto
    -------------------------------------------------------------------
    -- 1. Total exemplares por obra
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra, Assunto,
        COUNT(*) AS Total
    INTO #TotalExemplares
    FROM Exemplares_Nucleo
    INNER JOIN Exemplares
        ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Obras ON Exemplares.ID_Obra = Obras.ID_Obra
    INNER JOIN Assuntos ON Obras.ID_Assunto = Assuntos.ID_Assunto
    GROUP BY Exemplares.ID_Obra, Assunto

    -- SELECT * FROM #TotalExemplares
    -------------------------------------------------------------------
    -- 2. Exemplares atualmente requisitados (não entregues)
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra, Assunto,
        COUNT(*) AS Requisitadas
    INTO #Requisitadas
    FROM Requisicoes
    INNER JOIN Exemplares
        ON Requisicoes.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Obras ON Obras.ID_Obra = Exemplares.ID_Obra
    INNER JOIN Assuntos ON Assuntos.ID_Assunto = Obras.ID_Assunto
    WHERE
        DataRequisicao IS NOT NULL
        AND DataEntrega IS NULL
    GROUP BY Exemplares.ID_Obra, Assunto;

    -- SELECT * FROM #Requisitadas
    -------------------------------------------------------------------
    -- 3. Exemplares reservados para presença (1 por núcleo)
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra, Assunto,
        COUNT(DISTINCT Exemplares_Nucleo.ID_Nucleo) AS PresencaObrigatoria
    INTO #Presenca
    FROM Exemplares_Nucleo
    INNER JOIN Exemplares
        ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Obras ON Exemplares.ID_Obra = Obras.ID_Obra
    INNER JOIN Assuntos ON Obras.ID_Assunto = Assuntos.ID_Assunto
    GROUP BY Exemplares.ID_Obra, Assunto;

    -- SELECT * FROM #Presenca
    -- Grouped by Assunto
        SELECT
            o.Titulo,
            o.Autor,
            a.Assunto,
            SUM(t.Total) AS Total,
            SUM(ISNULL(r.Requisitadas,0)) AS Requisitadas,
            SUM(p.PresencaObrigatoria) AS PresencaObrigatoria,
            SUM(
                CASE 
                    WHEN t.Total - ISNULL(r.Requisitadas,0) - p.PresencaObrigatoria < 0 THEN 0
                    ELSE t.Total - ISNULL(r.Requisitadas,0) - p.PresencaObrigatoria
                END
            ) AS DisponiveisParaRequisicao
        FROM Obras o
        INNER JOIN #TotalExemplares t ON t.ID_Obra = o.ID_Obra
        LEFT JOIN #Requisitadas r ON r.ID_Obra = o.ID_Obra
        INNER JOIN #Presenca p ON p.ID_Obra = o.ID_Obra
        INNER JOIN Assuntos a ON a.ID_Assunto = o.ID_Assunto
        GROUP BY o.Titulo, o.Autor, a.Assunto
        ORDER BY a.Assunto, o.Titulo;
    DROP TABLE #Presenca
    DROP TABLE #Requisitadas
    DROP TABLE #TotalExemplares
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Disponibilidade_Exemplares_Nucleo]	
AS
BEGIN
/*
PARTE II. 3. Pesquisar as obras disponiveis, em geral ou por nucleo e/ou por tema
Esta SP deve ser chamada pela SP orquestradora Disponibilidade_Exemplares
consoante o valor dos argumentos passados a essa SP
*/
	SET NOCOUNT ON;
    -- Grouped by Núcleo
     -------------------------------------------------------------------
    -- 1. Total exemplares por obra
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra, Nome,
        COUNT(*) AS Total
    INTO #TotalExemplares
    FROM Exemplares_Nucleo
    INNER JOIN Exemplares
        ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Nucleos ON Nucleos.ID_Nucleo = Exemplares_Nucleo.ID_Nucleo
    GROUP BY Exemplares.ID_Obra, Nome

    -- SELECT * FROM #TotalExemplares
    -------------------------------------------------------------------
    -- 2. Exemplares atualmente requisitados (não entregues)
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra, Nome,
        COUNT(*) AS Requisitadas
    INTO #Requisitadas
    FROM Requisicoes
    INNER JOIN Exemplares
        ON Requisicoes.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Exemplares_Nucleo ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
    INNER JOIN Nucleos ON Nucleos.ID_Nucleo = Exemplares_Nucleo.ID_Nucleo
    WHERE
        DataRequisicao IS NOT NULL
        AND DataEntrega IS NULL
    GROUP BY Exemplares.ID_Obra, Nome;

    -- SELECT * FROM #Requisitadas
    -------------------------------------------------------------------
    -- 3. Exemplares reservados para presença (1 por núcleo)
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra, Nome,
        COUNT(DISTINCT Exemplares_Nucleo.ID_Nucleo) AS PresencaObrigatoria
	INTO #Presenca
	FROM Exemplares_Nucleo
		INNER JOIN Exemplares
    ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
		INNER JOIN Obras ON Exemplares.ID_Obra = Obras.ID_Obra
		INNER JOIN Nucleos ON Nucleos.ID_Nucleo = Exemplares_Nucleo.ID_Nucleo
	GROUP BY Exemplares.ID_Obra, Nome;

    -- Grouped by Nucleo
       SELECT
    o.Titulo,o.Autor, t.Nome AS Nucleo,t.Total,
    ISNULL(r.Requisitadas, 0) AS Requisitadas,
    p.PresencaObrigatoria,
    CASE 
        WHEN t.Total
             - ISNULL(r.Requisitadas, 0)
             - p.PresencaObrigatoria < 0
        THEN 0
        ELSE
            t.Total
            - ISNULL(r.Requisitadas, 0)
            - p.PresencaObrigatoria
    END 
	AS DisponiveisParaRequisicao
	FROM Obras o
	INNER JOIN #TotalExemplares t
    ON t.ID_Obra = o.ID_Obra
	LEFT JOIN #Requisitadas r
    ON r.ID_Obra = t.ID_Obra
	AND r.Nome    = t.Nome
	INNER JOIN #Presenca p
    ON p.ID_Obra = t.ID_Obra
	AND p.Nome    = t.Nome
	ORDER BY t.Nome, o.Titulo;
    
	DROP TABLE #Presenca
    DROP TABLE #Requisitadas
    DROP TABLE #TotalExemplares
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Disponibilidade_Exemplares_Geral]
AS
BEGIN
/*
PARTE II. 3. Pesquisar as obras disponiveis, em geral ou por nucleo e/ou por tema
Esta SP deve ser chamada pela SP orquestradora Disponibilidade_Exemplares
consoante o valor dos argumentos passados a essa SP
*/
	SET NOCOUNT ON;
    -------------------------------------------------------------------
    -- 1. Total exemplares por obra
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra,
        COUNT(*) AS Total
    INTO #TotalExemplares
    FROM Exemplares_Nucleo
    INNER JOIN Exemplares
        ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
    GROUP BY Exemplares.ID_Obra;

    -- SELECT * FROM #TotalExemplares
    -- DROP TABLE #TotalExemplares
    -------------------------------------------------------------------
    -- 2. Exemplares atualmente requisitados (não entregues)
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra,
        COUNT(*) AS Requisitadas
    INTO #Requisitadas
    FROM Requisicoes
    INNER JOIN Exemplares
        ON Requisicoes.ID_Exemplar = Exemplares.ID_Exemplar
    WHERE
        DataRequisicao IS NOT NULL
        AND DataEntrega IS NULL
    GROUP BY Exemplares.ID_Obra;

    -- SELECT * FROM #Requisitadas
    -- DROP TABLE #Requisitadas
    -------------------------------------------------------------------
    -- 3. Exemplares reservados para presença (1 por núcleo)
    -------------------------------------------------------------------
    SELECT
        Exemplares.ID_Obra,
        COUNT(DISTINCT Exemplares_Nucleo.ID_Nucleo) AS PresencaObrigatoria
    INTO #Presenca
    FROM Exemplares_Nucleo
    INNER JOIN Exemplares
        ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
    GROUP BY Exemplares.ID_Obra;

    SELECT
        o.Titulo, o.Autor, t.Total, ISNULL(r.Requisitadas, 0) AS Requisitadas,
        p.PresencaObrigatoria,
        CASE
            WHEN t.Total
                 - ISNULL(r.Requisitadas, 0)
                 - p.PresencaObrigatoria < 0
            THEN 0
            ELSE
                t.Total
                - ISNULL(r.Requisitadas, 0)
                - p.PresencaObrigatoria
        END AS DisponiveisParaRequisicao
    FROM Obras o
    INNER JOIN #TotalExemplares t ON t.ID_Obra = o.ID_Obra
    LEFT JOIN #Requisitadas r ON r.ID_Obra = o.ID_Obra
    INNER JOIN #Presenca p ON p.ID_Obra = o.ID_Obra;
    
	DROP TABLE #Presenca
    DROP TABLE #Requisitadas
    DROP TABLE #TotalExemplares
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Disponibilidade_Exemplares]
    @GroupByNucleo BIT = 0,
    @GroupByAssunto BIT = 0
AS
BEGIN
/*
PARTE II. 3. Pesquisar as obras disponiveis, em geral ou por nucleo e/ou por tema
SP orquestradora que cosoante os argumentos  @GroupByNucleo e @GroupByAssunto
decide e executa a SP necessária
*/
	IF @GroupByAssunto = 1 AND @GroupByNucleo = 0
	BEGIN
		EXEC Disponibilidade_Exemplares_Assunto
	END
	ELSE IF @GroupByAssunto = 0 AND @GroupByNucleo = 1
	BEGIN
		EXEC Disponibilidade_Exemplares_Nucleo
    END
	ELSE IF @GroupByAssunto = 1 AND @GroupByNucleo = 1
	BEGIN
		EXEC Disponibilidade_Exemplares_NucleoAssunto
	END
	ELSE
	BEGIN
		EXEC Disponibilidade_Exemplares_Geral
	END
END
GO

CREATE OR ALTER PROCEDURE [dbo].[Utilizador_ConsultarEstadoRequisicoes]
    @ID_Utilizador BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validações
    DECLARE @TipoUtilizador INT;
    DECLARE @NomeUtilizador NVARCHAR(50);
    
    -- Verificar se o utilizador existe e qual o seu tipo (se é Leitor ou não)
    SELECT @TipoUtilizador = ID_TipoUtilizador, @NomeUtilizador = Nome 
    FROM Utilizadores 
    WHERE ID_Utilizador = @ID_Utilizador;

    IF @TipoUtilizador IS NULL
    BEGIN
        PRINT 'ERRO: O ID de utilizador indicado não existe.';
        RETURN;
    END

    IF @TipoUtilizador <> 1
    BEGIN
        PRINT 'AVISO: O utilizador ' + @NomeUtilizador + ' não é um Leitor (Tipo 1). Esta consulta é exclusiva para leitores.';
        RETURN;
    END

    -- 2. Verificar se o leitor tem requisições (Histórico vazio vs Nada pendente)
    IF NOT EXISTS (SELECT 1 FROM Requisicoes WHERE ID_Utilizador = @ID_Utilizador)
    BEGIN
        PRINT 'O leitor ' + @NomeUtilizador + ' não possui qualquer registo de requisições no sistema.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Requisicoes WHERE ID_Utilizador = @ID_Utilizador AND DataEntrega IS NULL)
    BEGIN
        PRINT 'O leitor ' + @NomeUtilizador + ' tem a sua situação regularizada (não possui obras pendentes de entrega).';
        RETURN;
    END

    -- Se passou todas as validações: Ver o Estado das requisições
    
    SELECT 
        N.Nome AS Núcleo,
        O.Titulo AS Obra,
        R.DataRequisicao AS [Data da Requisição],
        -- Cálculo do Prazo: Data da Requisição + 15 dias
        DATEADD(DAY, 15, R.DataRequisicao) AS [Data Limite],
        CASE 
            -- 1. Se a data atual for superior ao prazo calculado
            WHEN CAST(GETDATE() AS DATE) > DATEADD(DAY, 15, R.DataRequisicao)
                THEN 'ATRASO'
            
            -- 2. Se faltarem menos de 3 dias para o prazo (diferença entre 0 e 2 dias)
            WHEN DATEDIFF(DAY, GETDATE(), DATEADD(DAY, 15, R.DataRequisicao)) < 3 
                THEN 'Devolução URGENTE'
            
            -- 3. Se faltarem exatamente 5 dias ou estiver no intervalo de 3 a 5 dias
            WHEN DATEDIFF(DAY, GETDATE(), DATEADD(DAY, 15, R.DataRequisicao)) <= 5 
                THEN 'Devolver em breve'
            
            ELSE 'Dentro do Prazo'
        END AS Situacao
    FROM 
        Requisicoes R
    JOIN 
        Exemplares E ON R.ID_Exemplar = E.ID_Exemplar
    JOIN 
        Obras O ON E.ID_Obra = O.ID_Obra
    JOIN 
        Exemplares_Nucleo EN ON E.ID_Exemplar = EN.ID_Exemplar
    JOIN 
        Nucleos N ON EN.ID_Nucleo = N.ID_Nucleo
    JOIN
        Utilizadores U ON R.ID_Utilizador = U.ID_Utilizador
    WHERE 
        R.ID_Utilizador = @ID_Utilizador 
        AND R.DataEntrega IS NULL -- Filtra apenas as obras que ainda não foram devolvidas
        AND U.ID_TipoUtilizador = 1
    ORDER BY 
        N.Nome;

    PRINT 'Consulta realizada com sucesso para o leitor: ' + @NomeUtilizador;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[Utilizador_ConsultarRequisicoes] 
@ID_Utilizador AS bigint, 
@ID_Nucleo AS bigint = NULL, 
@IntervaloInicio AS DATE = NULL, 
@IntervaloFim AS DATE = NULL

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Nome_Utilizador AS NVARCHAR(50) = ISNULL((SELECT Nome FROM Utilizadores WHERE ID_Utilizador = @ID_Utilizador), 'Desconhecido')

	-- Mostrar todas as requsições
	IF @ID_Nucleo IS NULL AND @IntervaloInicio IS NULL AND @IntervaloFim IS NULL
	BEGIN

	SELECT Obras.Titulo, Obras.Autor, Requisicoes.DataRequisicao, Requisicoes.DataEntrega 
	FROM Requisicoes
	JOIN Exemplares ON Exemplares.ID_Exemplar = Requisicoes.ID_Exemplar
	JOIN Obras ON Obras.ID_Obra = Exemplares.ID_Obra
	WHERE Requisicoes.ID_Utilizador = @ID_Utilizador
	
	PRINT 'A Apresentar todas as requisições feitas pelo Utilizador ' + @Nome_Utilizador + ' (ID: ' +  CAST(@ID_Utilizador AS NVARCHAR(50)) + ').';

	END;

	-- Agrupar por núcleo
	DECLARE @Nome_Nucleo AS NVARCHAR(50) = ISNULL((SELECT Nome FROM Nucleos WHERE ID_Nucleo = @ID_Nucleo), 'Desconhecido')
	IF @ID_Nucleo IS NOT NULL AND @IntervaloInicio IS NULL AND @IntervaloFim IS NULL
	BEGIN

	SELECT Nucleos.Nome, Nucleos.Local, Obras.Titulo, Obras.Autor, Requisicoes.DataRequisicao, Requisicoes.DataEntrega 
	FROM Requisicoes
	JOIN Exemplares ON Exemplares.ID_Exemplar = Requisicoes.ID_Exemplar
	JOIN Obras ON Obras.ID_Obra = Exemplares.ID_Obra
	JOIN Exemplares_Nucleo ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
	JOIN Nucleos ON Nucleos.ID_Nucleo = Exemplares_Nucleo.ID_Nucleo
	WHERE Requisicoes.ID_Utilizador = @ID_Utilizador
	ORDER BY Nucleos.Nome
	
	PRINT 'A Apresentar as requisições feitas pelo Utilizador ' + @Nome_Utilizador + ' (ID: ' +  CAST(@ID_Utilizador AS NVARCHAR(50)) + ') no Núcleo ' + @Nome_Nucleo + '.'

	END;

	-- Agrupar por intervalo
	IF @IntervaloInicio IS NOT NULL AND @IntervaloFim IS NOT NULL AND @ID_Nucleo IS NULL
	BEGIN

	SELECT Obras.Titulo, Obras.Autor, Requisicoes.DataRequisicao, Requisicoes.DataEntrega 
	FROM Requisicoes
	JOIN Exemplares ON Exemplares.ID_Exemplar = Requisicoes.ID_Exemplar
	JOIN Obras ON Obras.ID_Obra = Exemplares.ID_Obra
	WHERE Requisicoes.ID_Utilizador = @ID_Utilizador AND Requisicoes.DataRequisicao >= @IntervaloInicio AND Requisicoes.DataRequisicao <= @IntervaloFim
	ORDER BY Requisicoes.DataRequisicao
	
	PRINT 'A Apresentar as requisições feitas pelo Utilizador ' + @Nome_Utilizador + ' (ID: ' +  CAST(@ID_Utilizador AS NVARCHAR(50)) + ') entre os dias ' + CAST(@IntervaloInicio AS NVARCHAR(50)) + ' e ' + CAST(@IntervaloFim AS NVARCHAR(50)) + '.'

	END;

	-- Agrupar por Nucleo e por Intervalo
	IF @ID_Nucleo IS NOT NULL AND @IntervaloInicio IS NOT NULL AND @IntervaloFim IS NOT NULL
	BEGIN

	SELECT Nucleos.Nome, Nucleos.Local, Obras.Titulo, Obras.Autor, Requisicoes.DataRequisicao, Requisicoes.DataEntrega 
	FROM Requisicoes
	JOIN Exemplares ON Exemplares.ID_Exemplar = Requisicoes.ID_Exemplar
	JOIN Obras ON Obras.ID_Obra = Exemplares.ID_Obra
	JOIN Exemplares_Nucleo ON Exemplares_Nucleo.ID_Exemplar = Exemplares.ID_Exemplar
	JOIN Nucleos ON Nucleos.ID_Nucleo = Exemplares_Nucleo.ID_Nucleo
	WHERE Requisicoes.ID_Utilizador = @ID_Utilizador AND Requisicoes.DataRequisicao >= @IntervaloInicio AND Requisicoes.DataRequisicao <= @IntervaloFim
	ORDER BY Nucleos.Nome, Requisicoes.DataRequisicao

	PRINT 'A Apresentar as requisições feitas pelo Utilizador ' + @Nome_Utilizador + ' (ID: ' + CAST(@ID_Utilizador AS NVARCHAR(50)) + ') no Núcleo ' + @Nome_Nucleo + ' entre os dias ' + CAST(@IntervaloInicio AS NVARCHAR(50)) + ' e ' + CAST(@IntervaloFim AS NVARCHAR(50)) + '.'

	END;

	-- Caso o utilizador seja inválido
	IF @ID_Utilizador NOT IN (SELECT ID_Utilizador FROM Requisicoes)
	PRINT 'Não foi encontrada qualquer requisição associada ao utilizador indicado, pelo que não foi possível apresentar o histórico de requisições.';

END
GO

-- Inicio da secção de SP's que asseguram premissas mas nã oteêm correspondencia com
-- Pontos expecificos no subject

CREATE OR ALTER PROCEDURE [dbo].[Utilizador_Requisitar]
    @ID_Utilizador BIGINT,
    @ID_Exemplar BIGINT
AS
BEGIN
/*
Premissa assegurada:
5. Cada leitor pode ter requisitados, em cada momento, um máximo de
quatro exemplares
*/
    SET NOCOUNT ON;

    -- Validações
    DECLARE @ID_TipoUtilizador INT;
    DECLARE @ID_Obra BIGINT;
    DECLARE @ID_Nucleo BIGINT;
    DECLARE @TotalAtivas INT;
    DECLARE @ExemplaresNoNucleo INT;

    -- Validar se o Utilizador existe e se é Leitor (Tipo 1)
    SELECT @ID_TipoUtilizador = ID_TipoUtilizador FROM Utilizadores WHERE ID_Utilizador = @ID_Utilizador;

    IF @ID_TipoUtilizador IS NULL
    BEGIN
        PRINT 'ERRO: Utilizador não encontrado.';
        RETURN;
    END

    IF @ID_TipoUtilizador <> 1
    BEGIN
        PRINT 'ERRO: Apenas utilizadores do tipo Leitor podem requisitar obras.';
        RETURN;
    END

    -- Validar Limite de 4 exemplares
    SELECT @TotalAtivas = COUNT(*) 
    FROM Requisicoes 
    WHERE ID_Utilizador = @ID_Utilizador AND DataEntrega IS NULL;

    IF @TotalAtivas >= 4
    BEGIN
        PRINT 'ERRO: O leitor já atingiu o limite máximo de 4 exemplares ativos.';
        RETURN;
    END

    -- Identificar Obra e Núcleo do exemplar pretendido
    SELECT @ID_Obra = E.ID_Obra, @ID_Nucleo = EN.ID_Nucleo
    FROM Exemplares E
    JOIN Exemplares_Nucleo EN ON E.ID_Exemplar = EN.ID_Exemplar
    WHERE E.ID_Exemplar = @ID_Exemplar;

    -- Validar Stock Mínimo para Consulta Presencial
    SELECT @ExemplaresNoNucleo = COUNT(*)
    FROM Exemplares_Nucleo EN
    JOIN Exemplares E ON EN.ID_Exemplar = E.ID_Exemplar
    WHERE E.ID_Obra = @ID_Obra AND EN.ID_Nucleo = @ID_Nucleo;

    IF @ExemplaresNoNucleo <= 1
    BEGIN
        PRINT 'ERRO: Este exemplar é o único disponível neste núcleo e deve ser mantido para consulta presencial.';
        RETURN;
    END

    --Validar se o exemplar já não está requisitado por outra pessoa
    IF EXISTS (SELECT 1 FROM Requisicoes WHERE ID_Exemplar = @ID_Exemplar AND DataEntrega IS NULL)
    BEGIN
        PRINT 'ERRO: Este exemplar já se encontra requisitado.';
        RETURN;
    END

    -- Efetuar a Requisição
    INSERT INTO Requisicoes (ID_Utilizador, ID_Exemplar, DataRequisicao, DataEntrega)
    VALUES (@ID_Utilizador, @ID_Exemplar, GETDATE(), NULL);

    PRINT 'Sucesso: Requisição registada com êxito!';
END
GO

