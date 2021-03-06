USE [XMPPDb]
GO
/****** Object:  Trigger [dbo].[trne_SendNotification]    Script Date: 1/21/2015 4:16:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Zohaib Hanif
-- Create date: 05/03/2014 (MM/dd/yyyy)
-- Last Modification date: 10/12/2014, 10/28/2014, 01/05/2015
-- [14-Jan-2015] - Room name in json body is changed.[zohaib]
-- [25-Mar-2015] - Notfication failure on double qoutes is fixed. [zohaib]
-- Description:	It sends push notification for offline messages by calling notification service.
-- =============================================
CREATE TRIGGER [dbo].[trne_SendNotification] 
   ON  [dbo].[ofOffline] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @receiverID nvarchar(64);
	DECLARE @senderID nvarchar(64); 
	DECLARE @senderJID nvarchar(500); 
	DECLARE @senderName nvarchar(100); 
	DECLARE @deviceToken varchar(64);
	DECLARE @devicePlatform char(1);
	DECLARE @offlineMsgCount varchar(10);
	DECLARE @msgType tinyint = 0;
	DECLARE @stanzaXML xml;
	DECLARE @notificationMsg nvarchar(max);
	DECLARE @imTone char(1);
	-- Group Chat
	DECLARE @roomSubject nvarchar(100) = '';
	DECLARE @inviter nvarchar(164);

	DECLARE @reqBody nvarchar(max);
	-- Constant variables
	DECLARE @Chat tinyint = 1;
	DECLARE @File tinyint = 2;
	DECLARE @NotificationType char(1);
	

    SET @stanzaXML = (SELECT ofOffline.stanza
						  FROM ofOffline
						  WHERE ofOffline.messageID = (SELECT inserted.messageID
													   FROM inserted));
	
	SET @senderJID = (@stanzaXML.value('(/message/@from)[1]','nvarchar(100)'));

	SET @senderID = [dbo].GetIdFromJid(@senderJID);

	IF(dbo.IsFromGroup(@senderJID) = 1)
	BEGIN
		SET @NotificationType = '5';

		SET @receiverID = (SELECT inserted.username
							   FROM inserted); 

		WITH XMLNAMESPACES ('http://jabber.org/protocol/muc#user' AS ns)
		SELECT @inviter = @stanzaXML.value('(/message/ns:x/ns:invite/@from)[1]','nvarchar(164)');

		SELECT @deviceToken = neUserExtension.deviceToken, @devicePlatform = neUserExtension.devicePlatform, @imTone = neUserExtension.imTone
		FROM neUserExtension
		WHERE neUserExtension.username = @receiverID;

		--**************************************************************************
			
		-- Get User offline messages count and update it.
		UPDATE neOfflineUserMessageCount
		SET messageCount = messageCount + 1
		WHERE neOfflineUserMessageCount.username = @receiverID;

		SELECT @offlineMsgCount = (messageCount + mcrCount)
		FROM neOfflineUserMessageCount
		WHERE neOfflineUserMessageCount.username = @receiverID;
			
		--**************************************************************************
		  
		IF ((@deviceToken <> '' AND @deviceToken <> '-1'))
		BEGIN
			SET @senderName = (SELECT name
							   FROM ofUser
							   WHERE username = [dbo].[GetIdFromJid](@inviter)); 
			SELECT @roomSubject = [subject]
			FROM ofMucRoom
			WHERE name = @senderID;

			SET @notificationMsg = @senderName + ' added you to the group \"' + @roomSubject + '\"';
			--SET @notificationMsg = @senderName + ' added you to the group \"Test\"';

			SET @reqBody = N'{'; 
			SET @reqBody += '"nType": ' + @NotificationType + ',';
			SET @reqBody += '"rName": "' + @senderID + '",';
			SET @reqBody += '"alert": "' + [dbo].[FormatString](@notificationMsg) + '",';
			SET @reqBody += '"receiverID": "' + @receiverID + '",';
			SET @reqBody += '"dToken": "' + @deviceToken + '",';
			SET @reqBody += '"dp": ' + @devicePlatform + ',';
			SET @reqBody += '"badge": ' + @offlineMsgCount + ',';
			SET @reqBody += '"imTone": ' + @imTone + '';
			SET @reqBody += '}';
			EXEC spne_SendNotification @reqBody;
		END
	END
	ELSE
	BEGIN
		SET @NotificationType = '1';

		WITH XMLNAMESPACES ('x' AS ns)
		SELECT @msgType = (@stanzaXML.value('(/message/@msgType)[1]','tinyint')),
			   @msgType = @stanzaXML.value('(/message/ns:type/@_type)[1]','tinyint'),
			   @notificationMsg = (LTRIM(RTRIM(@stanzaXML.value('(/message/body)[1]','nvarchar(max)'))));

	  --*****************************************************************************
		-- If message type does not find in the message, consider it as a IM message.
		IF COALESCE(@msgType, 0) = 0
		BEGIN
			SET @msgType = @Chat;
		END
	  --*****************************************************************************
		IF (@msgType = @Chat OR @msgType = @File)
		BEGIN

			SET @receiverID = (SELECT inserted.username
							   FROM inserted); 
		  --**************************************************************************
			
			UPDATE neOfflineUserMessageCount
			SET messageCount = messageCount + 1
			WHERE neOfflineUserMessageCount.username = @receiverID;

			SELECT @offlineMsgCount = (messageCount + mcrCount)
			FROM neOfflineUserMessageCount
			WHERE neOfflineUserMessageCount.username = @receiverID;
			
			--**************************************************************************
		   
			SELECT @deviceToken = neUserExtension.deviceToken, @devicePlatform = neUserExtension.devicePlatform, @imTone = neUserExtension.imTone
			FROM neUserExtension
			WHERE neUserExtension.username = @receiverID;
	    
			IF ((@deviceToken <> '' AND @deviceToken <> '-1'))
			BEGIN

				SET @senderName = (SELECT CASE WHEN (name = '' OR name IS NULL) THEN CONCAT('+',username) ELSE name END
								   FROM ofUser 
								   WHERE username = @senderID);
							    
				IF @msgType = @File
				BEGIN
					SET @notificationMsg = (@senderName + ' sent you a photo');
				END

				IF COALESCE(@notificationMsg,'') <> ''
				BEGIN
				  --*****************************************************************************
					--If current message type is IM, add some sender name as a prefix.
					IF @msgType = @Chat
						SET @notificationMsg = (@senderName + ' says : ' + @notificationMsg);
				  --*****************************************************************************
			   
					SET @reqBody = N'{'; 
					SET @reqBody += '"nType": ' + @NotificationType + ',';
					SET @reqBody += '"dp": ' + @devicePlatform + ',';
					SET @reqBody += '"dToken": "' + @deviceToken + '",';
					SET @reqBody += '"alert": "' + [dbo].[FormatString](@notificationMsg) + '",';
					SET @reqBody += '"receiverID": "' + @receiverID + '",';
					SET @reqBody += '"senderID": "' + @senderID + '",';
					SET @reqBody += '"badge": ' +  @offlineMsgCount + ',';
					SET @reqBody += '"imTone": ' + @imTone;
					SET @reqBody += '}';

					EXEC spne_SendNotification @reqBody;
				END
			END
		END
	END
END


