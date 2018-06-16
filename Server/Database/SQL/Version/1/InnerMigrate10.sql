GO
SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: create table dbo.InterestKind-------------------------------------------------------------
GO
CREATE TABLE [dbo].[InterestKind] (
	[InterestKindID]	[tinyint]		 NOT NULL,
	[Title]				[varchar](255)	 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]

INSERT INTO 
[dbo].[InterestKind]
           (
		   [InterestKindID]
           ,[Title]
		   )
SELECT
0,
'Отсутствует'
UNION ALL
SELECT
1,
'Процент'


GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.InterestKind: add primary key pk_InterestKind-----------------------------------------
GO
ALTER TABLE [dbo].[InterestKind] ADD CONSTRAINT [pk_InterestKind] PRIMARY KEY CLUSTERED ([InterestKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: create table dbo.UserInterest-------------------------------------------------------------
GO
CREATE TABLE [dbo].[UserInterest] (
	[UserInterestID]			[uniqueidentifier]	 NOT NULL,
	[UserID]					[uniqueidentifier]	 NOT NULL,
	[EventDate]					[smalldatetime]		 NOT NULL,
	[WorkInterestKindID]		[tinyint]			 NOT NULL,
	[WorkValue]					[numeric](18, 8)	 NULL,
	[DeviceInterestKindID]		[tinyint]			 NOT NULL,
	[DeviceValue]				[numeric](18, 8)	 NULL,
	[Description]				[varchar](max)		 COLLATE Cyrillic_General_CI_AS NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: dbo.UserInterest: add default DS_UserInterest_UserInterestID------------------------------
GO
ALTER TABLE [dbo].[UserInterest] ADD CONSTRAINT [DS_UserInterest_UserInterestID] DEFAULT (newid())FOR [UserInterestID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
--step 6: dbo.UserInterest: add default DS_UserInterest_UserID--------------------------------------
GO
ALTER TABLE [dbo].[UserInterest] ADD CONSTRAINT [DS_UserInterest_UserID] DEFAULT (newid())FOR [UserID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 6 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 6 is finished with errors' SET NOEXEC ON END
GO
--step 7: dbo.UserInterest: add primary key pk_UserInterest-----------------------------------------
GO
ALTER TABLE [dbo].[UserInterest] ADD CONSTRAINT [pk_UserInterest] PRIMARY KEY CLUSTERED ([UserInterestID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 7 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 7 is finished with errors' SET NOEXEC ON END
GO
--step 8: dbo.UserInterest: add foreign key InterestKind_UserInterest_1-----------------------------
GO
ALTER TABLE [dbo].[UserInterest] ADD CONSTRAINT [InterestKind_UserInterest_1] FOREIGN KEY ([WorkInterestKindID]) REFERENCES [dbo].[InterestKind] ([InterestKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 8 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 8 is finished with errors' SET NOEXEC ON END
GO
--step 9: dbo.UserInterest: add foreign key InterestKind_UserInterest_2-----------------------------
GO
ALTER TABLE [dbo].[UserInterest] ADD CONSTRAINT [InterestKind_UserInterest_2] FOREIGN KEY ([DeviceInterestKindID]) REFERENCES [dbo].[InterestKind] ([InterestKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 9 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 9 is finished with errors' SET NOEXEC ON END
GO
--step 10: dbo.UserInterest: add foreign key User_UserInterest_1------------------------------------
GO
ALTER TABLE [dbo].[UserInterest] ADD CONSTRAINT [User_UserInterest_1] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 10 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 10 is finished with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
