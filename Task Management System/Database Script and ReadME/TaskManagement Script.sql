/****** Object:  Table [dbo].[Table_TaskManagement]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table_TaskManagement](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TaskTitle] [nvarchar](500) NOT NULL,
	[TaskDescription] [nvarchar](500) NOT NULL,
	[TaskDueDate] [datetime] NOT NULL,
	[TaskStatus] [bit] NOT NULL,
	[UserID] [int] NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Table_TaskManagement] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Table_User]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table_User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Table_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Table_TaskManagement]  WITH CHECK ADD  CONSTRAINT [FK_Table_TaskManagement_Table_TaskManagement] FOREIGN KEY([ID])
REFERENCES [dbo].[Table_TaskManagement] ([ID])
GO
ALTER TABLE [dbo].[Table_TaskManagement] CHECK CONSTRAINT [FK_Table_TaskManagement_Table_TaskManagement]
GO
/****** Object:  StoredProcedure [dbo].[DeleteTask]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteTask]
(
	@ID INT = NULL,
	@UserID INT = NULL
)
AS
BEGIN
	UPDATE [Table_TaskManagement]
	SET Deleted = 1
	WHERE ID = @ID AND UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllTaskManagement]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetAllTaskManagement]
(
	@Status INT = NULL,
	@UserID INT = NULL
)
AS
DECLARE @NewStatus BIT = NULL
SET @NewStatus = CASE WHEN @Status = 0 THEN NULL 
					  WHEN @Status = 2 THEN 0
					  ELSE @Status END

SELECT [ID],[TaskTitle],[TaskDescription],[TaskDueDate],[TaskStatus],CASE WHEN [TaskStatus] = 0 THEN 2 ELSE 1 END AS TaskStatusType,[UserID],[Deleted]
FROM [dbo].[Table_TaskManagement]
WHERE (TaskStatus = @NewStatus OR @NewStatus IS NULL)
AND UserID = @UserID AND Deleted = 0


GO
/****** Object:  StoredProcedure [dbo].[GetTaskManagementByID]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetTaskManagementByID]
(
	@TaskID INT = NULL,
	@UserID INT = NULL
)
AS

SELECT [ID],[TaskTitle],[TaskDescription],[TaskDueDate],[TaskStatus],[UserID],[Deleted]
FROM [dbo].[Table_TaskManagement]
WHERE ID = @TaskID AND UserID = @UserID

GO
/****** Object:  StoredProcedure [dbo].[GetTaskManagementByUser]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetTaskManagementByUser]
(
  @UserID INT = NULL
)
AS
	SELECT * FROM [dbo].[Table_TaskManagement]
	WHERE UserID = @UserID
	AND Deleted = 0
GO
/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RegisterUser]
(
  @UserName NVARCHAR(50) = NULL,
  @Email NVARCHAR(50) = NULL,
  @Password NVARCHAR(50) = NULL
)
AS
	IF NOT EXISTS(SELECT Email FROM Table_User WHERE Email = @Email)
	BEGIN
		INSERT INTO Table_User
		(
		UserName,Email,[Password]
		)
		VALUES
		(@UserName,@Email,@Password)
	END
	ELSE 
	THROW 51000, 'User Already Exists. Please Login...', 1

GO
/****** Object:  StoredProcedure [dbo].[SaveTaskManagementByUser]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaveTaskManagementByUser]
(
  @TaskTitle NVARCHAR(500) = NULL,
  @TaskDescription NVARCHAR(500) = NULL,
  @TaskDueDate DATETIME = NULL,
  @TaskStatus BIT = NULL,
  @UserID INT = NULL,
  @Deleted BIT = NULL
)
AS
	IF NOT EXISTS(SELECT TaskTitle FROM [Table_TaskManagement] WHERE UserID = @UserID)
	BEGIN
		INSERT INTO [Table_TaskManagement]
		(
		TaskTitle,TaskDescription,TaskDueDate,TaskStatus,UserID,Deleted
		)
		VALUES
		(@TaskTitle,@TaskDescription,@TaskDueDate,@TaskStatus,@UserID,@Deleted)
	END
	ELSE 
	THROW 51000, 'Task Already Exists. Please Change Task...', 1

GO
/****** Object:  StoredProcedure [dbo].[UpsertTask]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpsertTask]
(
	@ID INT = NULL,
	@Title VARCHAR(500) = NULL,
	@Description VARCHAR(500)= NULL,
	@DueDate DATETIME = NULL,
	@Status BIT = NULL,
	@UserID INT = NULL
)
AS
BEGIN
	IF @ID > 0
	BEGIN
		UPDATE [Table_TaskManagement]
		SET
		 [TaskTitle] = @Title
		,[TaskDescription] = @Description
		,[TaskDueDate] = @DueDate
		,[TaskStatus] = @Status
		WHERE ID = @ID
		AND UserID = @UserID
	END
	ELSE
	BEGIN
		INSERT INTO [Table_TaskManagement]
		([TaskTitle],[TaskDescription],[TaskDueDate],[TaskStatus],[UserID],[Deleted])
		VALUES
		(@Title,@Description,@DueDate,@Status,@UserID,0)
	END

END
GO
/****** Object:  StoredProcedure [dbo].[ValidateUser]    Script Date: 13-03-2024 08:08:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ValidateUser]
(
  @Email NVARCHAR(50) = NULL,
  @Password NVARCHAR(50) = NULL,
  @ID INT output
)
AS
	SELECT  @ID=ID from Table_User
	WHERE Email = @Email AND [Password] = @Password
GO
