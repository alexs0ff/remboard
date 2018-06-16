GO
SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: create table dbo.UserGridFilter-----------------------------------------------------------
GO
CREATE TABLE [dbo].[UserGridFilter] (
	[UserGridFilterID]		[uniqueidentifier]	 NOT NULL,
	[Title]					[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[GridName]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[FilterData]			[nvarchar](max)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[UserID]				[uniqueidentifier]	 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.UserGridFilter: add default DS_UserGridFilter_UserGridFilterID------------------------
GO
ALTER TABLE [dbo].[UserGridFilter] ADD CONSTRAINT [DS_UserGridFilter_UserGridFilterID] DEFAULT (newid())FOR [UserGridFilterID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: dbo.UserGridFilter: add primary key pk_UserGridFilter-------------------------------------
GO
ALTER TABLE [dbo].[UserGridFilter] ADD CONSTRAINT [pk_UserGridFilter] PRIMARY KEY CLUSTERED ([UserGridFilterID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: create table dbo.UserGridState------------------------------------------------------------
GO
CREATE TABLE [dbo].[UserGridState] (
	[UserGridStateID]		[uniqueidentifier]	 NOT NULL,
	[GridName]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[StateGrid]				[nvarchar](max)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[UserID]				[uniqueidentifier]	 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
--step 6: dbo.UserGridState: add primary key pk_UserGridState---------------------------------------
GO
ALTER TABLE [dbo].[UserGridState] ADD CONSTRAINT [pk_UserGridState] PRIMARY KEY NONCLUSTERED ([UserGridStateID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 6 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 6 is finished with errors' SET NOEXEC ON END
GO
--step 7: add index UserGridIndex to table dbo.UserGridState----------------------------------------
GO
CREATE UNIQUE CLUSTERED INDEX [UserGridIndex] ON [dbo].[UserGridState]([GridName], [UserID]) WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON ) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 7 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 7 is finished with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
