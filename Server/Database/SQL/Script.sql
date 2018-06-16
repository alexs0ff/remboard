/*
Created		23.07.2007
Modified		24.11.2016
Project		RubleExpress
Model			
Company		ООО "Рубль Экспресс"
Author		ООО "Рубль Экспресс"
Version		2
Database		MS SQL 2005 
*/


Create type [CurrentUser] from Varchar(255) NOT NULL
go

Create type [EventDate] from Smalldatetime NOT NULL
go

Create type [guidID] from Uniqueidentifier NOT NULL
go

Create type [CurrentDate] from Smalldatetime NOT NULL
go

Create type [incID] from Integer NOT NULL
go

Create type [Title] from Varchar(255) NOT NULL
go

Create type [UniqueNumber] from Varchar(50) NOT NULL
go

Create type [ShortTitle] from Varchar(50) NOT NULL
go

Create type [Account] from Varchar(50) NOT NULL
go

Create type [UniqueOperation] from Uniqueidentifier NOT NULL
go

Create type [Address] from Varchar(255) NOT NULL
go

Create type [EventDateTime] from Datetime NOT NULL
go

Create type [Description] from Text NOT NULL
go

Create type [INN] from Varchar(20) NOT NULL
go

Create type [KPP] from Varchar(20) NOT NULL
go

Create type [OGRN] from Varchar(20) NOT NULL
go

Create type [BankAccount] from Varchar(20) NOT NULL
go

Create type [CurrentDateTime] from Datetime NOT NULL
go

Create type [MemberPart] from Numeric(18,8) NOT NULL
go

Create type [GeoPointUnit] from Decimal(9,6) NOT NULL
go


Create table [User]
(
	[UserID] Uniqueidentifier Constraint [DS_User_UserID] Default NEWID() NOT NULL,
	[ProjectRoleID] Tinyint NOT NULL,
	[LoginName] Varchar(50) NOT NULL, UNIQUE ([LoginName]),
	[PasswordHash] Varchar(100) NOT NULL,
	[FirstName] Varchar(255) NOT NULL,
	[LastName] Varchar(255) NOT NULL,
	[MiddleName] Varchar(255) NULL,
	[Phone] Varchar(50) NULL,
	[Email] Varchar(255) NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_User_UserDomainID] Default NEWID() NOT NULL,
Constraint [pk_User] Primary Key ([UserID])
) 
go

Create table [ProjectRole]
(
	[ProjectRoleID] Tinyint NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_ProjectRole] Primary Key ([ProjectRoleID])
) 
go

Create table [Branch]
(
	[BranchID] Uniqueidentifier Constraint [DS_Branch_BranchID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[Address] Varchar(255) NOT NULL,
	[LegalName] Varchar(255) NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_Branch_UserDomainID] Default NEWID() NOT NULL,
Constraint [pk_Branch] Primary Key ([BranchID])
) 
go

Create table [UserBranchMap]
(
	[UserBranchMapID] Uniqueidentifier Constraint [DS_UserBranchMap_UserBranchMapID] Default NEWID() NOT NULL,
	[BranchID] Uniqueidentifier Constraint [DS_UserBranchMap_BranchID] Default NEWID() NOT NULL,
	[UserID] Uniqueidentifier Constraint [DS_UserBranchMap_UserID] Default NEWID() NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
Constraint [pk_UserBranchMap] Primary Key ([UserBranchMapID])
) 
go

Create table [RepairOrder]
(
	[RepairOrderID] Uniqueidentifier Constraint [DS_RepairOrder_RepairOrderID] Default NEWID() NOT NULL,
	[IssuerID] Uniqueidentifier Constraint [DS_RepairOrder_IssuerID] Default NEWID() NULL,
	[OrderStatusID] Uniqueidentifier Constraint [DS_RepairOrder_OrderStatusID] Default NEWID() NOT NULL,
	[EngineerID] Uniqueidentifier Constraint [DS_RepairOrder_EngineerID] Default NEWID() NULL,
	[ManagerID] Uniqueidentifier Constraint [DS_RepairOrder_ManagerID] Default NEWID() NOT NULL,
	[OrderKindID] Uniqueidentifier Constraint [DS_RepairOrder_OrderKindID] Default NEWID() NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
	[Number] Varchar(50) NOT NULL,
	[ClientFullName] Varchar(2000) NOT NULL,
	[ClientAddress] Varchar(max) NOT NULL,
	[ClientPhone] Varchar(255) NOT NULL,
	[ClientEmail] Varchar(255) NULL,
	[DeviceTitle] Varchar(2000) NOT NULL,
	[DeviceSN] Varchar(255) NULL,
	[DeviceTrademark] Varchar(255) NULL,
	[DeviceModel] Varchar(255) NULL,
	[Defect] Varchar(max) NOT NULL,
	[Options] Varchar(2000) NULL,
	[DeviceAppearance] Varchar(2000) NULL,
	[Notes] Varchar(max) NULL,
	[CallEventDate] Datetime NULL,
	[DateOfBeReady] Smalldatetime NOT NULL,
	[GuidePrice] Numeric(18,8) NULL,
	[PrePayment] Numeric(18,8) NULL,
	[IsUrgent] Bit NOT NULL,
	[Recommendation] Varchar(max) NULL,
	[IssueDate] Smalldatetime NULL,
	[WarrantyTo] Smalldatetime NULL,
	[BranchID] Uniqueidentifier Constraint [DS_RepairOrder_BranchID] Default NEWID() NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_RepairOrder_UserDomainID] Default NEWID() NOT NULL,
	[AccessPassword] Varchar(50) NULL,
