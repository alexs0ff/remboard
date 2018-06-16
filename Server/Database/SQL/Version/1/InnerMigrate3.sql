GO
SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: dbo.DeviceItem: drop default DS_DeviceItem_DeviceItemID-----------------------------------
GO
ALTER TABLE [dbo].[DeviceItem] DROP CONSTRAINT [DS_DeviceItem_DeviceItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.DeviceItem: drop default DS_DeviceItem_RepairOrderID----------------------------------
GO
ALTER TABLE [dbo].[DeviceItem] DROP CONSTRAINT [DS_DeviceItem_RepairOrderID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: create temp table tmp_DeviceItem----------------------------------------------------------
GO
CREATE TABLE [dbo].[tmp_DeviceItem]
(
	[DeviceItemID]		[uniqueidentifier]	 NOT NULL CONSTRAINT [DS_DeviceItem_DeviceItemID] DEFAULT (newid()),
	[UserID]			[uniqueidentifier]	 NOT NULL CONSTRAINT [DS_DeviceItem_UserID] DEFAULT (newid()),
	[Title]				[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Count]				[numeric](18, 8)	 NOT NULL,
	[CostPrice]			[numeric](18, 8)	 NOT NULL,
	[Price]				[numeric](18, 8)	 NOT NULL,
	[RepairOrderID]		[uniqueidentifier]	 NOT NULL CONSTRAINT [DS_DeviceItem_RepairOrderID] DEFAULT (newid()),
	[EventDate]			[smalldatetime]		 NOT NULL
)
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: copy existing data into new table tmp_DeviceItem------------------------------------------
GO
INSERT INTO [dbo].[tmp_DeviceItem]([DeviceItemID], [Title], [Count], [CostPrice], [Price], [RepairOrderID], [EventDate]) SELECT
	[DeviceItemID],
	[Title],
	[Count],
	[CostPrice],
	[Price],
	[RepairOrderID],
	GetDate()
FROM [dbo].[DeviceItem]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
--step 6: drop table dbo.DeviceItem-----------------------------------------------------------------
GO
DROP TABLE [dbo].[DeviceItem]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 6 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 6 is finished with errors' SET NOEXEC ON END
GO
--step 7: rename [tmp_DeviceItem] to dbo.DeviceItem-------------------------------------------------
GO
DECLARE @i int
EXEC @i = sp_rename N'[dbo].[tmp_DeviceItem]', N'DeviceItem'
IF @I <> 0 BEGIN PRINT 'step 7 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 7 is finished with errors' SET NOEXEC ON END
GO
--step 8: user script-------------------------------------------------------------------------------
GO
UPDATE di 
	SET 
		di.[EventDate] = DATEADD(dd, DATEDIFF(dd, 0, o.EventDate), 0),
		di.UserID = isnull(o.EngineerID,(SELECT TOP 1 u.UserID FROM [User] u WHERE u.ProjectRoleID = 1 AND u.UserDomainID = o.UserDomainID))	
FROM
	[DeviceItem] di
INNER JOIN
	[RepairOrder] o
ON
	di.RepairOrderID = o.RepairOrderID

GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 8 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 8 is finished with errors' SET NOEXEC ON END
GO
--step 9: dbo.DeviceItem: add primary key pk_DeviceItem---------------------------------------------
GO
ALTER TABLE [dbo].[DeviceItem] ADD CONSTRAINT [pk_DeviceItem] PRIMARY KEY CLUSTERED ([DeviceItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 9 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 9 is finished with errors' SET NOEXEC ON END
GO
--step 10: dbo.DeviceItem: add foreign key User_DeviceItem_1----------------------------------------
GO
ALTER TABLE [dbo].[DeviceItem] ADD CONSTRAINT [User_DeviceItem_1] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 10 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 10 is finished with errors' SET NOEXEC ON END
GO
--step 11: dbo.DeviceItem: add foreign key RepairOrder_DeviceItem_1---------------------------------
GO
ALTER TABLE [dbo].[DeviceItem] ADD CONSTRAINT [RepairOrder_DeviceItem_1] FOREIGN KEY ([RepairOrderID]) REFERENCES [dbo].[RepairOrder] ([RepairOrderID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 11 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 11 is finished with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
