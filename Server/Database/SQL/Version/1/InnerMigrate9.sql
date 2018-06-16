SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 2: create table dbo.AutocompleteItem---------------------------------------------------------
GO
CREATE TABLE [dbo].[AutocompleteItem] (
	[AutocompleteItemID]	[uniqueidentifier]	 NOT NULL,
	[UserDomainID]			[uniqueidentifier]	 NOT NULL,
	[AutocompleteKindID]	[tinyint]			 NOT NULL,
	[Title]					[varchar](255)		 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is finished with errors' SET NOEXEC ON END
GO
--step 3: dbo.AutocompleteItem: add default DS_AutocompleteItem_AutocompleteItemID------------------
GO
ALTER TABLE [dbo].[AutocompleteItem] ADD CONSTRAINT [DS_AutocompleteItem_AutocompleteItemID] DEFAULT (newid())FOR [AutocompleteItemID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 3 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 3 is finished with errors' SET NOEXEC ON END
GO
--step 4: dbo.AutocompleteItem: add default DS_AutocompleteItem_UserDomainID------------------------
GO
ALTER TABLE [dbo].[AutocompleteItem] ADD CONSTRAINT [DS_AutocompleteItem_UserDomainID] DEFAULT (newid())FOR [UserDomainID]
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 4 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 4 is finished with errors' SET NOEXEC ON END
GO
--step 5: dbo.AutocompleteItem: add primary key pk_AutocompleteItem---------------------------------
GO
ALTER TABLE [dbo].[AutocompleteItem] ADD CONSTRAINT [pk_AutocompleteItem] PRIMARY KEY CLUSTERED ([AutocompleteItemID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 5 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 5 is finished with errors' SET NOEXEC ON END
GO
--step 6: create table dbo.AutocompleteKind---------------------------------------------------------
GO
CREATE TABLE [dbo].[AutocompleteKind] (
	[AutocompleteKindID]	[tinyint]		 NOT NULL,
	[Title]					[varchar](255)	 COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
INSERT INTO [dbo].[AutocompleteKind]
           (
		   [AutocompleteKindID]
           ,[Title])
SELECT
1,
'Бренд'
UNION ALL
SELECT
2,
'Комплектация'
UNION ALL
SELECT
3,
'Внешний вид'
go
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 6 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 6 is finished with errors' SET NOEXEC ON END
GO
--step 7: dbo.AutocompleteKind: add primary key pk_AutocompleteKind---------------------------------
GO
ALTER TABLE [dbo].[AutocompleteKind] ADD CONSTRAINT [pk_AutocompleteKind] PRIMARY KEY CLUSTERED ([AutocompleteKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 7 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 7 is finished with errors' SET NOEXEC ON END
GO
--step 8: dbo.AutocompleteItem: add foreign key AutocompleteKind_AutocompleteItem_1-----------------
GO
ALTER TABLE [dbo].[AutocompleteItem] ADD CONSTRAINT [AutocompleteKind_AutocompleteItem_1] FOREIGN KEY ([AutocompleteKindID]) REFERENCES [dbo].[AutocompleteKind] ([AutocompleteKindID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 8 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 8 is finished with errors' SET NOEXEC ON END
GO
--step 9: dbo.AutocompleteItem: add foreign key UserDomain_AutocompleteItem_1-----------------------
GO
ALTER TABLE [dbo].[AutocompleteItem] ADD CONSTRAINT [UserDomain_AutocompleteItem_1] FOREIGN KEY ([UserDomainID]) REFERENCES [dbo].[UserDomain] ([UserDomainID])
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 9 is finished with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 9 is finished with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully finished.' END
GO