Constraint [pk_RepairOrder] Primary Key ([RepairOrderID])
) 
go

Create table [OrderCapacity]
(
	[OrderCapacityID] Uniqueidentifier Constraint [DS_OrderCapacity_OrderCapacityID] Default NEWID() NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_OrderCapacity_UserDomainID] Default NEWID() NOT NULL, UNIQUE ([UserDomainID]),
	[OrderNumber] Bigint NOT NULL,
Constraint [pk_OrderCapacity] Primary Key ([OrderCapacityID])
) 
go

Create table [OrderKind]
(
	[OrderKindID] Uniqueidentifier Constraint [DS_OrderKind_OrderKindID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_OrderKind_UserDomainID] Default NEWID() NOT NULL,
Constraint [pk_OrderKind] Primary Key ([OrderKindID])
) 
go

Create table [OrderStatus]
(
	[OrderStatusID] Uniqueidentifier Constraint [DS_OrderStatus_OrderStatusID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[StatusKindID] Tinyint NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_OrderStatus_UserDomainID] Default NEWID() NOT NULL,
Constraint [pk_OrderStatus] Primary Key ([OrderStatusID])
) 
go

Create table [StatusKind]
(
	[StatusKindID] Tinyint NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_StatusKind] Primary Key ([StatusKindID])
) 
go

Create table [WorkItem]
(
	[WorkItemID] Uniqueidentifier Constraint [DS_WorkItem_WorkItemID] Default NEWID() NOT NULL,
	[UserID] Uniqueidentifier Constraint [DS_WorkItem_UserID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
	[Price] Numeric(18,8) NOT NULL,
	[RepairOrderID] Uniqueidentifier Constraint [DS_WorkItem_RepairOrderID] Default NEWID() NOT NULL,
	[Notes] Varchar(max) NULL,
Constraint [pk_WorkItem] Primary Key ([WorkItemID])
) 
go

Create table [DeviceItem]
(
	[DeviceItemID] Uniqueidentifier Constraint [DS_DeviceItem_DeviceItemID] Default NEWID() NOT NULL,
	[UserID] Uniqueidentifier Constraint [DS_DeviceItem_UserID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[Count] Numeric(18,8) NOT NULL,
	[CostPrice] Numeric(18,8) NOT NULL,
	[Price] Numeric(18,8) NOT NULL,
	[RepairOrderID] Uniqueidentifier Constraint [DS_DeviceItem_RepairOrderID] Default NEWID() NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
	[WarehouseItemID] Uniqueidentifier Constraint [DS_DeviceItem_WarehouseItemID] Default NEWID() NULL,
Constraint [pk_DeviceItem] Primary Key ([DeviceItemID])
) 
go

Create table [OrderTimeline]
(
	[OrderTimelineID] Uniqueidentifier Constraint [DS_OrderTimeline_OrderTimelineID] Default NEWID() NOT NULL,
	[TimelineKindID] Tinyint NOT NULL,
	[RepairOrderID] Uniqueidentifier Constraint [DS_OrderTimeline_RepairOrderID] Default NEWID() NOT NULL,
	[EventDateTime] Datetime NOT NULL,
	[Title] Varchar(1000) NOT NULL,
Constraint [pk_OrderTimeline] Primary Key ([OrderTimelineID])
) 
go

Create table [TimelineKind]
(
	[TimelineKindID] Tinyint NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_TimelineKind] Primary Key ([TimelineKindID])
) 
go

Create table [DocumentKind]
(
	[DocumentKindID] Tinyint NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_DocumentKind] Primary Key ([DocumentKindID])
) 
go

Create table [CustomReport]
(
	[CustomReportID] Uniqueidentifier Constraint [DS_CustomReport_CustomReportID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[HtmlContent] Text NOT NULL,
	[DocumentKindID] Tinyint NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_CustomReport_UserDomainID] Default NEWID() NOT NULL,
Constraint [pk_CustomReport] Primary Key ([CustomReportID])
) 
go

