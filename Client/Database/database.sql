CREATE TABLE Warehouse
(
	WarehouseID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL
);

CREATE TABLE TimelineKind
(
	TimelineKindID Integer PRIMARY KEY NOT NULL,
	Title Text(50) NOT NULL
);

CREATE TABLE FinancialGroup
(
	FinancialGroupID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL,
	LegalName Text(255) NOT NULL,
	Trademark Text(255) NOT NULL
);

CREATE TABLE RepairOrderServerHash
(
	RepairOrderServerHashID Text(50) PRIMARY KEY NOT NULL,
	DataHash Text(50) NOT NULL,
	OrderTimelinesCount Integer NOT NULL
);

CREATE TABLE StatusKind
(
	StatusKindID Integer PRIMARY KEY NOT NULL,
	Title Text(50) NOT NULL
);

CREATE TABLE OrderKind
(
	OrderKindID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL
);

CREATE TABLE DimensionKind
(
	DimensionKindID Integer PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL,
	ShortTitle Text(50) NOT NULL
);

CREATE TABLE ItemCategory
(
	ItemCategoryID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL
);

CREATE TABLE DocumentKind
(
	DocumentKindID Integer PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL
);

CREATE TABLE Branch
(
	BranchID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL,
	Address Text(255) NOT NULL,
	LegalName Text(255) NOT NULL
);

CREATE TABLE ProjectRole
(
	ProjectRoleID Integer PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL
);

CREATE TABLE Settings
(
	Version Integer NOT NULL
);

CREATE TABLE RemSettings
(
	SetKey Text(255) PRIMARY KEY UNIQUE NOT NULL,
	SetVal Text(4000) NOT NULL
);

CREATE TABLE DocSequence
(
	CurrentNumber Integer NOT NULL
);

CREATE TABLE DeviceItemServerHash
(
	DeviceItemServerHashID Text(50) PRIMARY KEY NOT NULL,
	DataHash Text(50) NOT NULL,
	RepairOrderServerHashID Text(50) NOT NULL,
	FOREIGN KEY (RepairOrderServerHashID) REFERENCES RepairOrderServerHash(RepairOrderServerHashID)
);

CREATE TABLE WorkItemServerHash
(
	WorkItemServerHashID Text(50) PRIMARY KEY NOT NULL,
	DataHash Text(50) NOT NULL,
	RepairOrderServerHashID Text(50) NOT NULL,
	FOREIGN KEY (RepairOrderServerHashID) REFERENCES RepairOrderServerHash(RepairOrderServerHashID)
);

CREATE TABLE OrderStatus
(
	OrderStatusID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL,
	StatusKindID Integer NOT NULL,
	FOREIGN KEY (StatusKindID) REFERENCES StatusKind(StatusKindID)
);

CREATE TABLE SyncOperation
(
	SyncOperationID Text(50) PRIMARY KEY UNIQUE NOT NULL,
	OperationBeginTime Text(50) NOT NULL,
	OperationEndTime Text(50),
	IsSuccess Integer NOT NULL,
	Comment Text(2000),
	UserID Text(50) NOT NULL,
	FOREIGN KEY (UserID) REFERENCES User(UserID)
);

CREATE TABLE UserKey
(
	UserKeyID Text(50) PRIMARY KEY UNIQUE NOT NULL,
	EventDate Text(50) NOT NULL,
	PublicKeyData Text(2000) NOT NULL,
	PrivateKeyData Text(2000) NOT NULL,
	Number Text(50),
	IsActivated Integer NOT NULL,
	UserID Text(50) NOT NULL,
	FOREIGN KEY (UserID) REFERENCES User(UserID)
);

CREATE TABLE User
(
	UserID Text(50) PRIMARY KEY NOT NULL,
	LoginName Text(50) UNIQUE NOT NULL,
	PasswordHash Text(100) NOT NULL,
	FirstName Text(255) NOT NULL,
	LastName Text(255) NOT NULL,
	MiddleName Text(255),
	Phone Text(50),
	Email Text(255),
	DomainID Text(50),
	ProjectRoleID Integer NOT NULL,
	FOREIGN KEY (ProjectRoleID) REFERENCES ProjectRole(ProjectRoleID)
);

CREATE TABLE CustomReport
(
	CustomReportID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL,
	HtmlContent Text(10000) NOT NULL,
	DocumentKindID Integer NOT NULL,
	FOREIGN KEY (DocumentKindID) REFERENCES DocumentKind(DocumentKindID)
);

CREATE TABLE GoodsItem
(
	GoodsItemID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL,
	Description Text(4000),
	UserCode Text(255),
	Particular Text(255),
	BarCode Text(255),
	ItemCategoryID Text(50) NOT NULL,
	DimensionKindID Integer NOT NULL,
	FOREIGN KEY (ItemCategoryID) REFERENCES ItemCategory(ItemCategoryID),
	FOREIGN KEY (DimensionKindID) REFERENCES DimensionKind(DimensionKindID)
);

