USE [XMPPDb]
GO
/****** Object:  StoredProcedure [dbo].[spne_GetGroupParticipantsDataByRoomIDnSenderID]    Script Date: 12/31/2014 4:48:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Zohaib Hanif
-- Create date: 29-Dec-2014
-- Modification
-- [03-Feb-2015] - Updated to get sum of offline count and mcr count.[zohaib]
-- Description:	Gets the offline message count and device token for the group participants
-- =============================================
CREATE PROCEDURE [dbo].[spne_GetGroupParticipantsDataByRoomIDnSenderID] 
-- Add the parameters for the stored procedure here
	@groupID int,
	@domain varchar(30),
    @senderID nvarchar(64)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT neUserExtension.username, neUserExtension.deviceToken, 
		   (neOfflineUserMessageCount.messageCount + neOfflineUserMessageCount.mcrCount) AS messageCount,
		   neUserExtension.imTone,
		   neUserExtension.devicePlatform
	FROM neUserExtension 
	INNER JOIN neOfflineUserMessageCount
	ON neUserExtension.username = neOfflineUserMessageCount.username
	WHERE neUserExtension.username IN (SELECT SUBSTRING(jid, 0 , CHARINDEX('@', jid, 0)) AS username
					   FROM ofMucMember
					   WHERE roomID = @groupID AND jid <> (@senderID + '@' +@domain)
					   UNION ALL
					   SELECT SUBSTRING(jid, 0, CHARINDEX('@', jid, 0)) AS username
					   FROM ofMucAffiliation
					   WHERE roomID = @groupID AND jid <> (@senderID + '@' +@domain));
    
END