Create table [UserDomain]
(
	[UserDomainID] Uniqueidentifier Constraint [DS_UserDomain_UserDomainID] Default NEWID() NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
	[RegistredEmail] Varchar(255) NOT NULL, UNIQUE ([RegistredEmail]),
	[IsActive] Bit NOT NULL,
	[LegalName] Varchar(255) NOT NULL,
	[Trademark] Varchar(255) NOT NULL,
	[Address] Varchar(255) NOT NULL,
	[UserLogin] Varchar(255) NOT NULL,
	[PasswordHash] Varchar(100) NOT NULL,
	[Number] Integer Identity NOT NULL,
Constraint [pk_UserDomain] Primary Key ([UserDomainID])
) 
go

Create table [FinancialGroup]
(
	[FinancialGroupID] Uniqueidentifier Constraint [DS_FinancialGroup_FinancialGroupID] Default NEWID() NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_FinancialGroup_UserDomainID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[LegalName] Varchar(255) NOT NULL,
	[Trademark] Varchar(255) NOT NULL,
Constraint [pk_FinancialGroup] Primary Key ([FinancialGroupID])
) 
go

Create table [FinancialGroupBranchMap]
(
	[FinancialGroupBranchMapID] Uniqueidentifier Constraint [DS_FinancialGroupBranchMap_FinancialGroupBranchMapID] Default NEWID() NOT NULL,
	[BranchID] Uniqueidentifier Constraint [DS_FinancialGroupBranchMap_BranchID] Default NEWID() NOT NULL,
	[FinancialGroupID] Uniqueidentifier Constraint [DS_FinancialGroupBranchMap_FinancialGroupID] Default NEWID() NOT NULL,
Constraint [pk_FinancialGroupBranchMap] Primary Key ([FinancialGroupBranchMapID])
) 
go

Create table [TransactionKind]
(
	[TransactionKindID] Tinyint NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_TransactionKind] Primary Key ([TransactionKindID])
) 
go

Create table [FinancialItemKind]
(
	[FinancialItemKindID] Integer NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_FinancialItemKind] Primary Key ([FinancialItemKindID])
) 
go

Create table [FinancialItem]
(
	[FinancialItemID] Uniqueidentifier Constraint [DS_FinancialItem_FinancialItemID] Default NEWID() NOT NULL,
	[Title] Varchar(2000) NOT NULL,
	[Description] Varchar(max) NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_FinancialItem_UserDomainID] Default NEWID() NOT NULL,
	[FinancialItemKindID] Integer NOT NULL,
	[TransactionKindID] Tinyint NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
Constraint [pk_FinancialItem] Primary Key ([FinancialItemID])
) 
go

Create table [FinancialItemValue]
(
	[FinancialItemValueID] Uniqueidentifier Constraint [DS_FinancialItemValue_FinancialItemValueID] Default NEWID() NOT NULL,
	[FinancialGroupID] Uniqueidentifier Constraint [DS_FinancialItemValue_FinancialGroupID] Default NEWID() NOT NULL,
	[FinancialItemID] Uniqueidentifier Constraint [DS_FinancialItemValue_FinancialItemID] Default NEWID() NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
	[Amount] Numeric(18,8) NOT NULL,
	[CostAmount] Numeric(18,8) NULL,
	[Description] Varchar(255) NULL,
Constraint [pk_FinancialItemValue] Primary Key ([FinancialItemValueID])
) 
go

Create table [ItemCategory]
(
	[ItemCategoryID] Uniqueidentifier Constraint [DS_ItemCategory_ItemCategoryID] Default NEWID() NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_ItemCategory_UserDomainID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_ItemCategory] Primary Key ([ItemCategoryID])
) 
go

Create table [GoodsItem]
(
	[GoodsItemID] Uniqueidentifier Constraint [DS_GoodsItem_GoodsItemID] Default NEWID() NOT NULL,
	[DimensionKindID] Tinyint NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_GoodsItem_UserDomainID] Default NEWID() NOT NULL,
	[ItemCategoryID] Uniqueidentifier Constraint [DS_GoodsItem_ItemCategoryID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[Description] Varchar(4000) NULL,
	[UserCode] Varchar(255) NULL,
	[Particular] Varchar(255) NULL,
	[BarCode] Varchar(255) NULL,
Constraint [pk_GoodsItem] Primary Key ([GoodsItemID])
) 
go

Create table [DimensionKind]
(
	[DimensionKindID] Tinyint NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[ShortTitle] Varchar(50) NOT NULL,
Constraint [pk_DimensionKind] Primary Key ([DimensionKindID])
) 
go

Create table [Warehouse]
(
	[WarehouseID] Uniqueidentifier Constraint [DS_Warehouse_WarehouseID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_Warehouse_UserDomainID] Default NEWID() NOT NULL,
Constraint [pk_Warehouse] Primary Key ([WarehouseID])
) 
go

