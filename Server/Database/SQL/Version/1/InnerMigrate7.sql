GO
SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: create table dbo.UserPublicKeyRequest-----------------------------------------------------
GO
CREATE TABLE [dbo].[UserPublicKeyRequest] (
	[UserPublicKeyRequestID]	[uniqueidentifier]	 NOT NULL,
	[UserID]					[uniqueidentifier]	 NOT NULL,
	[EventDate]					[smalldatetime]		 NOT NULL,
	[PublicKeyData]				[varchar](2000)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Number]					[varchar](50)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[KeyNotes]					[varchar](1000)		 COLLATE Cyrillic_General_CI_AS NULL,
	[ClientIdentifier]			[varchar](50)		 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.UserPublicKeyRequest: add default DS_UserPublicKeyRequest_UserPublicKeyRequestID------
GO
ALTER TABLE [dbo].[UserPublicKeyRequest] ADD CONSTRAINT [DS_UserPublicKeyRequest_UserPublicKeyRequestID] DEFAULT (newid())FOR [UserPublicKeyRequestID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: dbo.UserPublicKeyRequest: add default DS_UserPublicKeyRequest_UserID----------------------
GO
ALTER TABLE [dbo].[UserPublicKeyRequest] ADD CONSTRAINT [DS_UserPublicKeyRequest_UserID] DEFAULT (newid())FOR [UserID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: dbo.UserPublicKeyRequest: add primary key pk_UserPublicKeyRequest-------------------------
GO
ALTER TABLE [dbo].[UserPublicKeyRequest] ADD CONSTRAINT [pk_UserPublicKeyRequest] PRIMARY KEY CLUSTERED ([UserPublicKeyRequestID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
--step 6: dbo.UserPublicKeyRequest: add unique UQ__UserPubl__1C49E2423B3F19F4-----------------------
GO
ALTER TABLE [dbo].[UserPublicKeyRequest] ADD CONSTRAINT [UQ__UserPubl__1C49E2423B3F19F4] UNIQUE NONCLUSTERED ([UserPublicKeyRequestID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 6 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 6 is finished with errors' SET NOEXEC ON END
GO
--step 7: dbo.UserPublicKeyRequest: add foreign key User_UserPublicKeyRequest_1---------------------
GO
ALTER TABLE [dbo].[UserPublicKeyRequest] ADD CONSTRAINT [User_UserPublicKeyRequest_1] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 7 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 7 is finished with errors' SET NOEXEC ON END
GO
--step 8: create table dbo.UserPublicKey------------------------------------------------------------
GO
CREATE TABLE [dbo].[UserPublicKey] (
	[UserPublicKeyID]		[uniqueidentifier]	 NOT NULL,
	[UserID]				[uniqueidentifier]	 NOT NULL,
	[Number]				[varchar](50)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[EventDate]				[smalldatetime]		 NOT NULL,
	[PublicKeyData]			[varchar](2000)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[ClientIdentifier]		[varchar](50)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[KeyNotes]				[varchar](1000)		 COLLATE Cyrillic_General_CI_AS NULL,
	[IsRevoked]				[bit]				 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 8 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 8 is finished with errors' SET NOEXEC ON END
GO
--step 9: dbo.UserPublicKey: add default DS_UserPublicKey_UserPublicKeyID---------------------------
GO
ALTER TABLE [dbo].[UserPublicKey] ADD CONSTRAINT [DS_UserPublicKey_UserPublicKeyID] DEFAULT (newid())FOR [UserPublicKeyID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 9 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 9 is finished with errors' SET NOEXEC ON END
GO
--step 10: dbo.UserPublicKey: add default DS_UserPublicKey_UserID-----------------------------------
GO
ALTER TABLE [dbo].[UserPublicKey] ADD CONSTRAINT [DS_UserPublicKey_UserID] DEFAULT (newid())FOR [UserID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 10 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 10 is finished with errors' SET NOEXEC ON END
GO
--step 11: dbo.UserPublicKey: add primary key pk_UserPublicKey--------------------------------------
GO
ALTER TABLE [dbo].[UserPublicKey] ADD CONSTRAINT [pk_UserPublicKey] PRIMARY KEY CLUSTERED ([UserPublicKeyID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 11 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 11 is finished with errors' SET NOEXEC ON END
GO
--step 12: dbo.UserPublicKey: add unique UQ__UserPubl__1788CCADF23320B6-----------------------------
GO
ALTER TABLE [dbo].[UserPublicKey] ADD CONSTRAINT [UQ__UserPubl__1788CCADF23320B6] UNIQUE NONCLUSTERED ([UserID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 12 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 12 is finished with errors' SET NOEXEC ON END
GO
--step 13: dbo.UserPublicKey: add foreign key User_UserPublicKey_1----------------------------------
GO
ALTER TABLE [dbo].[UserPublicKey] ADD CONSTRAINT [User_UserPublicKey_1] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 13 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 13 is finished with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
