USE [NeeoDashboard]
GO
/****** Object:  StoredProcedure [dbo].[spne_GetUserName]    Script Date: 1/18/2015 10:08:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Usman Saeed
-- Create date: 01/012/2015
-- Description:	It returns Username against a specific token.
-- =============================================
CREATE PROCEDURE [dbo].[spne_GetUserName]
@authKey varchar(30),
@username varchar(30) OUTPUT
AS
BEGIN
	SET @username=(SELECT U.userName 
				   FROM neUser U,neSessionDetails S 
				   WHERE U.uID = S.uID AND S.authKey = @authKey);
END

GO
/****** Object:  StoredProcedure [dbo].[spne_UpdateLastActivity]    Script Date: 1/18/2015 10:08:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Usman Saeed
-- Create date: 01/012/2015
-- Description:	It updates last activity time of current user if his session time not expired else deactivate user.
-- =============================================

CREATE PROCEDURE [dbo].[spne_UpdateLastActivity]
@authKey varchar(30)
AS
BEGIN

     IF(DATEDIFF(minute,(SELECT lastActivityTime FROM neSessionDetails WHERE authKey= @authKey),GETDATE() ) < 20
	  AND ( (SELECT isActive FROM neSessionDetails WHERE authKey=@authKey)=1 ))
     BEGIN
            UPDATE neSessionDetails 
	        SET lastActivityTime= GETDATE()
		    WHERE (authKey = @authKey);
			RETURN 1;
     END

     ELSE
     BEGIN
            UPDATE neSessionDetails 
	        SET isActive = 0 
		    WHERE (authKey = @authKey);
			RETURN 0;
	 END
	
END

GO
/****** Object:  StoredProcedure [dbo].[spne_UserLogin]    Script Date: 1/18/2015 10:08:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Usman Saeed
-- Create date: 01/012/2015
-- Description:	It ensures the login details and returns AuthKey
-- =============================================
CREATE PROCEDURE [dbo].[spne_UserLogin] 
	-- Add the parameters for the stored procedure here
	@username varchar(30),
	@password varchar(200),
	@authKey varchar(100) OUTPUT

AS
BEGIN
	DECLARE @UserID INT;
	SET  @UserID = (SELECT TB.uID 
					FROM neUser TB 
					WHERE TB.userName = @username AND TB.password = @password AND TB.isActive = 1);

	IF  (@UserID IS NOT NULL)
	BEGIN
		EXEC [dbo].spne_GetAuthKey @len=30, @min=48, @range=74, @exclude=':;<=>?@[]`^\/)', @output = @authKey OUT;
		INSERT INTO neSessionDetails([uID],[loginTime],[lastActivityTime],[isActive],[authKey])
		VALUES(@UserID, GETDATE(), GETDATE(), 1, @authKey);
	END
END


GO
/****** Object:  StoredProcedure [dbo].[spne_UserSignOut]    Script Date: 1/18/2015 10:08:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Usman Saeed
-- Create date: 01/012/2015
-- Description:	It is use to signout the current active user.
-- =============================================

CREATE PROCEDURE [dbo].[spne_UserSignOut]
@authKey varchar(30)
AS
BEGIN
	 UPDATE neSessionDetails 
	 SET isActive= 0 ,lastActivityTime = GETDATE()
	 WHERE (authKey = @authKey )
	 RETURN 1;
END

GO
/****** Object:  StoredProcedure [dbo].[spne_GetAuthKey]    Script Date: 1/18/2015 10:08:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Usman Saeed
-- Create date: 01/012/2015
-- Description:	It ensures the login details and returns AuthKey
-- =============================================

CREATE PROCEDURE [dbo].[spne_GetAuthKey]
    @len int,
    @min tinyint = 48,
    @range tinyint = 74,
    @exclude varchar(50) = '0:;<=>?@O[]`^\/',
    @output varchar(50) OUTPUT
AS 
BEGIN
    DECLARE @char char
    SET @output = '';
 
    WHILE @len > 0 
	BEGIN
       SELECT @char = char(ROUND(RAND() * @range + @min, 0))
       IF (CHARINDEX(@char, @exclude) = 0)
	   BEGIN
           SET @output += @char;
           SET @len = @len - 1;
       END
    END
END



