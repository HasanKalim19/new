SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Zohaib Hanif
-- Create date: 02/03/2014
-- Modification date: 04/08/2014 - dd/mm/yyyy
-- Description:	Determines whether contacts in the user contact list are application users or not and their respective subscription status
-- =============================================
CREATE PROCEDURE spne_GetContactsExistanceAndRosterStatusByContactsList 
	-- Add the parameters for the stored procedure here
	@userContacts varchar(max), 
	@applicationDomain varchar(30),
	@userID nvarchar(64),
	@delimeter varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Roster Info Enum
	DECLARE @NotExists varchar(1) = '0';
	DECLARE @Exists varchar(1) = '1';
	DECLARE @InvalidUser varchar(2) = '-1';
	-- Subscription Types
	DECLARE @SubFrom tinyint = 2;
	DECLARE @SubNone tinyint = 0;
	--Constant Values
	DECLARE @True bit = 1;
	DECLARE @False bit = 0;

	DECLARE @InvalidUserCode int = 50404;
	DECLARE @userContactTable TABLE (contact varchar(64));
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF EXISTS (SELECT 1 
			   FROM ofUser
			   WHERE username = @userID)
	BEGIN
	
		INSERT INTO @userContactTable
		SELECT * FROM [dbo].[SplitToTable](@userContacts,@delimeter);
		
		--Update  the LastSyncTimestamp in the neUserExtensionTable
		UPDATE neUserExtension
		SET lastSyncTimestamp = GETUTCDATE()
		WHERE username = @userID;
		
		SELECT [@userContactTable].contact,
		CASE WHEN (ofUser.username = [@userContactTable].contact) 
			THEN @True
			ELSE @False 
		END AS [isNeeoUser],
		CASE WHEN (myRoster.jid is not null and [@userContactTable].contact is not null )
			THEN @Exists 
			ELSE 
				CASE WHEN (myRoster.jid is null and ofUser.username is not null and [@userContactTable].contact is noT null )
					THEN @NotExists
					ELSE 
						CASE WHEN (myRoster.jid is not null and [@userContactTable].contact is null )
							THEN 
								SUBSTRING(myRoster.jid, 1, CHARINDEX('@',myRoster.jid) - 1)
							ELSE @InvalidUser
					END
				END  
		END AS [contactRosterInfo],
		myRoster.sub AS [contactSubState]
		FROM @userContactTable left join ofUser 
			ON [@userContactTable].contact = ofUser.username 
		FULL join (SELECT ofRoster.username,ofRoster.jid,ofRoster.sub 
				   FROM ofRoster
				   GROUP BY ofRoster.jid,ofRoster.username,ofRoster.sub
				   HAVING ofRoster.username = @userID and (ofRoster.sub != @SubNone and ofRoster.sub != @SubFrom)) AS myRoster
			ON ofUser.username + @applicationDomain = myRoster.jid 
		ORDER BY [@userContactTable].contact DESC;
	END
	ELSE
		RAISERROR(@InvalidUserCode,-1,-1); 
END