Create table [WarehouseItem]
(
	[WarehouseItemID] Uniqueidentifier Constraint [DS_WarehouseItem_WarehouseItemID] Default NEWID() NOT NULL,
	[WarehouseID] Uniqueidentifier Constraint [DS_WarehouseItem_WarehouseID] Default NEWID() NOT NULL,
	[GoodsItemID] Uniqueidentifier Constraint [DS_WarehouseItem_GoodsItemID] Default NEWID() NOT NULL,
	[Total] Numeric(18,8) NOT NULL,
	[StartPrice] Numeric(18,8) NOT NULL,
	[RepairPrice] Numeric(18,8) NOT NULL,
	[SalePrice] Numeric(18,8) NOT NULL,
Constraint [pk_WarehouseItem] Primary Key ([WarehouseItemID])
) 
go

Create table [Contractor]
(
	[ContractorID] Uniqueidentifier Constraint [DS_Contractor_ContractorID] Default NEWID() NOT NULL,
	[LegalName] Varchar(255) NOT NULL,
	[Trademark] Varchar(255) NOT NULL,
	[Address] Varchar(2000) NOT NULL,
	[Phone] Varchar(255) NULL,
	[EventDate] Smalldatetime NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_Contractor_UserDomainID] Default NEWID() NOT NULL,
Constraint [pk_Contractor] Primary Key ([ContractorID])
) 
go

Create table [IncomingDoc]
(
	[IncomingDocID] Uniqueidentifier Constraint [DS_IncomingDoc_IncomingDocID] Default NEWID() NOT NULL,
	[CreatorID] Uniqueidentifier Constraint [DS_IncomingDoc_CreatorID] Default NEWID() NOT NULL,
	[WarehouseID] Uniqueidentifier Constraint [DS_IncomingDoc_WarehouseID] Default NEWID() NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_IncomingDoc_UserDomainID] Default NEWID() NOT NULL,
	[ContractorID] Uniqueidentifier Constraint [DS_IncomingDoc_ContractorID] Default NEWID() NOT NULL,
	[DocDate] Smalldatetime NOT NULL,
	[DocNumber] Varchar(255) NOT NULL,
	[DocDescription] Varchar(255) NOT NULL,
Constraint [pk_IncomingDoc] Primary Key ([IncomingDocID])
) 
go

Create table [IncomingDocItem]
(
	[IncomingDocItemID] Uniqueidentifier Constraint [DS_IncomingDocItem_IncomingDocItemID] Default NEWID() NOT NULL,
	[IncomingDocID] Uniqueidentifier Constraint [DS_IncomingDocItem_IncomingDocID] Default NEWID() NOT NULL,
	[GoodsItemID] Uniqueidentifier Constraint [DS_IncomingDocItem_GoodsItemID] Default NEWID() NOT NULL,
	[Total] Numeric(18,8) NOT NULL,
	[InitPrice] Numeric(18,8) NOT NULL,
	[StartPrice] Numeric(18,8) NOT NULL,
	[RepairPrice] Numeric(18,8) NOT NULL,
	[SalePrice] Numeric(18,8) NOT NULL,
	[Description] Varchar(4000) NULL,
Constraint [pk_IncomingDocItem] Primary Key ([IncomingDocItemID])
) 
go

Create table [CancellationDoc]
(
	[CancellationDocID] Uniqueidentifier Constraint [DS_CancellationDoc_CancellationDocID] Default NEWID() NOT NULL,
	[WarehouseID] Uniqueidentifier Constraint [DS_CancellationDoc_WarehouseID] Default NEWID() NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_CancellationDoc_UserDomainID] Default NEWID() NOT NULL,
	[CreatorID] Uniqueidentifier Constraint [DS_CancellationDoc_CreatorID] Default NEWID() NOT NULL,
	[DocNumber] Varchar(255) NOT NULL,
	[DocDate] Smalldatetime NOT NULL,
	[DocDescription] Varchar(255) NOT NULL,
Constraint [pk_CancellationDoc] Primary Key ([CancellationDocID])
) 
go

Create table [CancellationDocItem]
(
	[CancellationDocItemID] Uniqueidentifier Constraint [DS_CancellationDocItem_CancellationDocItemID] Default NEWID() NOT NULL,
	[CancellationDocID] Uniqueidentifier Constraint [DS_CancellationDocItem_CancellationDocID] Default NEWID() NOT NULL,
	[GoodsItemID] Uniqueidentifier Constraint [DS_CancellationDocItem_GoodsItemID] Default NEWID() NOT NULL,
	[Total] Numeric(18,8) NOT NULL,
	[Description] Varchar(4000) NULL,
Constraint [pk_CancellationDocItem] Primary Key ([CancellationDocItemID])
) 
go

Create table [TransferDoc]
(
	[TransferDocID] Uniqueidentifier Constraint [DS_TransferDoc_TransferDocID] Default NEWID() NOT NULL,
	[SenderWarehouseID] Uniqueidentifier Constraint [DS_TransferDoc_SenderWarehouseID] Default NEWID() NOT NULL,
	[RecipientWarehouseID] Uniqueidentifier Constraint [DS_TransferDoc_RecipientWarehouseID] Default NEWID() NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_TransferDoc_UserDomainID] Default NEWID() NOT NULL,
	[CreatorID] Uniqueidentifier Constraint [DS_TransferDoc_CreatorID] Default NEWID() NOT NULL,
	[DocNumber] Varchar(255) NOT NULL,
	[DocDate] Smalldatetime NOT NULL,
	[DocDescription] Varchar(255) NOT NULL,
