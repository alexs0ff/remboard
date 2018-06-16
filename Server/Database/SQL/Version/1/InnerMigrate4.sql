SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: create table dbo.ItemCategory-------------------------------------------------------------
GO
CREATE TABLE [dbo].[ItemCategory] (
	[ItemCategoryID]	[uniqueidentifier]	 NOT NULL,
	[UserDomainID]		[uniqueidentifier]	 NOT NULL,
	[Title]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.ItemCategory: add default DS_ItemCategory_ItemCategoryID------------------------------
GO
ALTER TABLE [dbo].[ItemCategory] ADD CONSTRAINT [DS_ItemCategory_ItemCategoryID] DEFAULT (newid())FOR [ItemCategoryID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: dbo.ItemCategory: add default DS_ItemCategory_UserDomainID--------------------------------
GO
ALTER TABLE [dbo].[ItemCategory] ADD CONSTRAINT [DS_ItemCategory_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: dbo.ItemCategory: add primary key pk_ItemCategory-----------------------------------------
GO
ALTER TABLE [dbo].[ItemCategory] ADD CONSTRAINT [pk_ItemCategory] PRIMARY KEY CLUSTERED ([ItemCategoryID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
--step 6: dbo.ItemCategory: add foreign key UserDomain_ItemCategory_1-------------------------------
GO
ALTER TABLE [dbo].[ItemCategory] ADD CONSTRAINT [UserDomain_ItemCategory_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 6 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 6 is finished with errors' SET NOEXEC ON END
GO
--step 7: create table dbo.GoodsItem----------------------------------------------------------------
GO
CREATE TABLE [dbo].[GoodsItem] (
	[GoodsItemID]			[uniqueidentifier]	 NOT NULL,
	[DimensionKindID]		[tinyint]			 NOT NULL,
	[UserDomainID]			[uniqueidentifier]	 NOT NULL,
	[ItemCategoryID]		[uniqueidentifier]	 NOT NULL,
	[Title]					[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Description]			[varchar](4000)		 COLLATE Cyrillic_General_CI_AS NULL,
	[UserCode]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NULL,
	[Particular]			[varchar](255)		 COLLATE Cyrillic_General_CI_AS NULL,
	[BarCode]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 7 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 7 is finished with errors' SET NOEXEC ON END
GO
--step 8: dbo.GoodsItem: add default DS_GoodsItem_GoodsItemID---------------------------------------
GO
ALTER TABLE [dbo].[GoodsItem] ADD CONSTRAINT [DS_GoodsItem_GoodsItemID] DEFAULT (newid())FOR [GoodsItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 8 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 8 is finished with errors' SET NOEXEC ON END
GO
--step 9: dbo.GoodsItem: add default DS_GoodsItem_UserDomainID--------------------------------------
GO
ALTER TABLE [dbo].[GoodsItem] ADD CONSTRAINT [DS_GoodsItem_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 9 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 9 is finished with errors' SET NOEXEC ON END
GO
--step 10: dbo.GoodsItem: add default DS_GoodsItem_ItemCategoryID-----------------------------------
GO
ALTER TABLE [dbo].[GoodsItem] ADD CONSTRAINT [DS_GoodsItem_ItemCategoryID] DEFAULT (newid())FOR [ItemCategoryID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 10 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 10 is finished with errors' SET NOEXEC ON END
GO
--step 11: dbo.GoodsItem: add primary key pk_GoodsItem----------------------------------------------
GO
ALTER TABLE [dbo].[GoodsItem] ADD CONSTRAINT [pk_GoodsItem] PRIMARY KEY CLUSTERED ([GoodsItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 11 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 11 is finished with errors' SET NOEXEC ON END
GO
--step 12: create table dbo.DimensionKind-----------------------------------------------------------
GO
CREATE TABLE [dbo].[DimensionKind] (
	[DimensionKindID]		[tinyint]		 NOT NULL,
	[Title]					[varchar](255)	 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[ShortTitle]			[varchar](50)	 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 12 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 12 is finished with errors' SET NOEXEC ON END
GO
--step 13: dbo.DimensionKind: add primary key pk_DimensionKind--------------------------------------
GO
ALTER TABLE [dbo].[DimensionKind] ADD CONSTRAINT [pk_DimensionKind] PRIMARY KEY CLUSTERED ([DimensionKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 13 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 13 is finished with errors' SET NOEXEC ON END
GO
--step 14: dbo.GoodsItem: add foreign key DimensionKind_GoodsItem_1---------------------------------
GO
ALTER TABLE [dbo].[GoodsItem] ADD CONSTRAINT [DimensionKind_GoodsItem_1] FOREIGN KEY ([DimensionKindID]) REFERENCES [dbo].[DimensionKind] ([DimensionKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 14 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 14 is finished with errors' SET NOEXEC ON END
GO
--step 15: dbo.GoodsItem: add foreign key ItemCategory_GoodsItem_1----------------------------------
GO
ALTER TABLE [dbo].[GoodsItem] ADD CONSTRAINT [ItemCategory_GoodsItem_1] FOREIGN KEY ([ItemCategoryID]) REFERENCES [dbo].[ItemCategory] ([ItemCategoryID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 15 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 15 is finished with errors' SET NOEXEC ON END
GO
--step 16: dbo.GoodsItem: add foreign key UserDomain_GoodsItem_1------------------------------------
GO
ALTER TABLE [dbo].[GoodsItem] ADD CONSTRAINT [UserDomain_GoodsItem_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 16 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 16 is finished with errors' SET NOEXEC ON END
GO
--step 17: create table dbo.WarehouseItem-----------------------------------------------------------
GO
CREATE TABLE [dbo].[WarehouseItem] (
	[WarehouseItemID]		[uniqueidentifier]	 NOT NULL,
	[WarehouseID]			[uniqueidentifier]	 NOT NULL,
	[GoodsItemID]			[uniqueidentifier]	 NOT NULL,
	[Total]					[numeric](18, 8)	 NOT NULL,
	[StartPrice]			[numeric](18, 8)	 NOT NULL,
	[RepairPrice]			[numeric](18, 8)	 NOT NULL,
	[SalePrice]				[numeric](18, 8)	 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 17 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 17 is finished with errors' SET NOEXEC ON END
GO
--step 18: dbo.WarehouseItem: add default DS_WarehouseItem_WarehouseItemID--------------------------
GO
ALTER TABLE [dbo].[WarehouseItem] ADD CONSTRAINT [DS_WarehouseItem_WarehouseItemID] DEFAULT (newid())FOR [WarehouseItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 18 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 18 is finished with errors' SET NOEXEC ON END
GO
--step 19: dbo.WarehouseItem: add default DS_WarehouseItem_WarehouseID------------------------------
GO
ALTER TABLE [dbo].[WarehouseItem] ADD CONSTRAINT [DS_WarehouseItem_WarehouseID] DEFAULT (newid())FOR [WarehouseID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 19 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 19 is finished with errors' SET NOEXEC ON END
GO
--step 20: dbo.WarehouseItem: add default DS_WarehouseItem_GoodsItemID------------------------------
GO
ALTER TABLE [dbo].[WarehouseItem] ADD CONSTRAINT [DS_WarehouseItem_GoodsItemID] DEFAULT (newid())FOR [GoodsItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 20 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 20 is finished with errors' SET NOEXEC ON END
GO
--step 21: dbo.WarehouseItem: add primary key pk_WarehouseItem--------------------------------------
GO
ALTER TABLE [dbo].[WarehouseItem] ADD CONSTRAINT [pk_WarehouseItem] PRIMARY KEY CLUSTERED ([WarehouseItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 21 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 21 is finished with errors' SET NOEXEC ON END
GO
--step 22: dbo.WarehouseItem: add foreign key GoodsItem_WarehouseItem_1-----------------------------
GO
ALTER TABLE [dbo].[WarehouseItem] ADD CONSTRAINT [GoodsItem_WarehouseItem_1] FOREIGN KEY ([GoodsItemID]) REFERENCES [dbo].[GoodsItem] ([GoodsItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 22 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 22 is finished with errors' SET NOEXEC ON END
GO
--step 23: create table dbo.Warehouse---------------------------------------------------------------
GO
CREATE TABLE [dbo].[Warehouse] (
	[WarehouseID]		[uniqueidentifier]	 NOT NULL,
	[Title]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[UserDomainID]		[uniqueidentifier]	 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 23 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 23 is finished with errors' SET NOEXEC ON END
GO
--step 24: dbo.Warehouse: add default DS_Warehouse_WarehouseID--------------------------------------
GO
ALTER TABLE [dbo].[Warehouse] ADD CONSTRAINT [DS_Warehouse_WarehouseID] DEFAULT (newid())FOR [WarehouseID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 24 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 24 is finished with errors' SET NOEXEC ON END
GO
--step 25: dbo.Warehouse: add default DS_Warehouse_UserDomainID-------------------------------------
GO
ALTER TABLE [dbo].[Warehouse] ADD CONSTRAINT [DS_Warehouse_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 25 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 25 is finished with errors' SET NOEXEC ON END
GO
--step 26: dbo.Warehouse: add primary key pk_Warehouse----------------------------------------------
GO
ALTER TABLE [dbo].[Warehouse] ADD CONSTRAINT [pk_Warehouse] PRIMARY KEY CLUSTERED ([WarehouseID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 26 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 26 is finished with errors' SET NOEXEC ON END
GO
--step 27: dbo.Warehouse: add foreign key UserDomain_Warehouse_1------------------------------------
GO
ALTER TABLE [dbo].[Warehouse] ADD CONSTRAINT [UserDomain_Warehouse_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 27 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 27 is finished with errors' SET NOEXEC ON END
GO
--step 28: dbo.WarehouseItem: add foreign key Warehouse_WarehouseItem_1-----------------------------
GO
ALTER TABLE [dbo].[WarehouseItem] ADD CONSTRAINT [Warehouse_WarehouseItem_1] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([WarehouseID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 28 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 28 is finished with errors' SET NOEXEC ON END
GO
--step 29: create table dbo.CancellationDocItem-----------------------------------------------------
GO
CREATE TABLE [dbo].[CancellationDocItem] (
	[CancellationDocItemID]		[uniqueidentifier]	 NOT NULL,
	[CancellationDocID]			[uniqueidentifier]	 NOT NULL,
	[GoodsItemID]				[uniqueidentifier]	 NOT NULL,
	[Total]						[numeric](18, 8)	 NOT NULL,
	[Description]				[varchar](4000)		 COLLATE Cyrillic_General_CI_AS NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 29 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 29 is finished with errors' SET NOEXEC ON END
GO
--step 30: dbo.CancellationDocItem: add default DS_CancellationDocItem_CancellationDocItemID--------
GO
ALTER TABLE [dbo].[CancellationDocItem] ADD CONSTRAINT [DS_CancellationDocItem_CancellationDocItemID] DEFAULT (newid())FOR [CancellationDocItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 30 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 30 is finished with errors' SET NOEXEC ON END
GO
--step 31: dbo.CancellationDocItem: add default DS_CancellationDocItem_CancellationDocID------------
GO
ALTER TABLE [dbo].[CancellationDocItem] ADD CONSTRAINT [DS_CancellationDocItem_CancellationDocID] DEFAULT (newid())FOR [CancellationDocID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 31 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 31 is finished with errors' SET NOEXEC ON END
GO
--step 32: dbo.CancellationDocItem: add default DS_CancellationDocItem_GoodsItemID------------------
GO
ALTER TABLE [dbo].[CancellationDocItem] ADD CONSTRAINT [DS_CancellationDocItem_GoodsItemID] DEFAULT (newid())FOR [GoodsItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 32 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 32 is finished with errors' SET NOEXEC ON END
GO
--step 33: dbo.CancellationDocItem: add primary key pk_CancellationDocItem--------------------------
GO
ALTER TABLE [dbo].[CancellationDocItem] ADD CONSTRAINT [pk_CancellationDocItem] PRIMARY KEY CLUSTERED ([CancellationDocItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 33 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 33 is finished with errors' SET NOEXEC ON END
GO
--step 34: create table dbo.CancellationDoc---------------------------------------------------------
GO
CREATE TABLE [dbo].[CancellationDoc] (
	[CancellationDocID]		[uniqueidentifier]	 NOT NULL,
	[WarehouseID]			[uniqueidentifier]	 NOT NULL,
	[UserDomainID]			[uniqueidentifier]	 NOT NULL,
	[CreatorID]				[uniqueidentifier]	 NOT NULL,
	[DocNumber]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[DocDate]				[smalldatetime]		 NOT NULL,
	[DocDescription]		[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 34 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 34 is finished with errors' SET NOEXEC ON END
GO
--step 35: dbo.CancellationDoc: add default DS_CancellationDoc_CancellationDocID--------------------
GO
ALTER TABLE [dbo].[CancellationDoc] ADD CONSTRAINT [DS_CancellationDoc_CancellationDocID] DEFAULT (newid())FOR [CancellationDocID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 35 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 35 is finished with errors' SET NOEXEC ON END
GO
--step 36: dbo.CancellationDoc: add default DS_CancellationDoc_WarehouseID--------------------------
GO
ALTER TABLE [dbo].[CancellationDoc] ADD CONSTRAINT [DS_CancellationDoc_WarehouseID] DEFAULT (newid())FOR [WarehouseID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 36 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 36 is finished with errors' SET NOEXEC ON END
GO
--step 37: dbo.CancellationDoc: add default DS_CancellationDoc_UserDomainID-------------------------
GO
ALTER TABLE [dbo].[CancellationDoc] ADD CONSTRAINT [DS_CancellationDoc_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 37 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 37 is finished with errors' SET NOEXEC ON END
GO
--step 38: dbo.CancellationDoc: add default DS_CancellationDoc_CreatorID----------------------------
GO
ALTER TABLE [dbo].[CancellationDoc] ADD CONSTRAINT [DS_CancellationDoc_CreatorID] DEFAULT (newid())FOR [CreatorID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 38 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 38 is finished with errors' SET NOEXEC ON END
GO
--step 39: dbo.CancellationDoc: add primary key pk_CancellationDoc----------------------------------
GO
ALTER TABLE [dbo].[CancellationDoc] ADD CONSTRAINT [pk_CancellationDoc] PRIMARY KEY CLUSTERED ([CancellationDocID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 39 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 39 is finished with errors' SET NOEXEC ON END
GO
--step 40: dbo.CancellationDoc: add foreign key User_CancellationDoc_1------------------------------
GO
ALTER TABLE [dbo].[CancellationDoc] ADD CONSTRAINT [User_CancellationDoc_1] FOREIGN KEY ([CreatorID]) REFERENCES [dbo].[User] ([UserID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 40 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 40 is finished with errors' SET NOEXEC ON END
GO
--step 41: dbo.CancellationDoc: add foreign key UserDomain_CancellationDoc_1------------------------
GO
ALTER TABLE [dbo].[CancellationDoc] ADD CONSTRAINT [UserDomain_CancellationDoc_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 41 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 41 is finished with errors' SET NOEXEC ON END
GO
--step 42: dbo.CancellationDoc: add foreign key Warehouse_CancellationDoc_1-------------------------
GO
ALTER TABLE [dbo].[CancellationDoc] ADD CONSTRAINT [Warehouse_CancellationDoc_1] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([WarehouseID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 42 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 42 is finished with errors' SET NOEXEC ON END
GO
--step 43: dbo.CancellationDocItem: add foreign key CancellationDoc_CancellationDocItem_1-----------
GO
ALTER TABLE [dbo].[CancellationDocItem] ADD CONSTRAINT [CancellationDoc_CancellationDocItem_1] FOREIGN KEY ([CancellationDocID]) REFERENCES [dbo].[CancellationDoc] ([CancellationDocID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 43 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 43 is finished with errors' SET NOEXEC ON END
GO
--step 44: dbo.CancellationDocItem: add foreign key GoodsItem_CancellationDocItem_1-----------------
GO
ALTER TABLE [dbo].[CancellationDocItem] ADD CONSTRAINT [GoodsItem_CancellationDocItem_1] FOREIGN KEY ([GoodsItemID]) REFERENCES [dbo].[GoodsItem] ([GoodsItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 44 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 44 is finished with errors' SET NOEXEC ON END
GO
--step 45: create table dbo.ProcessedWarehouseDoc---------------------------------------------------
GO
CREATE TABLE [dbo].[ProcessedWarehouseDoc] (
	[ProcessedWarehouseDocID]		[uniqueidentifier]	 NOT NULL,
	[WarehouseID]					[uniqueidentifier]	 NOT NULL,
	[EventDate]						[smalldatetime]		 NOT NULL,
	[UTCEventDateTime]				[datetime]			 NOT NULL,
	[UserID]						[uniqueidentifier]	 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 45 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 45 is finished with errors' SET NOEXEC ON END
GO
--step 46: dbo.ProcessedWarehouseDoc: add default DS_ProcessedWarehouseDoc_ProcessedWarehouseDocID--
GO
ALTER TABLE [dbo].[ProcessedWarehouseDoc] ADD CONSTRAINT [DS_ProcessedWarehouseDoc_ProcessedWarehouseDocID] DEFAULT (newid())FOR [ProcessedWarehouseDocID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 46 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 46 is finished with errors' SET NOEXEC ON END
GO
--step 47: dbo.ProcessedWarehouseDoc: add default DS_ProcessedWarehouseDoc_WarehouseID--------------
GO
ALTER TABLE [dbo].[ProcessedWarehouseDoc] ADD CONSTRAINT [DS_ProcessedWarehouseDoc_WarehouseID] DEFAULT (newid())FOR [WarehouseID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 47 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 47 is finished with errors' SET NOEXEC ON END
GO
--step 48: dbo.ProcessedWarehouseDoc: add default DS_ProcessedWarehouseDoc_UserID-------------------
GO
ALTER TABLE [dbo].[ProcessedWarehouseDoc] ADD CONSTRAINT [DS_ProcessedWarehouseDoc_UserID] DEFAULT (newid())FOR [UserID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 48 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 48 is finished with errors' SET NOEXEC ON END
GO
--step 49: dbo.ProcessedWarehouseDoc: add primary key pk_ProcessedWarehouseDoc----------------------
GO
ALTER TABLE [dbo].[ProcessedWarehouseDoc] ADD CONSTRAINT [pk_ProcessedWarehouseDoc] PRIMARY KEY CLUSTERED ([ProcessedWarehouseDocID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 49 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 49 is finished with errors' SET NOEXEC ON END
GO
--step 50: dbo.ProcessedWarehouseDoc: add foreign key Warehouse_ProcessedWarehouseDoc_1-------------
GO
ALTER TABLE [dbo].[ProcessedWarehouseDoc] ADD CONSTRAINT [Warehouse_ProcessedWarehouseDoc_1] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([WarehouseID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 50 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 50 is finished with errors' SET NOEXEC ON END
GO
--step 51: create table dbo.IncomingDoc-------------------------------------------------------------
GO
CREATE TABLE [dbo].[IncomingDoc] (
	[IncomingDocID]		[uniqueidentifier]	 NOT NULL,
	[CreatorID]			[uniqueidentifier]	 NOT NULL,
	[WarehouseID]		[uniqueidentifier]	 NOT NULL,
	[UserDomainID]		[uniqueidentifier]	 NOT NULL,
	[ContractorID]		[uniqueidentifier]	 NOT NULL,
	[DocDate]			[smalldatetime]		 NOT NULL,
	[DocNumber]			[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[DocDescription]	[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 51 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 51 is finished with errors' SET NOEXEC ON END
GO
--step 52: dbo.IncomingDoc: add default DS_IncomingDoc_IncomingDocID--------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [DS_IncomingDoc_IncomingDocID] DEFAULT (newid())FOR [IncomingDocID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 52 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 52 is finished with errors' SET NOEXEC ON END
GO
--step 53: dbo.IncomingDoc: add default DS_IncomingDoc_CreatorID------------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [DS_IncomingDoc_CreatorID] DEFAULT (newid())FOR [CreatorID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 53 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 53 is finished with errors' SET NOEXEC ON END
GO
--step 54: dbo.IncomingDoc: add default DS_IncomingDoc_WarehouseID----------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [DS_IncomingDoc_WarehouseID] DEFAULT (newid())FOR [WarehouseID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 54 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 54 is finished with errors' SET NOEXEC ON END
GO
--step 55: dbo.IncomingDoc: add default DS_IncomingDoc_UserDomainID---------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [DS_IncomingDoc_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 55 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 55 is finished with errors' SET NOEXEC ON END
GO
--step 56: dbo.IncomingDoc: add default DS_IncomingDoc_ContractorID---------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [DS_IncomingDoc_ContractorID] DEFAULT (newid())FOR [ContractorID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 56 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 56 is finished with errors' SET NOEXEC ON END
GO
--step 57: dbo.IncomingDoc: add primary key pk_IncomingDoc------------------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [pk_IncomingDoc] PRIMARY KEY CLUSTERED ([IncomingDocID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 57 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 57 is finished with errors' SET NOEXEC ON END
GO
--step 58: create table dbo.Contractor--------------------------------------------------------------
GO
CREATE TABLE [dbo].[Contractor] (
	[ContractorID]		[uniqueidentifier]	 NOT NULL,
	[LegalName]			[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Trademark]			[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Address]			[varchar](2000)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Phone]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NULL,
	[EventDate]			[smalldatetime]		 NOT NULL,
	[UserDomainID]		[uniqueidentifier]	 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 58 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 58 is finished with errors' SET NOEXEC ON END
GO
--step 59: dbo.Contractor: add default DS_Contractor_ContractorID-----------------------------------
GO
ALTER TABLE [dbo].[Contractor] ADD CONSTRAINT [DS_Contractor_ContractorID] DEFAULT (newid())FOR [ContractorID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 59 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 59 is finished with errors' SET NOEXEC ON END
GO
--step 60: dbo.Contractor: add default DS_Contractor_UserDomainID-----------------------------------
GO
ALTER TABLE [dbo].[Contractor] ADD CONSTRAINT [DS_Contractor_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 60 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 60 is finished with errors' SET NOEXEC ON END
GO
--step 61: dbo.Contractor: add primary key pk_Contractor--------------------------------------------
GO
ALTER TABLE [dbo].[Contractor] ADD CONSTRAINT [pk_Contractor] PRIMARY KEY CLUSTERED ([ContractorID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 61 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 61 is finished with errors' SET NOEXEC ON END
GO
--step 62: dbo.Contractor: add foreign key UserDomain_Contractor_1----------------------------------
GO
ALTER TABLE [dbo].[Contractor] ADD CONSTRAINT [UserDomain_Contractor_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 62 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 62 is finished with errors' SET NOEXEC ON END
GO
--step 63: dbo.IncomingDoc: add foreign key Contractor_IncomingDoc_1--------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [Contractor_IncomingDoc_1] FOREIGN KEY ([ContractorID]) REFERENCES [dbo].[Contractor] ([ContractorID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 63 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 63 is finished with errors' SET NOEXEC ON END
GO
--step 64: dbo.IncomingDoc: add foreign key User_IncomingDoc_1--------------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [User_IncomingDoc_1] FOREIGN KEY ([CreatorID]) REFERENCES [dbo].[User] ([UserID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 64 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 64 is finished with errors' SET NOEXEC ON END
GO
--step 65: dbo.IncomingDoc: add foreign key UserDomain_IncomingDoc_1--------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [UserDomain_IncomingDoc_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 65 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 65 is finished with errors' SET NOEXEC ON END
GO
--step 66: dbo.IncomingDoc: add foreign key Warehouse_IncomingDoc_1---------------------------------
GO
ALTER TABLE [dbo].[IncomingDoc] ADD CONSTRAINT [Warehouse_IncomingDoc_1] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([WarehouseID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 66 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 66 is finished with errors' SET NOEXEC ON END
GO
--step 67: create table dbo.TransferDocItem---------------------------------------------------------
GO
CREATE TABLE [dbo].[TransferDocItem] (
	[TransferDocItemID]		[uniqueidentifier]	 NOT NULL,
	[TransferDocID]			[uniqueidentifier]	 NOT NULL,
	[GoodsItemID]			[uniqueidentifier]	 NOT NULL,
	[Total]					[numeric](18, 8)	 NOT NULL,
	[Description]			[varchar](4000)		 COLLATE Cyrillic_General_CI_AS NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 67 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 67 is finished with errors' SET NOEXEC ON END
GO
--step 68: dbo.TransferDocItem: add default DS_TransferDocItem_TransferDocItemID--------------------
GO
ALTER TABLE [dbo].[TransferDocItem] ADD CONSTRAINT [DS_TransferDocItem_TransferDocItemID] DEFAULT (newid())FOR [TransferDocItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 68 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 68 is finished with errors' SET NOEXEC ON END
GO
--step 69: dbo.TransferDocItem: add default DS_TransferDocItem_TransferDocID------------------------
GO
ALTER TABLE [dbo].[TransferDocItem] ADD CONSTRAINT [DS_TransferDocItem_TransferDocID] DEFAULT (newid())FOR [TransferDocID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 69 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 69 is finished with errors' SET NOEXEC ON END
GO
--step 70: dbo.TransferDocItem: add default DS_TransferDocItem_GoodsItemID--------------------------
GO
ALTER TABLE [dbo].[TransferDocItem] ADD CONSTRAINT [DS_TransferDocItem_GoodsItemID] DEFAULT (newid())FOR [GoodsItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 70 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 70 is finished with errors' SET NOEXEC ON END
GO
--step 71: dbo.TransferDocItem: add primary key pk_TransferDocItem----------------------------------
GO
ALTER TABLE [dbo].[TransferDocItem] ADD CONSTRAINT [pk_TransferDocItem] PRIMARY KEY CLUSTERED ([TransferDocItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 71 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 71 is finished with errors' SET NOEXEC ON END
GO
--step 72: dbo.TransferDocItem: add foreign key GoodsItem_TransferDocItem_1-------------------------
GO
ALTER TABLE [dbo].[TransferDocItem] ADD CONSTRAINT [GoodsItem_TransferDocItem_1] FOREIGN KEY ([GoodsItemID]) REFERENCES [dbo].[GoodsItem] ([GoodsItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 72 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 72 is finished with errors' SET NOEXEC ON END
GO
--step 73: create table dbo.TransferDoc-------------------------------------------------------------
GO
CREATE TABLE [dbo].[TransferDoc] (
	[TransferDocID]				[uniqueidentifier]	 NOT NULL,
	[SenderWarehouseID]			[uniqueidentifier]	 NOT NULL,
	[RecipientWarehouseID]		[uniqueidentifier]	 NOT NULL,
	[UserDomainID]				[uniqueidentifier]	 NOT NULL,
	[CreatorID]					[uniqueidentifier]	 NOT NULL,
	[DocNumber]					[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[DocDate]					[smalldatetime]		 NOT NULL,
	[DocDescription]			[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 73 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 73 is finished with errors' SET NOEXEC ON END
GO
--step 74: dbo.TransferDoc: add default DS_TransferDoc_TransferDocID--------------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [DS_TransferDoc_TransferDocID] DEFAULT (newid())FOR [TransferDocID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 74 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 74 is finished with errors' SET NOEXEC ON END
GO
--step 75: dbo.TransferDoc: add default DS_TransferDoc_SenderWarehouseID----------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [DS_TransferDoc_SenderWarehouseID] DEFAULT (newid())FOR [SenderWarehouseID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 75 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 75 is finished with errors' SET NOEXEC ON END
GO
--step 76: dbo.TransferDoc: add default DS_TransferDoc_RecipientWarehouseID-------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [DS_TransferDoc_RecipientWarehouseID] DEFAULT (newid())FOR [RecipientWarehouseID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 76 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 76 is finished with errors' SET NOEXEC ON END
GO
--step 77: dbo.TransferDoc: add default DS_TransferDoc_UserDomainID---------------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [DS_TransferDoc_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 77 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 77 is finished with errors' SET NOEXEC ON END
GO
--step 78: dbo.TransferDoc: add default DS_TransferDoc_CreatorID------------------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [DS_TransferDoc_CreatorID] DEFAULT (newid())FOR [CreatorID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 78 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 78 is finished with errors' SET NOEXEC ON END
GO
--step 79: dbo.TransferDoc: add primary key pk_TransferDoc------------------------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [pk_TransferDoc] PRIMARY KEY CLUSTERED ([TransferDocID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 79 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 79 is finished with errors' SET NOEXEC ON END
GO
--step 80: dbo.TransferDoc: add foreign key User_TransferDoc_1--------------------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [User_TransferDoc_1] FOREIGN KEY ([CreatorID]) REFERENCES [dbo].[User] ([UserID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 80 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 80 is finished with errors' SET NOEXEC ON END
GO
--step 81: dbo.TransferDoc: add foreign key UserDomain_TransferDoc_1--------------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [UserDomain_TransferDoc_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 81 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 81 is finished with errors' SET NOEXEC ON END
GO
--step 82: dbo.TransferDoc: add foreign key Warehouse_TransferDoc_1---------------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [Warehouse_TransferDoc_1] FOREIGN KEY ([SenderWarehouseID]) REFERENCES [dbo].[Warehouse] ([WarehouseID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 82 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 82 is finished with errors' SET NOEXEC ON END
GO
--step 83: dbo.TransferDoc: add foreign key Warehouse_TransferDoc_2---------------------------------
GO
ALTER TABLE [dbo].[TransferDoc] ADD CONSTRAINT [Warehouse_TransferDoc_2] FOREIGN KEY ([RecipientWarehouseID]) REFERENCES [dbo].[Warehouse] ([WarehouseID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 83 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 83 is finished with errors' SET NOEXEC ON END
GO
--step 84: dbo.TransferDocItem: add foreign key TransferDoc_TransferDocItem_1-----------------------
GO
ALTER TABLE [dbo].[TransferDocItem] ADD CONSTRAINT [TransferDoc_TransferDocItem_1] FOREIGN KEY ([TransferDocID]) REFERENCES [dbo].[TransferDoc] ([TransferDocID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 84 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 84 is finished with errors' SET NOEXEC ON END
GO
--step 85: create table dbo.IncomingDocItem---------------------------------------------------------
GO
CREATE TABLE [dbo].[IncomingDocItem] (
	[IncomingDocItemID]		[uniqueidentifier]	 NOT NULL,
	[IncomingDocID]			[uniqueidentifier]	 NOT NULL,
	[GoodsItemID]			[uniqueidentifier]	 NOT NULL,
	[Total]					[numeric](18, 8)	 NOT NULL,
	[InitPrice]				[numeric](18, 8)	 NOT NULL,
	[StartPrice]			[numeric](18, 8)	 NOT NULL,
	[RepairPrice]			[numeric](18, 8)	 NOT NULL,
	[SalePrice]				[numeric](18, 8)	 NOT NULL,
	[Description]			[varchar](4000)		 COLLATE Cyrillic_General_CI_AS NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 85 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 85 is finished with errors' SET NOEXEC ON END
GO
--step 86: dbo.IncomingDocItem: add default DS_IncomingDocItem_IncomingDocItemID--------------------
GO
ALTER TABLE [dbo].[IncomingDocItem] ADD CONSTRAINT [DS_IncomingDocItem_IncomingDocItemID] DEFAULT (newid())FOR [IncomingDocItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 86 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 86 is finished with errors' SET NOEXEC ON END
GO
--step 87: dbo.IncomingDocItem: add default DS_IncomingDocItem_IncomingDocID------------------------
GO
ALTER TABLE [dbo].[IncomingDocItem] ADD CONSTRAINT [DS_IncomingDocItem_IncomingDocID] DEFAULT (newid())FOR [IncomingDocID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 87 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 87 is finished with errors' SET NOEXEC ON END
GO
--step 88: dbo.IncomingDocItem: add default DS_IncomingDocItem_GoodsItemID--------------------------
GO
ALTER TABLE [dbo].[IncomingDocItem] ADD CONSTRAINT [DS_IncomingDocItem_GoodsItemID] DEFAULT (newid())FOR [GoodsItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 88 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 88 is finished with errors' SET NOEXEC ON END
GO
--step 89: dbo.IncomingDocItem: add primary key pk_IncomingDocItem----------------------------------
GO
ALTER TABLE [dbo].[IncomingDocItem] ADD CONSTRAINT [pk_IncomingDocItem] PRIMARY KEY CLUSTERED ([IncomingDocItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 89 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 89 is finished with errors' SET NOEXEC ON END
GO
--step 90: dbo.IncomingDocItem: add foreign key GoodsItem_IncomingDocItem_1-------------------------
GO
ALTER TABLE [dbo].[IncomingDocItem] ADD CONSTRAINT [GoodsItem_IncomingDocItem_1] FOREIGN KEY ([GoodsItemID]) REFERENCES [dbo].[GoodsItem] ([GoodsItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 90 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 90 is finished with errors' SET NOEXEC ON END
GO
--step 91: dbo.IncomingDocItem: add foreign key IncomingDoc_IncomingDocItem_1-----------------------
GO
ALTER TABLE [dbo].[IncomingDocItem] ADD CONSTRAINT [IncomingDoc_IncomingDocItem_1] FOREIGN KEY ([IncomingDocID]) REFERENCES [dbo].[IncomingDoc] ([IncomingDocID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 91 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 91 is finished with errors' SET NOEXEC ON END
GO
INSERT INTO [dbo].[DimensionKind]
           (
		   [DimensionKindID]
           ,[Title]
           ,[ShortTitle]
		   )
SELECT 1,'Øòóêè','øò'
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
