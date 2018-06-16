using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Client.DataLayer.EntityFramework;

namespace Remontinka.Client.DataLayer.Test
{
    [TestFixture]
    public class EntityDeployTest
    {
        private RemontinkaStore CreateStore()
        {
            return new RemontinkaStore();
        }

        [Test]
        public void UserTest()
        {
            var store = CreateStore();
            var item = new User();

            item.Email = "Email";
            item.LoginName = "LoginName";
            item.FirstName = "Тестовый";
            item.LastName = "LastName";
            item.LoginName = "LoginName";
            item.MiddleName = "MiddleName";
            item.PasswordHash = "PasswordHash";
            item.Phone = "Phone";
            item.ProjectRoleID = ProjectRoleSet.Admin.ProjectRoleID;

            item.DomainIDGuid = Guid.NewGuid();

            store.SaveUser(item);

            var savedItem = store.GetUser(item.UserIDGuid);

            Assert.IsNotNull(savedItem);
            Assert.AreEqual(item.Email, savedItem.Email);
            Assert.AreEqual(item.FirstName, savedItem.FirstName);
            Assert.AreEqual(item.LastName, savedItem.LastName);
            Assert.AreEqual(item.MiddleName, savedItem.MiddleName);
            Assert.AreEqual(item.LoginName, savedItem.LoginName);
            Assert.AreEqual(item.PasswordHash, savedItem.PasswordHash);
            Assert.AreEqual(item.Phone, savedItem.Phone);
            Assert.AreEqual(item.ProjectRoleID, savedItem.ProjectRoleID);
            Assert.AreEqual(item.UserID, savedItem.UserID);
            Assert.AreEqual(item.DomainIDGuid, savedItem.DomainIDGuid);

            store.DeleteUser(item.UserIDGuid);
            savedItem = store.GetUser(item.UserIDGuid);

            Assert.IsNull(savedItem);
        }

        private readonly Guid _testUserID = new Guid("78BA3B14-3D94-414C-9001-432F812DD19B");

        private User CreateTestUser()
        {

            var store = CreateStore();
            var item = new User();
            item.Email = "Email";
            item.LoginName = "LoginName";
            item.FirstName = "FirstName";
            item.LastName = "LastName";
            item.LoginName = "LoginName";
            item.MiddleName = "MiddleName";
            item.PasswordHash = "PasswordHash";
            item.Phone = "Phone";
            item.ProjectRoleID = ProjectRoleSet.Admin.ProjectRoleID;
            item.UserIDGuid = _testUserID;

            item.DomainIDGuid = Guid.NewGuid();
            store.SaveUser(item);

            return item;
        }

        private void DeleteTestUser()
        {
            var store = CreateStore();

            store.DeleteUser(_testUserID);
        }


        [Test]
        public void UserKeyTest()
        {
            var store = CreateStore();

            var user = CreateTestUser();
            try
            {
                var item = new UserKey();
                item.EventDateDateTime = new DateTime(2015,12,23,1,2,3);
                item.Number = "Number";
                item.PrivateKeyData = "PrivateKeyData";
                item.PublicKeyData = "PublicKeyData";
                item.UserIDGuid = user.UserIDGuid;
                item.IsActivatedBool = true;
                
                store.SaveUserKey(item);

                var savedItem = store.GetUserKey(item.UserKeyIDGuid);
                
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.EventDate,savedItem.EventDate);
                Assert.AreEqual(item.IsActivated, savedItem.IsActivated);
                Assert.AreEqual(item.Number,savedItem.Number);
                Assert.AreEqual(item.PrivateKeyData,savedItem.PrivateKeyData);
                Assert.AreEqual(item.PublicKeyData,savedItem.PublicKeyData);
                Assert.AreEqual(item.UserID, savedItem.UserID);

                item.EventDateDateTime = new DateTime(2016, 11, 15);
                item.Number = "Number2";
                item.PrivateKeyData = "PrivateKeyData3";
                item.PublicKeyData = "PublicKeyData4";
                item.IsActivatedBool = false;

                store.SaveUserKey(item);

                savedItem = store.GetUserKey(item.UserKeyIDGuid);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.EventDate, savedItem.EventDate);
                Assert.AreEqual(item.Number, savedItem.Number);
                Assert.AreEqual(item.PrivateKeyData, savedItem.PrivateKeyData);
                Assert.AreEqual(item.PublicKeyData, savedItem.PublicKeyData);
                Assert.AreEqual(item.UserID, savedItem.UserID);
                Assert.AreEqual(item.IsActivated, savedItem.IsActivated);

                store.DeleteUserKey(savedItem.UserKeyIDGuid);