Constraint [pk_TransferDoc] Primary Key ([TransferDocID])
) 
go

Create table [TransferDocItem]
(
	[TransferDocItemID] Uniqueidentifier Constraint [DS_TransferDocItem_TransferDocItemID] Default NEWID() NOT NULL,
	[TransferDocID] Uniqueidentifier Constraint [DS_TransferDocItem_TransferDocID] Default NEWID() NOT NULL,
	[GoodsItemID] Uniqueidentifier Constraint [DS_TransferDocItem_GoodsItemID] Default NEWID() NOT NULL,
	[Total] Numeric(18,8) NOT NULL,
	[Description] Varchar(4000) NULL,
Constraint [pk_TransferDocItem] Primary Key ([TransferDocItemID])
) 
go

Create table [ProcessedWarehouseDoc]
(
	[ProcessedWarehouseDocID] Uniqueidentifier Constraint [DS_ProcessedWarehouseDoc_ProcessedWarehouseDocID] Default NEWID() NOT NULL,
	[WarehouseID] Uniqueidentifier Constraint [DS_ProcessedWarehouseDoc_WarehouseID] Default NEWID() NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
	[UTCEventDateTime] Datetime NOT NULL,
	[UserID] Uniqueidentifier Constraint [DS_ProcessedWarehouseDoc_UserID] Default NEWID() NOT NULL,
Constraint [pk_ProcessedWarehouseDoc] Primary Key ([ProcessedWarehouseDocID])
) 
go

Create table [FinancialGroupWarehouseMap]
(
	[FinancialGroupWarehouseMapID] Uniqueidentifier Constraint [DS_FinancialGroupWarehouseMap_FinancialGroupWarehouseMapID] Default NEWID() NOT NULL,
	[WarehouseID] Uniqueidentifier Constraint [DS_FinancialGroupWarehouseMap_WarehouseID] Default NEWID() NOT NULL,
	[FinancialGroupID] Uniqueidentifier Constraint [DS_FinancialGroupWarehouseMap_FinancialGroupID] Default NEWID() NOT NULL,
Constraint [pk_FinancialGroupWarehouseMap] Primary Key ([FinancialGroupWarehouseMapID])
) 
go

Create table [RecoveryLogin]
(
	[RecoveryLoginID] Uniqueidentifier Constraint [DS_RecoveryLogin_RecoveryLoginID] Default NEWID() NOT NULL,
	[LoginName] Varchar(50) NOT NULL,
	[UTCEventDateTime] Datetime NOT NULL,
	[UTCEventDate] Smalldatetime NOT NULL,
	[RecoveryEmail] Varchar(255) NOT NULL,
	[RecoveryClientIdentifier] Varchar(50) NOT NULL,
	[IsRecovered] Bit NOT NULL,
	[RecoveredClientIdentifier] Varchar(50) NULL,
	[SentNumber] Varchar(255) NOT NULL, UNIQUE ([SentNumber]),
	[UTCRecoveredDateTime] Datetime NULL,
Constraint [pk_RecoveryLogin] Primary Key ([RecoveryLoginID])
) 
go

Create table [UserPublicKey]
(
	[UserPublicKeyID] Uniqueidentifier Constraint [DS_UserPublicKey_UserPublicKeyID] Default NEWID() NOT NULL,
	[UserID] Uniqueidentifier Constraint [DS_UserPublicKey_UserID] Default NEWID() NOT NULL,
	[Number] Varchar(50) NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
	[PublicKeyData] Varchar(2000) NOT NULL,
	[ClientIdentifier] Varchar(50) NOT NULL,
	[KeyNotes] Varchar(1000) NULL,
	[IsRevoked] Bit NOT NULL,
Constraint [pk_UserPublicKey] Primary Key ([UserPublicKeyID])
) 
go

Create table [UserPublicKeyRequest]
(
	[UserPublicKeyRequestID] Uniqueidentifier Constraint [DS_UserPublicKeyRequest_UserPublicKeyRequestID] Default NEWID() NOT NULL, UNIQUE ([UserPublicKeyRequestID]),
	[UserID] Uniqueidentifier Constraint [DS_UserPublicKeyRequest_UserID] Default NEWID() NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
	[PublicKeyData] Varchar(2000) NOT NULL,
	[Number] Varchar(50) NOT NULL,
	[KeyNotes] Varchar(1000) NULL,
	[ClientIdentifier] Varchar(50) NOT NULL,
Constraint [pk_UserPublicKeyRequest] Primary Key ([UserPublicKeyRequestID])
) 
go

