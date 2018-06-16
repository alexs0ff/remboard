GO
SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: create table dbo.FinancialItem------------------------------------------------------------
GO
CREATE TABLE [dbo].[FinancialItem] (
	[FinancialItemID]			[uniqueidentifier]	 NOT NULL,
	[Title]						[varchar](2000)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Description]				[varchar](max)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[UserDomainID]				[uniqueidentifier]	 NOT NULL,
	[FinancialItemKindID]		[int]				 NOT NULL,
	[TransactionKindID]			[tinyint]			 NOT NULL,
	[EventDate]					[smalldatetime]		 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.FinancialItem: add default DS_FinancialItem_FinancialItemID---------------------------
GO
ALTER TABLE [dbo].[FinancialItem] ADD CONSTRAINT [DS_FinancialItem_FinancialItemID] DEFAULT (newid())FOR [FinancialItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: dbo.FinancialItem: add default DS_FinancialItem_UserDomainID------------------------------
GO
ALTER TABLE [dbo].[FinancialItem] ADD CONSTRAINT [DS_FinancialItem_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: dbo.FinancialItem: add primary key pk_FinancialItem---------------------------------------
GO
ALTER TABLE [dbo].[FinancialItem] ADD CONSTRAINT [pk_FinancialItem] PRIMARY KEY CLUSTERED ([FinancialItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
--step 6: create table dbo.FinancialItemKind--------------------------------------------------------
GO
CREATE TABLE [dbo].[FinancialItemKind] (
	[FinancialItemKindID]		[int]			 NOT NULL,
	[Title]						[varchar](255)	 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 6 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 6 is finished with errors' SET NOEXEC ON END
GO
--step 7: dbo.FinancialItemKind: add primary key pk_FinancialItemKind-------------------------------
GO
ALTER TABLE [dbo].[FinancialItemKind] ADD CONSTRAINT [pk_FinancialItemKind] PRIMARY KEY CLUSTERED ([FinancialItemKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 7 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 7 is finished with errors' SET NOEXEC ON END
GO
--step 8: dbo.FinancialItem: add foreign key FinancialItemKind_FinancialItem_1----------------------
GO
ALTER TABLE [dbo].[FinancialItem] ADD CONSTRAINT [FinancialItemKind_FinancialItem_1] FOREIGN KEY ([FinancialItemKindID]) REFERENCES [dbo].[FinancialItemKind] ([FinancialItemKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 8 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 8 is finished with errors' SET NOEXEC ON END
GO
--step 9: create table dbo.TransactionKind----------------------------------------------------------
GO
CREATE TABLE [dbo].[TransactionKind] (
	[TransactionKindID]		[tinyint]		 NOT NULL,
	[Title]					[varchar](255)	 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 9 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 9 is finished with errors' SET NOEXEC ON END
GO
--step 10: dbo.TransactionKind: add primary key pk_TransactionKind----------------------------------
GO
ALTER TABLE [dbo].[TransactionKind] ADD CONSTRAINT [pk_TransactionKind] PRIMARY KEY CLUSTERED ([TransactionKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 10 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 10 is finished with errors' SET NOEXEC ON END
GO
--step 11: dbo.FinancialItem: add foreign key TransactionKind_FinancialItem_1-----------------------
GO
ALTER TABLE [dbo].[FinancialItem] ADD CONSTRAINT [TransactionKind_FinancialItem_1] FOREIGN KEY ([TransactionKindID]) REFERENCES [dbo].[TransactionKind] ([TransactionKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 11 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 11 is finished with errors' SET NOEXEC ON END
GO
--step 12: dbo.FinancialItem: add foreign key UserDomain_FinancialItem_1----------------------------
GO
ALTER TABLE [dbo].[FinancialItem] ADD CONSTRAINT [UserDomain_FinancialItem_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 12 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 12 is finished with errors' SET NOEXEC ON END
GO
--step 13: create table dbo.FinancialGroupBranchMap-------------------------------------------------
GO
CREATE TABLE [dbo].[FinancialGroupBranchMap] (
	[FinancialGroupBranchMapID]		[uniqueidentifier]	 NOT NULL,
	[BranchID]						[uniqueidentifier]	 NOT NULL,
	[FinancialGroupID]				[uniqueidentifier]	 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 13 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 13 is finished with errors' SET NOEXEC ON END
GO
--step 14: dbo.FinancialGroupBranchMap: add default DS_FinancialGroupBranchMap_FinancialGroupBranchMapID
GO
ALTER TABLE [dbo].[FinancialGroupBranchMap] ADD CONSTRAINT [DS_FinancialGroupBranchMap_FinancialGroupBranchMapID] DEFAULT (newid())FOR [FinancialGroupBranchMapID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 14 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 14 is finished with errors' SET NOEXEC ON END
GO
--step 15: dbo.FinancialGroupBranchMap: add default DS_FinancialGroupBranchMap_BranchID-------------
GO
ALTER TABLE [dbo].[FinancialGroupBranchMap] ADD CONSTRAINT [DS_FinancialGroupBranchMap_BranchID] DEFAULT (newid())FOR [BranchID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 15 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 15 is finished with errors' SET NOEXEC ON END
GO
--step 16: dbo.FinancialGroupBranchMap: add default DS_FinancialGroupBranchMap_FinancialGroupID-----
GO
ALTER TABLE [dbo].[FinancialGroupBranchMap] ADD CONSTRAINT [DS_FinancialGroupBranchMap_FinancialGroupID] DEFAULT (newid())FOR [FinancialGroupID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 16 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 16 is finished with errors' SET NOEXEC ON END
GO
--step 17: add index FinancialGroupBranchMapIndex to table dbo.FinancialGroupBranchMap--------------
GO
CREATE UNIQUE NONCLUSTERED INDEX [FinancialGroupBranchMapIndex] ON [dbo].[FinancialGroupBranchMap]([BranchID], [FinancialGroupID]) WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON ) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 17 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 17 is finished with errors' SET NOEXEC ON END
GO
--step 18: dbo.FinancialGroupBranchMap: add primary key pk_FinancialGroupBranchMap------------------
GO
ALTER TABLE [dbo].[FinancialGroupBranchMap] ADD CONSTRAINT [pk_FinancialGroupBranchMap] PRIMARY KEY CLUSTERED ([FinancialGroupBranchMapID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 18 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 18 is finished with errors' SET NOEXEC ON END
GO
--step 19: dbo.FinancialGroupBranchMap: add foreign key Branch_FinancialGroupBranchMap_1------------
GO
ALTER TABLE [dbo].[FinancialGroupBranchMap] ADD CONSTRAINT [Branch_FinancialGroupBranchMap_1] FOREIGN KEY ([BranchID]) REFERENCES [dbo].[Branch] ([BranchID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 19 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 19 is finished with errors' SET NOEXEC ON END
GO
--step 20: create table dbo.FinancialGroup----------------------------------------------------------
GO
CREATE TABLE [dbo].[FinancialGroup] (
	[FinancialGroupID]		[uniqueidentifier]	 NOT NULL,
	[UserDomainID]			[uniqueidentifier]	 NOT NULL,
	[Title]					[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[LegalName]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Trademark]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 20 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 20 is finished with errors' SET NOEXEC ON END
GO
--step 21: dbo.FinancialGroup: add default DS_FinancialGroup_FinancialGroupID-----------------------
GO
ALTER TABLE [dbo].[FinancialGroup] ADD CONSTRAINT [DS_FinancialGroup_FinancialGroupID] DEFAULT (newid())FOR [FinancialGroupID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 21 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 21 is finished with errors' SET NOEXEC ON END
GO
--step 22: dbo.FinancialGroup: add default DS_FinancialGroup_UserDomainID---------------------------
GO
ALTER TABLE [dbo].[FinancialGroup] ADD CONSTRAINT [DS_FinancialGroup_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 22 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 22 is finished with errors' SET NOEXEC ON END
GO
--step 23: dbo.FinancialGroup: add primary key pk_FinancialGroup------------------------------------
GO
ALTER TABLE [dbo].[FinancialGroup] ADD CONSTRAINT [pk_FinancialGroup] PRIMARY KEY CLUSTERED ([FinancialGroupID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 23 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 23 is finished with errors' SET NOEXEC ON END
GO
--step 24: dbo.FinancialGroup: add foreign key UserDomain_FinancialGroup_1--------------------------
GO
ALTER TABLE [dbo].[FinancialGroup] ADD CONSTRAINT [UserDomain_FinancialGroup_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 24 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 24 is finished with errors' SET NOEXEC ON END
GO
--step 25: dbo.FinancialGroupBranchMap: add foreign key FinancialGroup_FinancialGroupBranchMap_1----
GO
ALTER TABLE [dbo].[FinancialGroupBranchMap] ADD CONSTRAINT [FinancialGroup_FinancialGroupBranchMap_1] FOREIGN KEY ([FinancialGroupID]) REFERENCES [dbo].[FinancialGroup] ([FinancialGroupID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 25 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 25 is finished with errors' SET NOEXEC ON END
GO
--step 26: create table dbo.FinancialItemValue------------------------------------------------------
GO
CREATE TABLE [dbo].[FinancialItemValue] (
	[FinancialItemValueID]		[uniqueidentifier]	 NOT NULL,
	[FinancialGroupID]			[uniqueidentifier]	 NOT NULL,
	[FinancialItemID]			[uniqueidentifier]	 NOT NULL,
	[EventDate]					[smalldatetime]		 NOT NULL,
	[Amount]					[numeric](18, 8)	 NOT NULL,
	[CostAmount]				[numeric](18, 8)	 NULL,
	[Description]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 26 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 26 is finished with errors' SET NOEXEC ON END
GO
--step 27: dbo.FinancialItemValue: add default DS_FinancialItemValue_FinancialItemValueID-----------
GO
ALTER TABLE [dbo].[FinancialItemValue] ADD CONSTRAINT [DS_FinancialItemValue_FinancialItemValueID] DEFAULT (newid())FOR [FinancialItemValueID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 27 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 27 is finished with errors' SET NOEXEC ON END
GO
--step 28: dbo.FinancialItemValue: add default DS_FinancialItemValue_FinancialGroupID---------------
GO
ALTER TABLE [dbo].[FinancialItemValue] ADD CONSTRAINT [DS_FinancialItemValue_FinancialGroupID] DEFAULT (newid())FOR [FinancialGroupID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 28 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 28 is finished with errors' SET NOEXEC ON END
GO
--step 29: dbo.FinancialItemValue: add default DS_FinancialItemValue_FinancialItemID----------------
GO
ALTER TABLE [dbo].[FinancialItemValue] ADD CONSTRAINT [DS_FinancialItemValue_FinancialItemID] DEFAULT (newid())FOR [FinancialItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 29 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 29 is finished with errors' SET NOEXEC ON END
GO
--step 30: dbo.FinancialItemValue: add primary key pk_FinancialItemValue----------------------------
GO
ALTER TABLE [dbo].[FinancialItemValue] ADD CONSTRAINT [pk_FinancialItemValue] PRIMARY KEY CLUSTERED ([FinancialItemValueID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 30 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 30 is finished with errors' SET NOEXEC ON END
GO
--step 31: dbo.FinancialItemValue: add foreign key FinancialGroup_FinancialItemValue_1--------------
GO
ALTER TABLE [dbo].[FinancialItemValue] ADD CONSTRAINT [FinancialGroup_FinancialItemValue_1] FOREIGN KEY ([FinancialGroupID]) REFERENCES [dbo].[FinancialGroup] ([FinancialGroupID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 31 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 31 is finished with errors' SET NOEXEC ON END
GO
--step 32: dbo.FinancialItemValue: add foreign key FinancialItem_FinancialItemValue_1---------------
GO
ALTER TABLE [dbo].[FinancialItemValue] ADD CONSTRAINT [FinancialItem_FinancialItemValue_1] FOREIGN KEY ([FinancialItemID]) REFERENCES [dbo].[FinancialItem] ([FinancialItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 32 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 32 is finished with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO

GO
--Тип статьи бюджета
INSERT INTO [dbo].[FinancialItemKind]
           (
           [FinancialItemKindID],
           [Title]
           )
SELECT
	1,
	'Пользовательский'
UNION ALL
SELECT
	2,
	'Статья доходов от оплаты заказов'
go

INSERT INTO [dbo].[TransactionKind]
           (
           [TransactionKindID],
           [Title]
           )
SELECT
	1,
	'Доходы'
UNION ALL
SELECT
	2,
	'Расходы'	
           
GO

----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
