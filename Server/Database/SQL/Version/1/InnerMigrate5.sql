GO
SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: add columns to Table dbo.DeviceItem-------------------------------------------------------
GO
ALTER TABLE [dbo].[DeviceItem] ADD 
	[WarehouseItemID]	[uniqueidentifier] NULL CONSTRAINT [DS_DeviceItem_WarehouseItemID] DEFAULT (newid())
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.DeviceItem: add foreign key WarehouseItem_DeviceItem_1--------------------------------
GO
ALTER TABLE [dbo].[DeviceItem] ADD CONSTRAINT [WarehouseItem_DeviceItem_1] FOREIGN KEY ([WarehouseItemID]) REFERENCES [dbo].[WarehouseItem] ([WarehouseItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: create table dbo.FinancialGroupWarehouseMap-----------------------------------------------
GO
CREATE TABLE [dbo].[FinancialGroupWarehouseMap] (
	[FinancialGroupWarehouseMapID]		[uniqueidentifier]	 NOT NULL,
	[WarehouseID]						[uniqueidentifier]	 NOT NULL,
	[FinancialGroupID]					[uniqueidentifier]	 NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: dbo.FinancialGroupWarehouseMap: add default DS_FinancialGroupWarehouseMap_FinancialGroupWarehouseMapID
GO
ALTER TABLE [dbo].[FinancialGroupWarehouseMap] ADD CONSTRAINT [DS_FinancialGroupWarehouseMap_FinancialGroupWarehouseMapID] DEFAULT (newid())FOR [FinancialGroupWarehouseMapID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
--step 6: dbo.FinancialGroupWarehouseMap: add default DS_FinancialGroupWarehouseMap_WarehouseID-----
GO
ALTER TABLE [dbo].[FinancialGroupWarehouseMap] ADD CONSTRAINT [DS_FinancialGroupWarehouseMap_WarehouseID] DEFAULT (newid())FOR [WarehouseID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 6 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 6 is finished with errors' SET NOEXEC ON END
GO
--step 7: dbo.FinancialGroupWarehouseMap: add default DS_FinancialGroupWarehouseMap_FinancialGroupID
GO
ALTER TABLE [dbo].[FinancialGroupWarehouseMap] ADD CONSTRAINT [DS_FinancialGroupWarehouseMap_FinancialGroupID] DEFAULT (newid())FOR [FinancialGroupID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 7 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 7 is finished with errors' SET NOEXEC ON END
GO
--step 8: add index FinancialGroupWarehouseMapIndex to table dbo.FinancialGroupWarehouseMap---------
GO
CREATE UNIQUE NONCLUSTERED INDEX [FinancialGroupWarehouseMapIndex] ON [dbo].[FinancialGroupWarehouseMap]([WarehouseID], [FinancialGroupID]) WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON ) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 8 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 8 is finished with errors' SET NOEXEC ON END
GO
--step 9: dbo.FinancialGroupWarehouseMap: add primary key pk_FinancialGroupWarehouseMap-------------
GO
ALTER TABLE [dbo].[FinancialGroupWarehouseMap] ADD CONSTRAINT [pk_FinancialGroupWarehouseMap] PRIMARY KEY CLUSTERED ([FinancialGroupWarehouseMapID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 9 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 9 is finished with errors' SET NOEXEC ON END
GO
--step 10: dbo.FinancialGroupWarehouseMap: add foreign key FinancialGroup_FinancialGroupWarehouseMap_1
GO
ALTER TABLE [dbo].[FinancialGroupWarehouseMap] ADD CONSTRAINT [FinancialGroup_FinancialGroupWarehouseMap_1] FOREIGN KEY ([FinancialGroupID]) REFERENCES [dbo].[FinancialGroup] ([FinancialGroupID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 10 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 10 is finished with errors' SET NOEXEC ON END
GO
--step 11: dbo.FinancialGroupWarehouseMap: add foreign key Warehouse_FinancialGroupWarehouseMap_1---
GO
ALTER TABLE [dbo].[FinancialGroupWarehouseMap] ADD CONSTRAINT [Warehouse_FinancialGroupWarehouseMap_1] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([WarehouseID])
GO
INSERT INTO [dbo].[FinancialItemKind]
           (
           [FinancialItemKindID],
           [Title]
           )
SELECT
	3,
	'Статья расходов от закупки комплектующих по накладным'

GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 11 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 11 is finished with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