Create table [AutocompleteItem]
(
	[AutocompleteItemID] Uniqueidentifier Constraint [DS_AutocompleteItem_AutocompleteItemID] Default NEWID() NOT NULL,
	[UserDomainID] Uniqueidentifier Constraint [DS_AutocompleteItem_UserDomainID] Default NEWID() NOT NULL,
	[AutocompleteKindID] Tinyint NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_AutocompleteItem] Primary Key ([AutocompleteItemID])
) 
go

Create table [AutocompleteKind]
(
	[AutocompleteKindID] Tinyint NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_AutocompleteKind] Primary Key ([AutocompleteKindID])
) 
go

Create table [InterestKind]
(
	[InterestKindID] Tinyint NOT NULL,
	[Title] Varchar(255) NOT NULL,
Constraint [pk_InterestKind] Primary Key ([InterestKindID])
) 
go

Create table [UserInterest]
(
	[UserInterestID] Uniqueidentifier Constraint [DS_UserInterest_UserInterestID] Default NEWID() NOT NULL,
	[UserID] Uniqueidentifier Constraint [DS_UserInterest_UserID] Default NEWID() NOT NULL,
	[EventDate] Smalldatetime NOT NULL,
	[WorkInterestKindID] Tinyint NOT NULL,
	[WorkValue] Numeric(18,8) NULL,
	[DeviceInterestKindID] Tinyint NOT NULL,
	[DeviceValue] Numeric(18,8) NULL,
	[Description] Varchar(max) NULL,
Constraint [pk_UserInterest] Primary Key ([UserInterestID])
) 
go

Create table [UserGridState]
(
	[UserGridStateID] Uniqueidentifier NOT NULL,
	[GridName] Varchar(255) NOT NULL,
	[StateGrid] Nvarchar(max) NOT NULL,
	[UserID] Uniqueidentifier NOT NULL,
Constraint [pk_UserGridState] Primary Key nonclustered ([UserGridStateID])
) 
go

Create table [UserGridFilter]
(
	[UserGridFilterID] Uniqueidentifier Constraint [DS_UserGridFilter_UserGridFilterID] Default NEWID() NOT NULL,
	[Title] Varchar(255) NOT NULL,
	[GridName] Varchar(255) NOT NULL,
	[FilterData] Nvarchar(max) NOT NULL,
	[UserID] Uniqueidentifier NOT NULL,
Constraint [pk_UserGridFilter] Primary Key ([UserGridFilterID])
) 
go

Create table [UserSettings]
(
	[UserSettingsID] Uniqueidentifier Constraint [DS_UserSettings_UserSettingsID] Default NEWID() NOT NULL,
	[Number] Nvarchar(255) NOT NULL,
	[UserLogin] Nvarchar(50) NOT NULL,
	[Data] Nvarchar(max) NOT NULL,
Constraint [pk_UserSettings] Primary Key nonclustered ([UserSettingsID])
) 
go

Create table [UserDomainSettings]
(
	[UserDomainSettingsID] Uniqueidentifier Constraint [DS_UserDomainSettings_UserDomainSettingsID] Default NEWID() NOT NULL,
	[UserDomainID] Uniqueidentifier NOT NULL,
	[Number] Nvarchar(255) NOT NULL,
	[Data] Nvarchar(max) NOT NULL,
Constraint [pk_UserDomainSettings] Primary Key nonclustered ([UserDomainSettingsID])
) 
go


Create UNIQUE Index [UserDomainOrderNumberIndex] ON [RepairOrder] ([UserDomainID] ,[Number] ) 
go
Create UNIQUE Index [FinancialGroupBranchMapIndex] ON [FinancialGroupBranchMap] ([BranchID] ,[FinancialGroupID] ) 
go
Create UNIQUE Index [FinancialGroupWarehouseMapIndex] ON [FinancialGroupWarehouseMap] ([WarehouseID] ,[FinancialGroupID] ) 
go
Create UNIQUE CLUSTERED Index [UserGridIndex] ON [UserGridState] ([UserID] ,[GridName] ) 
go
Create UNIQUE CLUSTERED Index [LoginNumberSettingsIndex] ON [UserSettings] ([UserLogin] ,[Number] ) 
go
Create UNIQUE CLUSTERED Index [UserDomainIDNumberIndex] ON [UserDomainSettings] ([UserDomainID] ,[Number] ) 
go


