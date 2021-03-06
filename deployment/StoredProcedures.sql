USE [TopPuzzle]
GO
/****** Object:  StoredProcedure [dbo].[GetPictureById]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPictureById] 
@id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].Picture Where Id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[GetPictureByPictureId]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPictureByPictureId] 
@pictureId nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].Picture Where PictureId = @pictureId
END


GO
/****** Object:  StoredProcedure [dbo].[GetPictures]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetPictures] 
@startRowIndex INT,
@maximumIndex INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, PictureId, Picture FROM 
		(SELECT Id, PictureId, Picture, ROW_NUMBER() OVER (ORDER BY Id) AS RowRank FROM Picture) AS PictureWithRows 
	Where RowRank > @startRowIndex AND RowRank <= (@startRowIndex + @maximumIndex)
END



GO
/****** Object:  StoredProcedure [dbo].[GetPicturesCount]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPicturesCount]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT COUNT(*) FROM Picture
END



GO
/****** Object:  StoredProcedure [dbo].[GetScores]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetScores] 
@complexity int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 10 * FROM [dbo].[Score] WHERE Complexity = @complexity
	ORDER BY [Time]
END




GO
/****** Object:  StoredProcedure [dbo].[GetUserById]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserById]
@Id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Users] WHERE [Id] = @Id
END




GO
/****** Object:  StoredProcedure [dbo].[GetUserByLogin]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserByLogin]
@Login nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Users] WHERE [UserName] = @Login
END




GO
/****** Object:  StoredProcedure [dbo].[GetUserByLoginAndHash]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserByLoginAndHash] 
@Login nvarchar(50),
@PasswordHash int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * From [TopPuzzle].[dbo].Users WHERE UserName = @Login AND PasswordHash = @PasswordHash
END


GO
/****** Object:  StoredProcedure [dbo].[GetUserScoreById]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserScoreById] 
@userId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].Score WHERE UserId = @userId
END




GO
/****** Object:  StoredProcedure [dbo].[InsertPicture]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertPicture] 
@PictureId nvarchar(50),
@Picture nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;
	Insert into [dbo].Picture (PictureId, Picture) Values (@PictureId, @Picture)
	DECLARE @Id INT = SCOPE_IDENTITY()
	SELECT * FROM [Picture] WHERE Id = @Id
END




GO
/****** Object:  StoredProcedure [dbo].[InsertScore]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertScore]
@UserId int,
@PictureId int,
@Complexity int,
@Time int,
@Date datetime
AS
BEGIN
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
	DECLARE @id INT = SCOPE_IDENTITY()
	SELECT * FROM [dbo].[Score] WHERE [Id] = @id
END




GO
/****** Object:  StoredProcedure [dbo].[InsertUser]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertUser]
@UserName nvarchar(50),
@PasswordHash nvarchar(50),
@Email nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Users]
           ([UserName],
		   [PasswordHash],
		   [Email])
     VALUES
           (@UserName,
		   @PasswordHash,
		   @Email)
	DECLARE @id INT = SCOPE_IDENTITY()
	SELECT * FROM [dbo].[Users] WHERE Id = @id
END




GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAvatar]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUserAvatar]
@Id int,
@Avatar nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].Users SET 
		Avatar = @Avatar, 
		HasAvatar = 1
	WHERE Id = @Id
	SET @id = SCOPE_IDENTITY()
	SELECT * FROM [dbo].[Users] WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserEmail]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUserEmail]
@Id int,
@Email nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].Users SET 
		Email = @Email
	WHERE Id = @Id
	SET @id = SCOPE_IDENTITY()
	SELECT * FROM [dbo].[Users] WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserPassword]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUserPassword]
@Id int,
@PasswordHash int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].Users SET 
		PasswordHash = @PasswordHash
	WHERE Id = @Id
	SET @id = SCOPE_IDENTITY()
	SELECT * FROM [dbo].[Users] WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserRating]    Script Date: 20.03.2015 23:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUserRating]
@Id int,
@Rating int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].Users SET
		Rating = @Rating
	WHERE Id = @Id
	SET @id = SCOPE_IDENTITY()
	SELECT * FROM [dbo].[Users] WHERE Id = @id
END
GO
