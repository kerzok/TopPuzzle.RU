USE [TopPuzzle]
GO
/****** Object:  User [IIS APPPOOL\toppuzzle.ru]    Script Date: 07.01.2015 18:48:40 ******/
CREATE USER [IIS APPPOOL\toppuzzle.ru] FOR LOGIN [IIS APPPOOL\toppuzzle.ru] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\toppuzzle.ru]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\toppuzzle.ru]
GO
/****** Object:  StoredProcedure [dbo].[GetPuzzlesByUserId]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  StoredProcedure [dbo].[GetScores]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  StoredProcedure [dbo].[GetUserById]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  StoredProcedure [dbo].[GetUserByLogin]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  StoredProcedure [dbo].[GetUserByLoginAndPassword]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  StoredProcedure [dbo].[GetUserNameById]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  StoredProcedure [dbo].[GetUserScoreById]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  StoredProcedure [dbo].[InsertNewScore]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  StoredProcedure [dbo].[InsertNewUser]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 07.01.2015 18:48:40 ******/
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
/****** Object:  Table [dbo].[AnonymUser]    Script Date: 07.01.2015 18:48:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnonymUser](
	[Id] [int] NOT NULL,
	[UniqueId] [nvarchar](64) NOT NULL,
	[LastRequestDate] [datetime] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[IP] [nvarchar](64) NULL,
	[Url] [nvarchar](4000) NOT NULL,
	[Refer] [nvarchar](4000) NULL,
	[UserAgent] [nvarchar](4000) NULL,
 CONSTRAINT [PK_AnonymUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Puzzle]    Script Date: 07.01.2015 18:48:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Puzzle](
	[Id] [int] NOT NULL,
	[Picture] [nvarchar](500) NOT NULL,
	[Complexity] [int] NOT NULL,
	[Partition] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Puzzle] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Score]    Script Date: 07.01.2015 18:48:41 ******/
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
/****** Object:  Table [dbo].[UserPuzzle]    Script Date: 07.01.2015 18:48:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPuzzle](
	[UserId] [int] NOT NULL,
	[PuzzleId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 07.01.2015 18:48:41 ******/
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
ALTER TABLE [dbo].[Score] ADD  DEFAULT (getdate()) FOR [Date]
GO