CREATE TABLE UserBranchMap
(
	UserBranchMapID Text(50) PRIMARY KEY UNIQUE NOT NULL,
	EventDate Text(50) NOT NULL,
	UserID Text(50) NOT NULL,
	BranchID Text(50) NOT NULL,
	FOREIGN KEY (UserID) REFERENCES User(UserID),
	FOREIGN KEY (BranchID) REFERENCES Branch(BranchID)
);

CREATE TABLE FinancialGroupBranchMap
(
	FinancialGroupBranchMapID Text(50) PRIMARY KEY NOT NULL,
	BranchID Text(50) NOT NULL,
	FinancialGroupID Text(50) NOT NULL,
	FOREIGN KEY (BranchID) REFERENCES Branch(BranchID),
	FOREIGN KEY (FinancialGroupID) REFERENCES FinancialGroup(FinancialGroupID)
);

CREATE TABLE DeviceItem
(
	DeviceItemID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL,
	Count Real NOT NULL,
	CostPrice Real,
	EventDate Text(50) NOT NULL,
	WarehouseItemID Text(50),
	Price Real NOT NULL,
	UserID Text(50) NOT NULL,
	RepairOrderID Text(50) NOT NULL,
	FOREIGN KEY (UserID) REFERENCES User(UserID),
	FOREIGN KEY (RepairOrderID) REFERENCES RepairOrder(RepairOrderID)
);

CREATE TABLE WorkItem
(
	WorkItemID Text(50) PRIMARY KEY NOT NULL,
	Title Text(255) NOT NULL,
	EventDate Text(50) NOT NULL,
	Price Real NOT NULL,
	UserID Text(50) NOT NULL,
	RepairOrderID Text(50) NOT NULL,
	FOREIGN KEY (UserID) REFERENCES User(UserID),
	FOREIGN KEY (RepairOrderID) REFERENCES RepairOrder(RepairOrderID)
);

CREATE TABLE WarehouseItem
(
	WarehouseItemID Text(50) PRIMARY KEY NOT NULL,
	Total Real NOT NULL,
	StartPrice Real NOT NULL,
	RepairPrice Real,
	SalePrice Real NOT NULL,
	GoodsItemID Text(50) NOT NULL,
	WarehouseID Text(50) NOT NULL,
	FOREIGN KEY (GoodsItemID) REFERENCES GoodsItem(GoodsItemID),
	FOREIGN KEY (WarehouseID) REFERENCES Warehouse(WarehouseID)
);

CREATE TABLE OrderTimeline
(
	OrderTimelineID Text(50) PRIMARY KEY NOT NULL,
	EventDateTime Text(50) NOT NULL,
	Title Text(1000) NOT NULL,
	TimelineKindID Integer NOT NULL,
	RepairOrderID Text(50) NOT NULL,
	FOREIGN KEY (TimelineKindID) REFERENCES TimelineKind(TimelineKindID),
	FOREIGN KEY (RepairOrderID) REFERENCES RepairOrder(RepairOrderID)
);

CREATE TABLE FinancialGroupWarehouseMap
(
	FinancialGroupWarehouseMapID Text(50) PRIMARY KEY NOT NULL,
	WarehouseID Text(50) NOT NULL,
	FinancialGroupID Text(50) NOT NULL,
	FOREIGN KEY (WarehouseID) REFERENCES Warehouse(WarehouseID),
	FOREIGN KEY (FinancialGroupID) REFERENCES FinancialGroup(FinancialGroupID)
);

CREATE TABLE RepairOrder
(
	RepairOrderID Text(50) PRIMARY KEY NOT NULL,
	IssuerID Text(50),
	EngineerID Text(50),
	ManagerID Text(50) NOT NULL,
	EventDate Text(50) NOT NULL,
	Number Text(50) NOT NULL,
	ClientFullName Text(1000) NOT NULL,
	ClientAddress Text(2000) NOT NULL,
	ClientPhone Text(50) NOT NULL,
	ClientEmail Text(50),
	DeviceTitle Text(2000) NOT NULL,
	DeviceSN Text(255),
	DeviceTrademark Text(255),
	DeviceModel Text(255),
	Defect Text(2000) NOT NULL,
	Options Text(2000),
	DeviceAppearance Text(2000),
	Notes Text(2000),
	CallEventDate Text(50),
	DateOfBeReady Text(50),
	GuidePrice Real,
	PrePayment Real,
	IsUrgent Integer,
	Recommendation Text(2000),
	IssueDate Text(50),
	WarrantyTo Text(50),
	OrderStatusID Text(50) NOT NULL,
	OrderKindID Text(50) NOT NULL,
	BranchID Text(50) NOT NULL,
	FOREIGN KEY (OrderStatusID) REFERENCES OrderStatus(OrderStatusID),
	FOREIGN KEY (OrderKindID) REFERENCES OrderKind(OrderKindID),
	FOREIGN KEY (BranchID) REFERENCES Branch(BranchID)
);