Alter table [UserBranchMap] add Constraint [User_UserBranchMap_1] foreign key([UserID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [RepairOrder] add Constraint [User_RepairOrder_1] foreign key([ManagerID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [RepairOrder] add Constraint [User_RepairOrder_2] foreign key([EngineerID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [RepairOrder] add Constraint [User_RepairOrder_3] foreign key([IssuerID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [WorkItem] add Constraint [User_WorkItem_1] foreign key([UserID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [DeviceItem] add Constraint [User_DeviceItem_1] foreign key([UserID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [IncomingDoc] add Constraint [User_IncomingDoc_1] foreign key([CreatorID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [CancellationDoc] add Constraint [User_CancellationDoc_1] foreign key([CreatorID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [TransferDoc] add Constraint [User_TransferDoc_1] foreign key([CreatorID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [UserPublicKey] add Constraint [User_UserPublicKey_1] foreign key([UserID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [UserPublicKeyRequest] add Constraint [User_UserPublicKeyRequest_1] foreign key([UserID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [UserInterest] add Constraint [User_UserInterest_1] foreign key([UserID]) references [User] ([UserID])  on update no action on delete no action 
go
Alter table [User] add Constraint [ProjectRole_User_1] foreign key([ProjectRoleID]) references [ProjectRole] ([ProjectRoleID])  on update no action on delete no action 
go
Alter table [UserBranchMap] add Constraint [Branch_UserBranchMap_1] foreign key([BranchID]) references [Branch] ([BranchID])  on update no action on delete no action 
go
Alter table [RepairOrder] add Constraint [Branch_RepairOrder_1] foreign key([BranchID]) references [Branch] ([BranchID])  on update no action on delete no action 
go
Alter table [FinancialGroupBranchMap] add Constraint [Branch_FinancialGroupBranchMap_1] foreign key([BranchID]) references [Branch] ([BranchID])  on update no action on delete no action 
go
Alter table [DeviceItem] add Constraint [RepairOrder_DeviceItem_1] foreign key([RepairOrderID]) references [RepairOrder] ([RepairOrderID])  on update no action on delete no action 
go
Alter table [WorkItem] add Constraint [RepairOrder_WorkItem_1] foreign key([RepairOrderID]) references [RepairOrder] ([RepairOrderID])  on update no action on delete no action 
go
Alter table [OrderTimeline] add Constraint [RepairOrder_OrderTimeline_1] foreign key([RepairOrderID]) references [RepairOrder] ([RepairOrderID])  on update no action on delete no action 
go
Alter table [RepairOrder] add Constraint [OrderKind_RepairOrder_1] foreign key([OrderKindID]) references [OrderKind] ([OrderKindID])  on update no action on delete no action 
go
Alter table [RepairOrder] add Constraint [OrderStatus_RepairOrder_1] foreign key([OrderStatusID]) references [OrderStatus] ([OrderStatusID])  on update no action on delete no action 
go
Alter table [OrderStatus] add Constraint [StatusKind_OrderStatus_1] foreign key([StatusKindID]) references [StatusKind] ([StatusKindID])  on update no action on delete no action 
go
Alter table [OrderTimeline] add Constraint [TimelineKind_OrderTimeline_1] foreign key([TimelineKindID]) references [TimelineKind] ([TimelineKindID])  on update no action on delete no action 
go
Alter table [CustomReport] add Constraint [DocumentKind_CustomReport_1] foreign key([DocumentKindID]) references [DocumentKind] ([DocumentKindID])  on update no action on delete no action 
go
Alter table [User] add Constraint [UserDomain_User_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [OrderKind] add Constraint [UserDomain_OrderKind_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [OrderStatus] add Constraint [UserDomain_OrderStatus_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [CustomReport] add Constraint [UserDomain_CustomReport_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [Branch] add Constraint [UserDomain_Branch_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [RepairOrder] add Constraint [UserDomain_RepairOrder_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [OrderCapacity] add Constraint [UserDomain_OrderCapacity_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [FinancialGroup] add Constraint [UserDomain_FinancialGroup_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [FinancialItem] add Constraint [UserDomain_FinancialItem_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [ItemCategory] add Constraint [UserDomain_ItemCategory_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [GoodsItem] add Constraint [UserDomain_GoodsItem_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [Warehouse] add Constraint [UserDomain_Warehouse_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [IncomingDoc] add Constraint [UserDomain_IncomingDoc_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [CancellationDoc] add Constraint [UserDomain_CancellationDoc_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [TransferDoc] add Constraint [UserDomain_TransferDoc_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [Contractor] add Constraint [UserDomain_Contractor_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [AutocompleteItem] add Constraint [UserDomain_AutocompleteItem_1] foreign key([UserDomainID]) references [UserDomain] ([UserDomainID])  on update no action on delete no action 
go
Alter table [FinancialGroupBranchMap] add Constraint [FinancialGroup_FinancialGroupBranchMap_1] foreign key([FinancialGroupID]) references [FinancialGroup] ([FinancialGroupID])  on update no action on delete no action 
go
Alter table [FinancialItemValue] add Constraint [FinancialGroup_FinancialItemValue_1] foreign key([FinancialGroupID]) references [FinancialGroup] ([FinancialGroupID])  on update no action on delete no action 
go
Alter table [FinancialGroupWarehouseMap] add Constraint [FinancialGroup_FinancialGroupWarehouseMap_1] foreign key([FinancialGroupID]) references [FinancialGroup] ([FinancialGroupID])  on update no action on delete no action 
go
Alter table [FinancialItem] add Constraint [TransactionKind_FinancialItem_1] foreign key([TransactionKindID]) references [TransactionKind] ([TransactionKindID])  on update no action on delete no action 
go
Alter table [FinancialItem] add Constraint [FinancialItemKind_FinancialItem_1] foreign key([FinancialItemKindID]) references [FinancialItemKind] ([FinancialItemKindID])  on update no action on delete no action 
go
Alter table [FinancialItemValue] add Constraint [FinancialItem_FinancialItemValue_1] foreign key([FinancialItemID]) references [FinancialItem] ([FinancialItemID])  on update no action on delete no action 
go
Alter table [GoodsItem] add Constraint [ItemCategory_GoodsItem_1] foreign key([ItemCategoryID]) references [ItemCategory] ([ItemCategoryID])  on update no action on delete no action 
go
Alter table [WarehouseItem] add Constraint [GoodsItem_WarehouseItem_1] foreign key([GoodsItemID]) references [GoodsItem] ([GoodsItemID])  on update no action on delete no action 
go
Alter table [IncomingDocItem] add Constraint [GoodsItem_IncomingDocItem_1] foreign key([GoodsItemID]) references [GoodsItem] ([GoodsItemID])  on update no action on delete no action 
go
Alter table [CancellationDocItem] add Constraint [GoodsItem_CancellationDocItem_1] foreign key([GoodsItemID]) references [GoodsItem] ([GoodsItemID])  on update no action on delete no action 
go
Alter table [TransferDocItem] add Constraint [GoodsItem_TransferDocItem_1] foreign key([GoodsItemID]) references [GoodsItem] ([GoodsItemID])  on update no action on delete no action 
go
Alter table [GoodsItem] add Constraint [DimensionKind_GoodsItem_1] foreign key([DimensionKindID]) references [DimensionKind] ([DimensionKindID])  on update no action on delete no action 
go
Alter table [WarehouseItem] add Constraint [Warehouse_WarehouseItem_1] foreign key([WarehouseID]) references [Warehouse] ([WarehouseID])  on update no action on delete no action 
go
Alter table [IncomingDoc] add Constraint [Warehouse_IncomingDoc_1] foreign key([WarehouseID]) references [Warehouse] ([WarehouseID])  on update no action on delete no action 
go
Alter table [TransferDoc] add Constraint [Warehouse_TransferDoc_1] foreign key([SenderWarehouseID]) references [Warehouse] ([WarehouseID])  on update no action on delete no action 
go
Alter table [TransferDoc] add Constraint [Warehouse_TransferDoc_2] foreign key([RecipientWarehouseID]) references [Warehouse] ([WarehouseID])  on update no action on delete no action 
go
Alter table [CancellationDoc] add Constraint [Warehouse_CancellationDoc_1] foreign key([WarehouseID]) references [Warehouse] ([WarehouseID])  on update no action on delete no action 
go
Alter table [ProcessedWarehouseDoc] add Constraint [Warehouse_ProcessedWarehouseDoc_1] foreign key([WarehouseID]) references [Warehouse] ([WarehouseID])  on update no action on delete no action 
go
Alter table [FinancialGroupWarehouseMap] add Constraint [Warehouse_FinancialGroupWarehouseMap_1] foreign key([WarehouseID]) references [Warehouse] ([WarehouseID])  on update no action on delete no action 
go
Alter table [DeviceItem] add Constraint [WarehouseItem_DeviceItem_1] foreign key([WarehouseItemID]) references [WarehouseItem] ([WarehouseItemID])  on update no action on delete no action 
go
Alter table [IncomingDoc] add Constraint [Contractor_IncomingDoc_1] foreign key([ContractorID]) references [Contractor] ([ContractorID])  on update no action on delete no action 
go
Alter table [IncomingDocItem] add Constraint [IncomingDoc_IncomingDocItem_1] foreign key([IncomingDocID]) references [IncomingDoc] ([IncomingDocID])  on update no action on delete no action 
go
Alter table [CancellationDocItem] add Constraint [CancellationDoc_CancellationDocItem_1] foreign key([CancellationDocID]) references [CancellationDoc] ([CancellationDocID])  on update no action on delete no action 
go
Alter table [TransferDocItem] add Constraint [TransferDoc_TransferDocItem_1] foreign key([TransferDocID]) references [TransferDoc] ([TransferDocID])  on update no action on delete no action 
go
Alter table [AutocompleteItem] add Constraint [AutocompleteKind_AutocompleteItem_1] foreign key([AutocompleteKindID]) references [AutocompleteKind] ([AutocompleteKindID])  on update no action on delete no action 
go
Alter table [UserInterest] add Constraint [InterestKind_UserInterest_1] foreign key([WorkInterestKindID]) references [InterestKind] ([InterestKindID])  on update no action on delete no action 
go
Alter table [UserInterest] add Constraint [InterestKind_UserInterest_2] foreign key([DeviceInterestKindID]) references [InterestKind] ([InterestKindID])  on update no action on delete no action 
go


Set quoted_identifier on
go


Set quoted_identifier off
go


