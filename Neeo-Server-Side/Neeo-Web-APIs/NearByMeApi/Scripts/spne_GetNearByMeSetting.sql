/****** Object:  StoredProcedure [dbo].[spne_GetUserBlockingStateByPhoneNumber]    Script Date: 3/11/2019 2:05:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Zohaib Hanif
-- Create date: 11/03/2019
-- Description:	The following procedure is to get the NearByMe settings for the user.
-- =============================================
CREATE PROCEDURE [dbo].[spne_GetNearByMeSetting] 
	-- Add the parameters for the stored procedure here
	@username nvarchar(64)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY
		EXEC spne_IsUserExist @username;
	END TRY
	BEGIN CATCH
		THROW 
	END CATCH

	IF NOT EXISTS (SELECT 1 
			   FROM [dbo].[neNearByMeSetting] 
			   WHERE [username] = @username)
	BEGIN
		INSERT INTO [dbo].[neNearByMeSetting]
           ([username],
			[enabled],
		    [createDate])
		VALUES
           (@username,
		    1,
		    GETUTCDATE());
	END
	
	SELECT [username]
      ,[enabled]
      ,[notificationTone]
      ,[notificationOn]
      ,[showInfo]
      ,[showProfileImage]
	  ,[isPrivateAccount]
  FROM [dbo].[neNearByMeSetting]
  WHERE [username] = @username;
END
