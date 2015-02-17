USE [master]
GO

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
/****** Object:  User [IIS APPPOOL\toppuzzle.ru]    Script Date: 17.02.2015 23:08:56 ******/
CREATE USER [IIS APPPOOL\toppuzzle.ru] FOR LOGIN [IIS APPPOOL\toppuzzle.ru] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\toppuzzle.ru]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\toppuzzle.ru]
GO
/****** Object:  StoredProcedure [dbo].[GetAllPictures]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllPictures] 
@startRowIndex INT,
@maximumIndex INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id, PictureId, Picture FROM 
		(SELECT Id, PictureId, Picture, ROW_NUMBER() OVER (ORDER BY Id) AS RowRank FROM Picture) AS PictureWithRows 
	Where RowRank > @startRowIndex AND RowRank <= (@startRowIndex + @maximumIndex)
END

GO
/****** Object:  StoredProcedure [dbo].[GetPictureByPictureId]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPictureByPictureId] 
	-- Add the parameters for the stored procedure here
@Id nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM [dbo].Picture Where PictureId = @Id
END


GO
/****** Object:  StoredProcedure [dbo].[GetPuzzlesByUserId]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPuzzlesByUserId] 
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT [Puzzle].*
  FROM [dbo].[Puzzle] INNER JOIN [dbo].[UserPuzzle] ON UserId = @Id AND PuzzleId = [Puzzle].Id
END


GO
/****** Object:  StoredProcedure [dbo].[GetScores]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetScores] 
	@complexity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 100 * FROM [dbo].[Score] WHERE Complexity = @complexity
	ORDER BY Time
END


GO
/****** Object:  StoredProcedure [dbo].[GetTotalPicturesCount]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetTotalPicturesCount]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) FROM Picture
END

GO
/****** Object:  StoredProcedure [dbo].[GetUserById]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserById]
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * FROM [Users] WHERE [Id] = @Id
END


GO
/****** Object:  StoredProcedure [dbo].[GetUserByLogin]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserByLogin]
@Login nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * FROM [Users] WHERE [UserName] = @Login
END


GO
/****** Object:  StoredProcedure [dbo].[GetUserByLoginAndPassword]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserByLoginAndPassword] 
@Login nvarchar(50),
@Password nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT * From [TopPuzzle].[dbo].Users WHERE UserName = @Login AND Password = @Password
END


GO
/****** Object:  StoredProcedure [dbo].[GetUserNameById]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserNameById] 
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT UserName FROM [dbo].[Users] WHERE Id = id
END


GO
/****** Object:  StoredProcedure [dbo].[GetUserScoreById]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserScoreById] 
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM [dbo].Score WHERE UserId = @Id
END


GO
/****** Object:  StoredProcedure [dbo].[InsertNewScore]    Script Date: 17.02.2015 23:08:56 ******/
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
@PictureId nvarchar(MAX),
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
           ,[PictureId]
           ,[Complexity]
           ,[Time]
           ,[Date])
     VALUES
           (@UserId,
		   @PictureId,
		   @Complexity,
		   @Time,
		   @Date)
END


GO
/****** Object:  StoredProcedure [dbo].[InsertNewUser]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertNewUser]
@UserName nvarchar(50),
@Password nvarchar(50),
@Email nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @id INT
	INSERT INTO [dbo].[Users]
           ([UserName],
		   [Password],
		   [Email])
     VALUES
           (@UserName,
		   @Password,
		   @Email)
	SET @id = SCOPE_IDENTITY()
	SELECT * FROM [dbo].[Users] WHERE Id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[SavePicture]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SavePicture] 
	-- Add the parameters for the stored procedure here
@PictureId nvarchar(50),
@Picture nvarchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into [dbo].Picture (PictureId, Picture) Values (@PictureId, @Picture)
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateUser]
@Id int,
@Email nvarchar(50),
@Password nvarchar(50),
@Avatar nvarchar(500),
@HasAvatar bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].Users SET 
		Email = @Email, 
		Password = @Password,
		Avatar = @Avatar, 
		HasAvatar = @HasAvatar
	WHERE Id = @Id
END


GO
/****** Object:  Table [dbo].[Picture]    Script Date: 17.02.2015 23:08:56 ******/
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
/****** Object:  Table [dbo].[Score]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Score](
	[UserId] [int] NOT NULL,
	[PictureId] [nvarchar](max) NOT NULL,
	[Complexity] [int] NOT NULL,
	[Time] [int] NOT NULL,
	[Date] [datetime] NOT NULL DEFAULT (getdate())
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 17.02.2015 23:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[HasAvatar] [bit] NOT NULL DEFAULT ((0)),
	[Avatar] [nvarchar](500) NULL,
	[Rating] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [TopPuzzle] SET  READ_WRITE 
GO
