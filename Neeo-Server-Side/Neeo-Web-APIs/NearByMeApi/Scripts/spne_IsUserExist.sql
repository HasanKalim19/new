USE [XMPPDb]
GO
/****** Object:  StoredProcedure [dbo].[spne_InsertSharedFileInformationByUserID]    Script Date: 3/11/2019 2:09:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Zohaib Hanif
-- Create date: 11-03-2019
-- Description:	This sp checks whether the user exists or not. If not it throws user not exists exception.
-- =============================================
CREATE PROCEDURE [dbo].[spne_IsUserExist]
	@username nvarchar(64)
AS
BEGIN
	DECLARE @InvalidUserCode int = 50404;

	SET NOCOUNT OFF;

	IF NOT EXISTS (SELECT 1 
			   FROM ofUser
			   WHERE username = @username)
	BEGIN
		RAISERROR(@InvalidUserCode,-1,-1);
	END;
END
