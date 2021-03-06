USE [XMPPDb]
GO
/****** Object:  StoredProcedure [dbo].[spne_GetUserGroupsDetails]    Script Date: 6/15/2015 3:11:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Zohaib Hainf
-- Create date: 14-May-2015
-- Description:	Gets the user's joined groups with the participants and group details
-- =============================================
CREATE PROCEDURE [dbo].[spne_GetUserGroupsDetailsByUserID] 
	@userJid nvarchar(64)
AS
BEGIN
	DECLARE @InvalidUserCode int = 50404;
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	IF EXISTS ( SELECT 1 FROM ofUser WHERE username = dbo.GetIdFromJid(@userJid))
	BEGIN
		SELECT i.roomID, i.memberCount
		INTO #temp
		FROM (SELECT ofMucAffiliationExtension.roomID, (SELECT COUNT(1) FROM ofMucMember WHERE roomID = ofMucAffiliationExtension.roomID)  AS memberCount
			  FROM ofMucAffiliationExtension 
			  WHERE username = dbo.GetIdFromJid(@userJid) 
			  AND affiliation = 10   
		UNION ALL
		SELECT roomID, (SELECT COUNT(1) FROM ofMucMember WHERE roomID = ofMucAffiliation.roomID)  AS memberCount
		FROM  ofMucAffiliation 
		WHERE jid = @userJid AND affiliation = 20) AS i;

		SELECT ofMucRoom.roomID, ofMucRoom.name, ofMucRoom.subject, ofMucRoom.creationDate, ofMucAffiliationExtension.username AS [admin],
		COALESCE((SELECT [dbo].[GetIdFromJid]([jid]) + ',' 
				  FROM [ofMucAffiliation] 
				  WHERE [roomID] = [ofMucRoom].[roomID] AND affiliation = 20 FOR XML PATH('')),'') 
				  AS [participants]
		FROM ofMucRoom
		INNER JOIN ofMucAffiliationExtension 
		ON ofMucRoom.roomID = ofMucAffiliationExtension.roomID
		WHERE ofMucRoom.roomID in (SELECT roomID FROM #temp WHERE memberCount = 0) AND ofMucRoom.subject != '';
		DROP table #temp;
	END
	ELSE
		RAISERROR(@InvalidUserCode,-1,-1);
END