                savedItem = store.GetUserKey(item.UserKeyIDGuid);

                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestUser();
            }
        }

         [Test]
        public void SyncOperationTest()
        {
            var store = CreateStore();

            var user = CreateTestUser();
            try
            {
                var operation = new SyncOperation();
                operation.UserIDGuid = user.UserIDGuid;
                operation.Comment = "Comment";
                operation.IsSuccessBoolean = true;
                operation.OperationBeginTimeDateTime = new DateTime(2015,06,05);
                operation.OperationEndTimeDateTime = new DateTime(2015,05,06);
                
                store.SaveSyncOperation(operation);

                var savedItem = store.GetSyncOperation(operation.SyncOperationIDGuid);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(operation.Comment,savedItem.Comment);
                Assert.AreEqual(operation.IsSuccess, savedItem.IsSuccess);
                Assert.AreEqual(operation.OperationBeginTime, savedItem.OperationBeginTime);
                Assert.AreEqual(operation.OperationEndTime, savedItem.OperationEndTime);
                Assert.AreEqual(operation.UserID, savedItem.UserID);
                
                operation.Comment = "Comment2";
                operation.IsSuccessBoolean = false;
                operation.OperationBeginTimeDateTime = new DateTime(2015, 07, 05);
                operation.OperationEndTimeDateTime = new DateTime(2015, 08, 06);

                store.SaveSyncOperation(operation);

                savedItem = store.GetSyncOperation(operation.SyncOperationIDGuid);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(operation.Comment, savedItem.Comment);
                Assert.AreEqual(operation.IsSuccess, savedItem.IsSuccess);
                Assert.AreEqual(operation.OperationBeginTime, savedItem.OperationBeginTime);
                Assert.AreEqual(operation.OperationEndTime, savedItem.OperationEndTime);
                Assert.AreEqual(operation.UserID, savedItem.UserID);

                store.DeleteSyncOperation(operation.SyncOperationIDGuid);
                savedItem = store.GetSyncOperation(operation.SyncOperationIDGuid);
                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestUser();
            }
        }

         [Test]
         public void BranchTest()
         {
             var store = CreateStore();
             var item = new Branch();

             item.Address = "Address";
             item.Title = "Title";
             item.LegalName = "LegalName";

             store.SaveBranch(item);

             var savedItem = store.GetBranch(item.BranchIDGuid);

             Assert.IsNotNull(savedItem);
             Assert.AreEqual(item.Address, savedItem.Address);
             Assert.AreEqual(item.BranchID, savedItem.BranchID);
             Assert.AreEqual(item.Title, savedItem.Title);
             Assert.AreEqual(item.LegalName, savedItem.LegalName);

             store.DeleteBranch(item.BranchIDGuid);

             savedItem = store.GetBranch(item.BranchIDGuid);

             Assert.IsNull(savedItem);
         }

         private readonly Guid _testBranchID = new Guid("3E7E3663-972B-4517-A234-27F1EE74DB81");

         private Branch CreateTestBranch()
         {
             var item = new Branch();
             var store = CreateStore();
             item.BranchIDGuid = _testBranchID;
             item.Address = "Address";
             item.Title = "Title";
             item.LegalName = "LegalName";
             store.SaveBranch(item);
             return item;
         }

         private void DeleteTestBranch()
         {
             var store = CreateStore();
             store.DeleteBranch(_testBranchID);
         }

         [Test]
         public void UserBranchMapItemTest()
         {
             var branch = CreateTestBranch();
             var user = CreateTestUser();

             try
             {
                 var item = new UserBranchMapItem();
                 item.EventDateDateTime = new DateTime(2015, 01, 01);
                 item.BranchID = branch.BranchID;
                 item.UserID = user.UserID;

                 var store = CreateStore();

                 store.SaveUserBranchMapItem(item);

                 var savedItem = store.GetUserBranchMapItem(item.UserBranchMapIDGuid);
                 Assert.IsNotNull(savedItem);
                 Assert.AreEqual(item.UserBranchMapID, savedItem.UserBranchMapID);
                 Assert.AreEqual(item.UserID, savedItem.UserID);
                 Assert.AreEqual(item.EventDate, savedItem.EventDate);
                 Assert.AreEqual(item.BranchID, savedItem.BranchID);

                 store.DeleteUserBranchMapItem(item.UserBranchMapIDGuid);

                 savedItem = store.GetUserBranchMapItem(item.UserBranchMapIDGuid);
                 Assert.IsNull(savedItem);
             }
             finally
             {
                 DeleteTestBranch();
                 DeleteTestUser();
             }
         }

         private readonly Guid _financialGroupItemId = new Guid("4B88797B-6878-4786-B247-215F623DF68E");

         private FinancialGroupItem CreateTestFinancialGroupItem()
         {
             
             var item = new FinancialGroupItem();
             item.LegalName = "LegalName";
             item.Trademark = "Trademark";
             item.Title = "Title";
             
             item.FinancialGroupIDGuid = _financialGroupItemId;

             var store = CreateStore();

             store.SaveFinancialGroupItem(item);

             return item;
         }

         private void DeleteTestFinancialGroupItem()
         {
             var store = CreateStore();
             store.DeleteFinancialGroupItem(_financialGroupItemId);
         }

         [Test]
         public void FinancialGroupItemTest()
         {
             var item = new FinancialGroupItem();
             item.LegalName = "LegalName";
             item.Trademark = "Trademark";
             item.Title = "Title";
                 

             var store = CreateStore();

             store.SaveFinancialGroupItem(item);

             var savedItem = store.GetFinancialGroupItem(item.FinancialGroupIDGuid);

             Assert.IsNotNull(savedItem);

             Assert.AreEqual(savedItem.FinancialGroupID, item.FinancialGroupID);
             Assert.AreEqual(savedItem.LegalName, item.LegalName);
             Assert.AreEqual(savedItem.Title, item.Title);
             Assert.AreEqual(savedItem.Trademark, item.Trademark);

             item.LegalName = "LegalName2";
             item.Trademark = "Tradem3ark";
             item.Title = "Titl4e";

             store.SaveFinancialGroupItem(item);

             savedItem = store.GetFinancialGroupItem(item.FinancialGroupIDGuid);

             Assert.IsNotNull(savedItem);

             Assert.AreEqual(savedItem.FinancialGroupID, item.FinancialGroupID);
             Assert.AreEqual(savedItem.LegalName, item.LegalName);
             Assert.AreEqual(savedItem.Title, item.Title);
             Assert.AreEqual(savedItem.Trademark, item.Trademark);

             store.DeleteFinancialGroupItem(item.FinancialGroupIDGuid);

             savedItem = store.GetFinancialGroupItem(item.FinancialGroupIDGuid);

             Assert.IsNull(savedItem);
         }

         [Test]
         public void FinancialGroupBranchMapItemTest()
         {
             var finGroup = CreateTestFinancialGroupItem();
             var branch = CreateTestBranch();
             try
             {
                 var item = new FinancialGroupBranchMapItem();
                 item.BranchID = branch.BranchID;
                 item.FinancialGroupID = finGroup.FinancialGroupID;

                 var store = CreateStore();
                 store.SaveFinancialGroupMapBranchItem(item);

                 var savedItem = store.GetFinancialGroupMapBranchItem(item.FinancialGroupBranchMapIDGuid);
                 Assert.IsNotNull(savedItem);

                 store.DeleteFinancialGroupBranchMapItem(item.FinancialGroupBranchMapIDGuid);
                 savedItem = store.GetFinancialGroupMapBranchItem(item.FinancialGroupBranchMapIDGuid);
                 Assert.IsNull(savedItem);
             }
             finally
             {
                 DeleteTestBranch();
                 DeleteTestFinancialGroupItem();
             }
         }

         private readonly Guid _warehouseId = new Guid("126208C7-53C0-4BA4-9C87-CD8214842B37");

         private Warehouse CreateTestWarehouse()
         {
             var item = new Warehouse();
             item.Title = "Title";
             item.WarehouseIDGuid = _warehouseId;

             var store = CreateStore();
             store.SaveWarehouse(item);
             return item;
         }

         private void DeleteTestWarehouse()
         {
             var store = CreateStore();
             store.DeleteWarehouse(_warehouseId);
         }

         [Test]
         public void WarehouseTest()
         {
             var item = new Warehouse();
             item.Title = "Title";
                 
             var store = CreateStore();
             store.SaveWarehouse(item);

             var savedItem = store.GetWarehouse(item.WarehouseIDGuid);
             Assert.IsNotNull(savedItem);
             Assert.AreEqual(savedItem.Title, item.Title);
             item.Title = "Title22";
             store.SaveWarehouse(item);

             savedItem = store.GetWarehouse(item.WarehouseIDGuid);
             Assert.IsNotNull(savedItem);
             Assert.AreEqual(savedItem.Title, item.Title);

             store.DeleteWarehouse(item.WarehouseIDGuid);
             savedItem = store.GetWarehouse(item.WarehouseIDGuid);
             Assert.IsNull(savedItem);
         }

         [Test]
         public void FinancialGroupWarehouseMapItemTest()
         {
             var finGroup = CreateTestFinancialGroupItem();
             var warehouse = CreateTestWarehouse();
             try
             {
                 var item = new FinancialGroupWarehouseMapItem();
                 item.WarehouseID = warehouse.WarehouseID;
                 item.FinancialGroupID = finGroup.FinancialGroupID;

                 var store = CreateStore();
                 store.SaveFinancialGroupWarehouseItem(item);

                 var savedItem = store.GetFinancialGroupMapWarehouseItem(item.FinancialGroupWarehouseMapIDGuid);
                 Assert.IsNotNull(savedItem);

                 store.DeleteFinancialGroupWarehouseMapItem(item.FinancialGroupWarehouseMapIDGuid);
                 savedItem = store.GetFinancialGroupMapWarehouseItem(item.FinancialGroupWarehouseMapIDGuid);
                 Assert.IsNull(savedItem);
             }
             finally
             {
                 DeleteTestWarehouse();
                 DeleteTestFinancialGroupItem();
             }
         }

         private readonly Guid _itemCategoryId = new Guid("5D5DD943-1648-4410-9C68-8091B62AE34E");

         private ItemCategory CreateTestItemCategory()
         {
             var item = new ItemCategory();
             item.Title = "Title";
             item.ItemCategoryIDGuid = _itemCategoryId;

             var store = CreateStore();
             store.SaveItemCategory(item);
             return item;
         }

         private void DeleteTestItemCategory()
         {
             var store = CreateStore();
             store.DeleteItemCategory(_itemCategoryId);
         }

         [Test]
         public void ItemCategoryTest()
         {
             var item = new ItemCategory();
             item.Title = "Title";
                 
             var store = CreateStore();
             store.SaveItemCategory(item);

             var savedItem = store.GetItemCategory(item.ItemCategoryIDGuid);
             Assert.IsNotNull(savedItem);
             Assert.AreEqual(savedItem.Title, item.Title);
             item.Title = "Title22";
             store.SaveItemCategory(item);

             savedItem = store.GetItemCategory(item.ItemCategoryIDGuid);
             Assert.IsNotNull(savedItem);
             Assert.AreEqual(savedItem.Title, item.Title);

             store.DeleteItemCategory(item.ItemCategoryIDGuid);
             savedItem = store.GetItemCategory(item.ItemCategoryIDGuid);
             Assert.IsNull(savedItem);
         }


         private readonly Guid _goodsItemId = new Guid("999AD6A9-6E81-4089-BF3A-C71B2428D535");

         private GoodsItem CreateTestGoodsItem()
         {
             var category = CreateTestItemCategory();
             var item = new GoodsItem();
             item.GoodsItemIDGuid = _goodsItemId;
             item.Title = "Title";
             item.Particular = "Particular";
             item.ItemCategoryID = category.ItemCategoryID;
             item.BarCode = "BarCode";
             item.Description = "Description";
             item.UserCode = "UserCode";
             item.DimensionKindID = DimensionKindSet.Thing.DimensionKindID;

             var store = CreateStore();
             store.SaveGoodsItem(item);
             return item;
         }

         private void DeleteTestGoodsItem()
         {
             var store = CreateStore();
             store.DeleteGoodsItem(_goodsItemId);
             DeleteTestItemCategory();
         }

         [Test]
         public void GoodsItemTest()
         {
             var category = CreateTestItemCategory();
             try
             {
                 var item = new GoodsItem();
                 item.Title = "Title";
                 item.Particular = "Particular";
                 item.ItemCategoryID = category.ItemCategoryID;
                 item.BarCode = "BarCode";
                 item.Description = "Description";
                 item.UserCode = "UserCode";
                 item.DimensionKindID = DimensionKindSet.Thing.DimensionKindID;

                 var store = CreateStore();
                 store.SaveGoodsItem(item);

                 var savedItem = store.GetGoodsItem(item.GoodsItemIDGuid);
                 Assert.IsNotNull(savedItem);
                 Assert.AreEqual(savedItem.Title, item.Title);
                 Assert.AreEqual(savedItem.Particular, item.Particular);
                 Assert.AreEqual(savedItem.ItemCategoryID, item.ItemCategoryID);
                 Assert.AreEqual(savedItem.BarCode, item.BarCode);
                 Assert.AreEqual(savedItem.DimensionKindID, item.DimensionKindID);
                 Assert.AreEqual(savedItem.Description, item.Description);
                 Assert.AreEqual(savedItem.UserCode, item.UserCode);
                 item.Title = "Title2";
                 item.Particular = "Particular3";
                 item.BarCode = "BarCode4";
                 item.Description = "Description5";
                 item.UserCode = "UserCode2";
                 store.SaveGoodsItem(item);

                 savedItem = store.GetGoodsItem(item.GoodsItemIDGuid);
                 Assert.IsNotNull(savedItem);
                 Assert.AreEqual(savedItem.Title, item.Title);
                 Assert.AreEqual(savedItem.Particular, item.Particular);
                 Assert.AreEqual(savedItem.ItemCategoryID, item.ItemCategoryID);
                 Assert.AreEqual(savedItem.BarCode, item.BarCode);
                 Assert.AreEqual(savedItem.Description, item.Description);
                 Assert.AreEqual(savedItem.UserCode, item.UserCode);

                 store.DeleteGoodsItem(item.GoodsItemIDGuid);
                 savedItem = store.GetGoodsItem(item.GoodsItemIDGuid);
                 Assert.IsNull(savedItem);

             }
             finally
             {
                 DeleteTestItemCategory();
             }
         }

         private readonly Guid _warehouseItemId = new Guid("C2D4DC21-9488-4666-BABE-378627408902");

         private WarehouseItem CreateTestWarehouseItem()
         {
             var warehouse = CreateTestWarehouse();
             var goodsItem = CreateTestGoodsItem();
             var item = new WarehouseItem();
             item.GoodsItemID = goodsItem.GoodsItemID;
             item.RepairPrice = 22.56;
             item.SalePrice = 663.44;
             item.StartPrice = 354.0;
             item.Total = 33.2;
             item.WarehouseID = warehouse.WarehouseID;
             item.WarehouseItemIDGuid = _warehouseItemId;

             var store = CreateStore();
             store.SaveWarehouseItem(item);
             return item;
         }

         private void DeleteTestWarehouseItem()
         {
             var store = CreateStore();
             store.DeleteWarehouseItem(_warehouseItemId);
             DeleteTestWarehouse();
             DeleteTestGoodsItem();
         }

         [Test]
         public void WarehouseItemTest()
         {
             var warehouse = CreateTestWarehouse();
             var goodsItem = CreateTestGoodsItem();
             try
             {
                 var item = new WarehouseItem();
                 item.GoodsItemID = goodsItem.GoodsItemID;
                 item.RepairPrice = 22.56;
                 item.SalePrice = 663.44;
                 item.StartPrice = 354.0;
                 item.Total = 33.2;
                 item.WarehouseID = warehouse.WarehouseID;

                 var store = CreateStore();
                 store.SaveWarehouseItem(item);

                 var savedItem = store.GetWarehouseItem(item.WarehouseItemIDGuid);
                 Assert.IsNotNull(savedItem);
                 Assert.AreEqual(savedItem.GoodsItemID, item.GoodsItemID);
                 Assert.AreEqual(savedItem.SalePrice, item.SalePrice);
                 Assert.AreEqual(savedItem.RepairPrice, item.RepairPrice);
                 Assert.AreEqual(savedItem.StartPrice, item.StartPrice);
                 Assert.AreEqual(savedItem.Total, item.Total);
                 Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);

                 item.RepairPrice = 122.56;
                 item.SalePrice = 1663.44;
                 item.StartPrice = 1354.0;
                 item.Total = 333.2;
                 store.SaveWarehouseItem(item);

                 savedItem = store.GetWarehouseItem(item.WarehouseItemIDGuid);
                 Assert.IsNotNull(savedItem);
                 Assert.AreEqual(savedItem.GoodsItemID, item.GoodsItemID);
                 Assert.AreEqual(savedItem.SalePrice, item.SalePrice);
                 Assert.AreEqual(savedItem.RepairPrice, item.RepairPrice);
                 Assert.AreEqual(savedItem.StartPrice, item.StartPrice);
                 Assert.AreEqual(savedItem.Total, item.Total);
                 Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);

                 store.DeleteWarehouseItem(item.WarehouseItemIDGuid);
                 savedItem = store.GetWarehouseItem(item.WarehouseItemIDGuid);
                 Assert.IsNull(savedItem);

             }
             finally
             {
                 DeleteTestWarehouse();
                 DeleteTestGoodsItem();
             }
         }

         [Test]
         public void OrderKindTest()
         {
             var store = CreateStore();

             var item = new OrderKind();
             item.Title = "Title";
             store.SaveOrderKind(item);

             var savedItem = store.GetOrderKind(item.OrderKindIDGuid);
             Assert.IsNotNull(savedItem);

             Assert.AreEqual(item.Title, savedItem.Title);

             store.DeleteOrderKind(savedItem.OrderKindIDGuid);
             savedItem = store.GetOrderKind(item.OrderKindIDGuid);
             Assert.IsNull(savedItem);
         }

         [Test]
         public void OrderStatusTest()
         {
             var store = CreateStore();

             var item = new OrderStatus();
             item.Title = "Title";
             item.StatusKindID = StatusKindSet.Completed.StatusKindID;
             store.SaveOrderStatus(item);

             var savedItem = store.GetOrderStatus(item.OrderStatusIDGuid);
             Assert.IsNotNull(savedItem);

             Assert.AreEqual(item.Title, savedItem.Title);
             Assert.AreEqual(item.StatusKindID, savedItem.StatusKindID);

             store.DeleteOrderStatus(savedItem.OrderStatusIDGuid);
             savedItem = store.GetOrderStatus(item.OrderStatusIDGuid);
             Assert.IsNull(savedItem);
         }

         [Test]
         public void RepairOrderTest()
         {
             var store = CreateStore();

             var orderStatus = new OrderStatus();
             orderStatus.Title = "Title";
             orderStatus.StatusKindID = StatusKindSet.Completed.StatusKindID;
             store.SaveOrderStatus(orderStatus);

             var item = new OrderKind();
             item.Title = "Title";
             store.SaveOrderKind(item);

             var branch = CreateTestBranch();
             var user = CreateTestUser();

             try
             {
                 var order = new RepairOrder();
                 order.BranchID = branch.BranchID;
                 order.CallEventDateDateTime = new DateTime(2015, 06, 01);
                 order.ClientAddress = "ClientAddress";
                 order.ClientEmail = "ClientEmail";
                 order.ClientFullName = "ClientFullName";
                 order.ClientPhone = "ClientPhone";
                 order.DateOfBeReadyDateTime = new DateTime(2015, 07, 07);
                 order.Defect = "Defect";
                 order.DeviceAppearance = "DeviceAppearance";
                 order.DeviceModel = "DeviceModel";
                 order.DeviceSN = "DeviceSN";
                 order.DeviceTitle = "DeviceTitle";
                 order.DeviceTrademark = "DeviceTrademark";
                 order.EngineerID = user.UserID;
                 order.EventDateDateTime = new DateTime(2014, 02, 05);
                 order.GuidePrice = 44;
                 order.IsUrgentBoolean = true;
                 order.IssueDateDateTime = new DateTime(2013, 05, 04);
                 order.IssuerID = user.UserID;
                 order.ManagerID = user.UserID;
                 order.Notes = "Notes";
                 order.Number = "Number" + Guid.NewGuid();
                 order.Options = "Options";
                 order.OrderKindID = item.OrderKindID;
                 order.OrderStatusID = orderStatus.OrderStatusID;
                 order.PrePayment = 55;
                 order.Recommendation = "Recommendation";
                 order.WarrantyToDateTime = new DateTime(2017, 01, 2);

                 store.SaveRepairOrder(order);

                 var savedItem = store.GetRepairOrder(order.RepairOrderIDGuid);

                 Assert.IsNotNull(savedItem);

                 Assert.AreEqual(order.BranchID, savedItem.BranchID);
                 Assert.AreEqual(order.CallEventDate, savedItem.CallEventDate);
                 Assert.AreEqual(order.ClientAddress, savedItem.ClientAddress);
                 Assert.AreEqual(order.ClientEmail, savedItem.ClientEmail);
                 Assert.AreEqual(order.ClientFullName, savedItem.ClientFullName);
                 Assert.AreEqual(order.ClientPhone, savedItem.ClientPhone);
                 Assert.AreEqual(order.DateOfBeReady, savedItem.DateOfBeReady);
                 Assert.AreEqual(order.Defect, savedItem.Defect);
                 Assert.AreEqual(order.DeviceAppearance, savedItem.DeviceAppearance);
                 Assert.AreEqual(order.DeviceModel, savedItem.DeviceModel);
                 Assert.AreEqual(order.DeviceSN, savedItem.DeviceSN);
                 Assert.AreEqual(order.DeviceTitle, savedItem.DeviceTitle);
                 Assert.AreEqual(order.DeviceTrademark, savedItem.DeviceTrademark);
                 Assert.AreEqual(order.EngineerID, savedItem.EngineerID);
                 Assert.AreEqual(order.EventDate, savedItem.EventDate);
                 Assert.AreEqual(order.GuidePrice, savedItem.GuidePrice);
                 Assert.AreEqual(order.IsUrgent, savedItem.IsUrgent);
                 Assert.AreEqual(order.IssueDate, savedItem.IssueDate);
                 Assert.AreEqual(order.IssuerID, savedItem.IssuerID);
                 Assert.AreEqual(order.ManagerID, savedItem.ManagerID);
                 Assert.AreEqual(order.Notes, savedItem.Notes);
                 Assert.AreEqual(order.Number, savedItem.Number);
                 Assert.AreEqual(order.Options, savedItem.Options);
                 Assert.AreEqual(order.OrderKindID, savedItem.OrderKindID);
                 Assert.AreEqual(order.OrderStatusID, savedItem.OrderStatusID);
                 Assert.AreEqual(order.PrePayment, savedItem.PrePayment);
                 Assert.AreEqual(order.Recommendation, savedItem.Recommendation);
                 Assert.AreEqual(order.WarrantyTo, savedItem.WarrantyTo);

                 store.DeleteRepairOrder(order.RepairOrderIDGuid);

                 savedItem = store.GetRepairOrder(order.RepairOrderIDGuid);

                 Assert.IsNull(savedItem);
             }
             finally
             {
                 store.DeleteOrderStatus(orderStatus.OrderStatusIDGuid);
                 store.DeleteOrderKind(item.OrderKindIDGuid);
                 DeleteTestBranch();
                 DeleteTestUser();
             }

         }

         private readonly Guid _repairOrderId = new Guid("6ECA4833-4906-4235-A37F-EF8077867CD4");

         public RepairOrder CreateTestRepairOrder()
         {
             var store = CreateStore();
             var orderStatus = new OrderStatus();
             orderStatus.Title = "Title";
             orderStatus.StatusKindID = StatusKindSet.Completed.StatusKindID;

             store.SaveOrderStatus(orderStatus);

             var item = new OrderKind();
             item.Title = "Title";
             store.SaveOrderKind(item);

             var branch = CreateTestBranch();
             var user = CreateTestUser();

             var order = new RepairOrder();
             order.BranchID = branch.BranchID;
             order.CallEventDateDateTime = new DateTime(2015, 06, 01);
             order.ClientAddress = "ClientAddress33323";
             order.ClientEmail = "ClientEmail";
             order.ClientFullName = "ClientFullName";
             order.ClientPhone = "ClientPhone";
             order.DateOfBeReadyDateTime = new DateTime(2015, 07, 07);
             order.Defect = "Defect";
             order.DeviceAppearance = "DeviceAppearance";
             order.DeviceModel = "DeviceModel";
             order.DeviceSN = "DeviceSN";
             order.DeviceTitle = "DeviceTitle";
             order.DeviceTrademark = "DeviceTrademark";
             order.EngineerID = user.UserID;
             order.EventDateDateTime = new DateTime(2014, 02, 05);
             order.GuidePrice = 44;
             order.IsUrgentBoolean = true;
             order.IssueDateDateTime = new DateTime(2013, 05, 04);
             order.IssuerID = user.UserID;
             order.ManagerID = user.UserID;
             order.Notes = "Notes";
             order.Number = "Number" + Guid.NewGuid();
             order.Options = "Options";
             order.OrderKindID = item.OrderKindID;
             order.OrderStatusID = orderStatus.OrderStatusID;
             order.PrePayment = 55;
             order.Recommendation = "Recommendation";
             order.WarrantyToDateTime = new DateTime(2017, 01, 2);
             order.RepairOrderIDGuid = _repairOrderId;
             store.SaveRepairOrder(order);

             return order;
         }

         private void DeleteTestRepairOrder()
         {
             var store = CreateStore();

             var savedItem = store.GetRepairOrder(_repairOrderId);
             var orderStatusId = savedItem.OrderStatusIDGuid;
             var orderKindId = savedItem.OrderKindIDGuid;

             store.DeleteRepairOrder(_repairOrderId);

             DeleteTestBranch();
             store.DeleteOrderKind(orderKindId);
             store.DeleteOrderStatus(orderStatusId);
             DeleteTestUser();

         }


         [Test]
         public void DeviceItemTest()
         {
             var store = CreateStore();
             var order = CreateTestRepairOrder();
             var warehouseItem = CreateTestWarehouseItem();
             try
             {
                 var item = new DeviceItem();
                 item.CostPrice = 22.3;
                 item.Count = 22;
                 item.Price = 56;
                 item.Title = "Title";
                 item.RepairOrderID = order.RepairOrderID;
                 item.EventDateDateTime = new DateTime(2015, 06, 04);
                 item.UserID = order.ManagerID;

                 store.SaveDeviceItem(item);

                 var savedItem = store.GetDeviceItem(item.DeviceItemIDGuid);

                 Assert.IsNotNull(savedItem);

                 Assert.AreEqual(item.CostPrice, savedItem.CostPrice);
                 Assert.AreEqual(item.Count, savedItem.Count);
                 Assert.AreEqual(item.Price, savedItem.Price);
                 Assert.AreEqual(item.Title, savedItem.Title);
                 Assert.AreEqual(item.UserID, savedItem.UserID);
                 Assert.AreEqual(item.EventDate, savedItem.EventDate);
                 Assert.AreEqual(item.WarehouseItemID, savedItem.WarehouseItemID);

                 item.CostPrice = 25.3;
                 item.Count = 12;
                 item.Price = 54;
                 item.Title = "Title2";
                 item.WarehouseItemID = warehouseItem.WarehouseItemID;

                 store.SaveDeviceItem(item);

                 savedItem = store.GetDeviceItem(item.DeviceItemIDGuid);

                 Assert.IsNotNull(savedItem);

                 Assert.AreEqual(item.CostPrice, savedItem.CostPrice);
                 Assert.AreEqual(item.Count, savedItem.Count);
                 Assert.AreEqual(item.Price, savedItem.Price);
                 Assert.AreEqual(item.Title, savedItem.Title);
                 Assert.AreEqual(item.UserID, savedItem.UserID);
                 Assert.AreEqual(item.EventDate, savedItem.EventDate);
                 Assert.AreEqual(item.WarehouseItemID, savedItem.WarehouseItemID);

                 store.DeleteDeviceItem(item.DeviceItemIDGuid);

                 savedItem = store.GetDeviceItem(item.DeviceItemIDGuid);

                 Assert.IsNull(savedItem);
             }
             finally
             {
                 DeleteTestWarehouseItem();
                 DeleteTestRepairOrder();
             }
         }


         [Test]
         public void WorkItemTest()
         {
             var store = CreateStore();
             var order = CreateTestRepairOrder();

             try
             {
                 var item = new WorkItem();
                 item.EventDateDateTime = new DateTime(2015, 06, 01);
                 item.Price = 10;
                 item.Title = "Title";
                 item.UserID = order.IssuerID;
                 item.RepairOrderID = order.RepairOrderID;

                 store.SaveWorkItem(item);

                 var savedItem = store.GetWorkItem(item.WorkItemIDGuid);

                 Assert.IsNotNull(savedItem);

                 Assert.AreEqual(item.EventDate, savedItem.EventDate);
                 Assert.AreEqual(item.Price, savedItem.Price);
                 Assert.AreEqual(item.Title, savedItem.Title);
                 Assert.AreEqual(item.UserID, savedItem.UserID);

                 store.DeleteWorkItem(item.WorkItemIDGuid);
                 savedItem = store.GetWorkItem(item.WorkItemIDGuid);

                 Assert.IsNull(savedItem);
             }
             finally
             {
                 DeleteTestRepairOrder();

             }
         }

         [Test]
         public void OrderTimelineTest()
         {
             var store = CreateStore();
             var order = CreateTestRepairOrder();

             try
             {
                 var item = new OrderTimeline();
                 item.EventDateTimeDateTime = new DateTime(2016, 03, 01);
                 item.RepairOrderID = order.RepairOrderID;
                 item.TimelineKindID = TimelineKindSet.Completed.TimelineKindID;
                 item.Title = "Title";

                 store.SaveOrderTimeline(item);
                 var savedItem = store.GetOrderTimeline(item.OrderTimelineIDGuid);

                 Assert.IsNotNull(savedItem);

                 Assert.AreEqual(item.EventDateTime, savedItem.EventDateTime);
                 Assert.AreEqual(item.TimelineKindID, savedItem.TimelineKindID);
                 Assert.AreEqual(item.Title, savedItem.Title);

                 store.DeleteOrderTimeline(item.OrderTimelineIDGuid);
                 savedItem = store.GetOrderTimeline(item.OrderTimelineIDGuid);

                 Assert.IsNull(savedItem);
             }
             finally
             {
                 DeleteTestRepairOrder();
             }
         }

         [Test]
         public void RepairOrderServerHashItemTest()
         {
             var store = CreateStore();

             var item = new RepairOrderServerHashItem();
             item.DataHash = "DataHash";
             item.OrderTimelinesCount = 11;

             store.SaveRepairOrderServerHashItem(item);
             var savedItem = store.GetRepairOrderServerHashItem(item.RepairOrderServerHashIDGuid);

             Assert.IsNotNull(savedItem);

             Assert.AreEqual(item.OrderTimelinesCount, savedItem.OrderTimelinesCount);
             Assert.AreEqual(item.DataHash, savedItem.DataHash);
             Assert.AreEqual(item.RepairOrderServerHashID, savedItem.RepairOrderServerHashID);

             store.DeleteRepairOrderServerHashItem(item.RepairOrderServerHashIDGuid);
             savedItem = store.GetRepairOrderServerHashItem(item.RepairOrderServerHashIDGuid);

             Assert.IsNull(savedItem);
         }

         [Test]
         public void WorkItemServerHashItemTest()
         {
             var store = CreateStore();

             var item = new WorkItemServerHashItem();
             item.DataHash = "DataHash";
             item.RepairOrderServerHashIDGuid = Guid.NewGuid();

             store.SaveWorkItemServerHashItem(item);
             var savedItem = store.GetWorkItemServerHashItem(item.WorkItemServerHashIDGuid);

             Assert.IsNotNull(savedItem);

             Assert.AreEqual(item.RepairOrderServerHashIDGuid, savedItem.RepairOrderServerHashIDGuid);
             Assert.AreEqual(item.DataHash, savedItem.DataHash);
             Assert.AreEqual(item.WorkItemServerHashID, savedItem.WorkItemServerHashID);

             store.DeleteWorkItemServerHashItem(item.WorkItemServerHashIDGuid);
             savedItem = store.GetWorkItemServerHashItem(item.WorkItemServerHashIDGuid);

             Assert.IsNull(savedItem);
         }

         [Test]
         public void DeviceItemServerHashItemTest()
         {
             var store = CreateStore();

             var item = new DeviceItemServerHashItem();
             item.DataHash = "DataHash";
             item.RepairOrderServerHashIDGuid = Guid.NewGuid();

             store.SaveDeviceItemServerHashItem(item);
             var savedItem = store.GetDeviceItemServerHashItem(item.DeviceItemServerHashIDGuid);

             Assert.IsNotNull(savedItem);

             Assert.AreEqual(item.RepairOrderServerHashIDGuid, savedItem.RepairOrderServerHashIDGuid);
             Assert.AreEqual(item.DataHash, savedItem.DataHash);
             Assert.AreEqual(item.DeviceItemServerHashID, savedItem.DeviceItemServerHashID);

             store.DeleteDeviceItemServerHashItem(item.DeviceItemServerHashIDGuid);
             savedItem = store.GetDeviceItemServerHashItem(item.DeviceItemServerHashIDGuid);

             Assert.IsNull(savedItem);
         }

         [Test]
         public void CustomReportTest()
         {
             var store = CreateStore();
             var entity = new CustomReportItem();
             entity.DocumentKindID = DocumentKindSet.OrderReportDocument.DocumentKindID;
             entity.Title = "Title";
             entity.HtmlContent = "HtmlContent";

             store.SaveCustomReportItem(entity);

             var savedItem = store.GetCustomReportItem(entity.CustomReportIDGuid);

             Assert.IsNotNull(savedItem);

             Assert.AreEqual(entity.DocumentKindID, savedItem.DocumentKindID);
             Assert.AreEqual(entity.HtmlContent, savedItem.HtmlContent);
             Assert.AreEqual(entity.Title, savedItem.Title);

             store.DeleteCustomReportItem(entity.CustomReportIDGuid);

             savedItem = store.GetCustomReportItem(entity.CustomReportIDGuid);

             Assert.IsNull(savedItem);
         }

    }
}
