USE [master]
GO
/****** Object:  Database [TopPuzzle]    Script Date: 03.03.2015 3:16:52 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = 'TopPuzzle')
BEGIN
	ALTER DATABASE [TopPuzzle] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [TopPuzzle]
END
GO

CREATE DATABASE [TopPuzzle]
GO

USE [TopPuzzle]
GO
/****** Object:  User [IIS APPPOOL\toppuzzle.ru]    Script Date: 03.03.2015 3:16:52 ******/
CREATE USER [IIS APPPOOL\toppuzzle.ru] FOR LOGIN [IIS APPPOOL\toppuzzle.ru] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\toppuzzle.ru]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\toppuzzle.ru]
GO

GO
/****** Object:  Table [dbo].[Picture]    Script Date: 19.03.2015 22:57:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Picture](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PictureId] [nvarchar](50) NOT NULL,
	[Picture] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Picture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Score]    Script Date: 19.03.2015 22:57:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Score](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PictureId] [int] NOT NULL,
	[Complexity] [int] NOT NULL,
	[Time] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Score] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  Table [dbo].[Users]    Script Date: 20.03.2015 23:50:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[PasswordHash] [int] NOT NULL,
	[Email] [nvarchar](50) NULL,
	[HasAvatar] [bit] NOT NULL,
	[Avatar] [nvarchar](500) NULL,
	[Rating] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Score] ADD  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [HasAvatar]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Rating]
GO

GO
USE [master]
GO
ALTER DATABASE [TopPuzzle] SET  READ_WRITE 
GO