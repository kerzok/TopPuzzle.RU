USE [TopPuzzle]
GO
/****** Object:  StoredProcedure [dbo].[GetAllPictures]    Script Date: 13.01.2015 19:07:36 ******/
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
/****** Object:  StoredProcedure [dbo].[GetTotalPicturesCount]    Script Date: 13.01.2015 19:07:37 ******/
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
