USE [TopPuzzle]
GO
/****** Object:  StoredProcedure [dbo].[InsertNewScore]    Script Date: 17.02.2015 14:15:02 ******/
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
@PictureId int,
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
/****** Object:  Table [dbo].[Score]    Script Date: 17.02.2015 14:15:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Score](
	[UserId] [int] NOT NULL,
	[PictureId] [int] NOT NULL,
	[Complexity] [int] NOT NULL,
	[Time] [int] NOT NULL,
	[Date] [datetime] NOT NULL DEFAULT (getdate())
) ON [PRIMARY]

GO
