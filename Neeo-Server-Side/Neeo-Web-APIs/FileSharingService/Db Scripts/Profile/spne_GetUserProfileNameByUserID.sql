USE [XMPPDb]
GO
/****** Object:  StoredProcedure [dbo].[spne_GetUserProfileNameByUserID]    Script Date: 10/23/2014 2:30:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Zohaib Hanif
-- Create date: 05/06/2014 (MM/dd/yyyy)
-- Modification date: 10/12/2014
-- Description:	It gives the name of the user whose user name is given to it. If name does not exist then it returns the user's phone number
-- =============================================
CREATE PROCEDURE [dbo].[spne_GetUserProfileNameByUserID] 
	-- Add the parameters for the stored procedure here
	@userID nvarchar(64)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CASE WHEN (Name IS NULL) THEN '' ELSE Name END AS Name
	From ofUser
	WHERE username = @userID;
END
