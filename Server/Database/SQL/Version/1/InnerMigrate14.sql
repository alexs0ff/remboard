SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: create table dbo.UserSettings-------------------------------------------------------------
GO
CREATE TABLE [dbo].[UserSettings] (
	[UserSettingsID]	[uniqueidentifier]	 NOT NULL,
	[Number]			[nvarchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[UserLogin]			[nvarchar](50)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Data]				[nvarchar](max)		 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.UserSettings: add default DS_UserSettings_UserSettingsID------------------------------
GO
ALTER TABLE [dbo].[UserSettings] ADD CONSTRAINT [DS_UserSettings_UserSettingsID] DEFAULT (newid())FOR [UserSettingsID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: add index LoginNumberSettingsIndex to table dbo.UserSettings------------------------------
GO
CREATE UNIQUE CLUSTERED INDEX [LoginNumberSettingsIndex] ON [dbo].[UserSettings]([Number], [UserLogin]) WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON ) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: dbo.UserSettings: add primary key pk_UserSettings-----------------------------------------
GO
ALTER TABLE [dbo].[UserSettings] ADD CONSTRAINT [pk_UserSettings] PRIMARY KEY NONCLUSTERED ([UserSettingsID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
--step 6: create table dbo.UserDomainSettings-------------------------------------------------------
GO
CREATE TABLE [dbo].[UserDomainSettings] (
	[UserDomainSettingsID]		[uniqueidentifier]	 NOT NULL,
	[UserDomainID]				[uniqueidentifier]	 NOT NULL,
	[Number]					[nvarchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Data]						[nvarchar](max)		 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 6 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 6 is finished with errors' SET NOEXEC ON END
GO
--step 7: dbo.UserDomainSettings: add default DS_UserDomainSettings_UserDomainSettingsID------------
GO
ALTER TABLE [dbo].[UserDomainSettings] ADD CONSTRAINT [DS_UserDomainSettings_UserDomainSettingsID] DEFAULT (newid())FOR [UserDomainSettingsID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 7 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 7 is finished with errors' SET NOEXEC ON END
GO
--step 8: dbo.UserDomainSettings: add primary key pk_UserDomainSettings-----------------------------
GO
ALTER TABLE [dbo].[UserDomainSettings] ADD CONSTRAINT [pk_UserDomainSettings] PRIMARY KEY NONCLUSTERED ([UserDomainSettingsID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 8 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 8 is finished with errors' SET NOEXEC ON END
GO
--step 9: add index UserDomainIDNumberIndex to table dbo.UserDomainSettings-------------------------
GO
CREATE UNIQUE CLUSTERED INDEX [UserDomainIDNumberIndex] ON [dbo].[UserDomainSettings]([UserDomainID], [Number]) WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON ) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 9 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 9 is finished with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
