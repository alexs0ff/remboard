GO
SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: create table dbo.RecoveryLogin------------------------------------------------------------
GO
CREATE TABLE [dbo].[RecoveryLogin] (
	[RecoveryLoginID]				[uniqueidentifier]	 NOT NULL,
	[LoginName]						[varchar](50)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[UTCEventDateTime]				[datetime]			 NOT NULL,
	[UTCEventDate]					[smalldatetime]		 NOT NULL,
	[RecoveryEmail]					[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[RecoveryClientIdentifier]		[varchar](50)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[IsRecovered]					[bit]				 NOT NULL,
	[RecoveredClientIdentifier]		[varchar](50)		 COLLATE Cyrillic_General_CI_AS NULL,
	[SentNumber]					[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[UTCRecoveredDateTime]			[datetime]			 NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.RecoveryLogin: add default DS_RecoveryLogin_RecoveryLoginID---------------------------
GO
ALTER TABLE [dbo].[RecoveryLogin] ADD CONSTRAINT [DS_RecoveryLogin_RecoveryLoginID] DEFAULT (newid())FOR [RecoveryLoginID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: dbo.RecoveryLogin: add primary key pk_RecoveryLogin---------------------------------------
GO
ALTER TABLE [dbo].[RecoveryLogin] ADD CONSTRAINT [pk_RecoveryLogin] PRIMARY KEY CLUSTERED ([RecoveryLoginID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: dbo.RecoveryLogin: add unique UQ__Recovery__9A1570622FC2625F------------------------------
GO
ALTER TABLE [dbo].[RecoveryLogin] ADD CONSTRAINT [UQ__Recovery__9A1570622FC2625F] UNIQUE NONCLUSTERED ([SentNumber])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
