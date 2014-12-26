USE [TopPuzzle]
GO
/****** Object:  User [IIS APPPOOL\toppuzzle.ru]    Script Date: 26.12.2014 15:35:34 ******/
CREATE USER [IIS APPPOOL\toppuzzle.ru] FOR LOGIN [IIS APPPOOL\toppuzzle.ru] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\toppuzzle.ru]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\toppuzzle.ru]
GO
/****** Object:  StoredProcedure [dbo].[FindAccountById]    Script Date: 26.12.2014 15:35:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindAccountById]
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * FROM [Users] WHERE [Id] = @Id
END

GO
/****** Object:  StoredProcedure [dbo].[FindAccountByUserName]    Script Date: 26.12.2014 15:35:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindAccountByUserName]
@UserName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * FROM [Users] WHERE [UserName] = @UserName
END

GO
/****** Object:  StoredProcedure [dbo].[InsertNewAccount]    Script Date: 26.12.2014 15:35:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertNewAccount]
@UserName nvarchar(50),
@Password nvarchar(50),
@Email nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Users]
           ([UserName],
		   [Password],
		   [Email])
     VALUES
           (@UserName,
		   @Password,
		   @Email)
END

GO
/****** Object:  StoredProcedure [dbo].[InsertNewScore]    Script Date: 26.12.2014 15:35:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertNewScore]
@UserId int,
@PuzzleId int,
@Complexity int,
@Time int,
@Date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Score]
           ([UserId]
           ,[PuzzleId]
           ,[Complexity]
           ,[Time]
           ,[Date])
     VALUES
           (@UserId,
		   @PuzzleId,
		   @Complexity,
		   @Time,
		   @Date)
END

GO
/****** Object:  Table [dbo].[Score]    Script Date: 26.12.2014 15:35:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Score](
	[UserId] [int] NOT NULL,
	[PuzzleId] [int] NOT NULL,
	[Complexity] [int] NOT NULL,
	[Time] [int] NOT NULL,
	[Date] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 26.12.2014 15:35:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Name] [nchar](10) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Score] ADD  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Score]  WITH CHECK ADD  CONSTRAINT [FK_Score_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Score] CHECK CONSTRAINT [FK_Score_Users]
GO
