using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Remontinka.Server.Crypto;
using Romontinka.Server.Protocol;
using Romontinka.Server.Protocol.AuthMessages;
using Romontinka.Server.Protocol.SynchronizationMessages;

namespace Romontinka.Test
{
    [TestFixture]
    public class ProtocolTest
    {
        [Test]
        public void RegisterPublicKeyRequestTest()
        {
            var message = new RegisterPublicKeyRequest();
            Assert.AreEqual(message.Kind, MessageKind.RegisterPublicKeyRequest);

            message.KeyNotes = "KeyNotes";
            message.UserLogin = "UserLogin";
            message.PublicKeyData = "PublicKeyData";
            message.ClientUserDomainID = Guid.NewGuid();
            message.EventDate = new DateTime(2015,01,15);

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.RegisterPublicKeyRequest);

            var savedMessage = serializer.DeserializeRegisterPublicKeyRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind,savedMessage.Kind);
            Assert.AreEqual(message.EventDate, savedMessage.EventDate);
            Assert.AreEqual(message.Version, savedMessage.Version);
            Assert.AreEqual(message.KeyNotes, savedMessage.KeyNotes);
            Assert.AreEqual(message.PublicKeyData, savedMessage.PublicKeyData);
            Assert.AreEqual(message.UserLogin, savedMessage.UserLogin);
            Assert.AreEqual(message.ClientUserDomainID, savedMessage.ClientUserDomainID);

            message.ClientUserDomainID = null;

            data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.RegisterPublicKeyRequest);

            savedMessage = serializer.DeserializeRegisterPublicKeyRequest(data);
            Assert.AreEqual(message.ClientUserDomainID, savedMessage.ClientUserDomainID);
        }

        [Test]
        public void ErrorResponseTest()
        {
            var message = new ErrorResponse();
            Assert.AreEqual(message.Kind, MessageKind.ErrorResponse);

            message.Description = "Description";

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.ErrorResponse);

            var savedMessage = serializer.DeserializeErrorResponse(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.Version, savedMessage.Version);
            Assert.AreEqual(message.Description, savedMessage.Description);

        }

        [Test]
        public void AuthErrorResponseTest()
        {
            var message = new AuthErrorResponse();
            Assert.AreEqual(message.Kind, MessageKind.AuthErrorResponse);

            message.Message = "AuthErrorResponse";

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.AuthErrorResponse);

            var savedMessage = serializer.DeserializeAuthErrorResponse(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.Version, savedMessage.Version);
            Assert.AreEqual(message.Message, savedMessage.Message);

        }

        [Test]
        public void RegisterPublicKeyResponseTest()
        {
            var message = new RegisterPublicKeyResponse();
            Assert.AreEqual(message.Kind, MessageKind.RegisterPublicKeyResponse);

            message.Number = "Number";
            message.UserDomainID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.RegisterPublicKeyResponse);

            var savedMessage = serializer.DeserializeRegisterPublicKeyResponse(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.Version, savedMessage.Version);
            Assert.AreEqual(message.Number, savedMessage.Number);
            Assert.AreEqual(message.UserDomainID, savedMessage.UserDomainID);

        }

        [Test]
        public void ProbeKeyActivationRequestTest()
        {
            var message = new ProbeKeyActivationRequest();
            Assert.AreEqual(message.Kind, MessageKind.ProbeKeyActivationRequest);

            message.KeyNumber = "KeyNumber";
            message.UserDomainID = Guid.Empty;
            

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.ProbeKeyActivationRequest);

            var savedMessage = serializer.DeserializeProbeKeyActivationRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.KeyNumber, savedMessage.KeyNumber);
            Assert.AreEqual(message.UserDomainID, savedMessage.UserDomainID);
           
        }

        [Test]
        public void ProbeKeyActivationResponseTest()
        {
            var message = new ProbeKeyActivationResponse();
            Assert.AreEqual(message.Kind, MessageKind.ProbeKeyActivationResponse);

            message.IsExists = true;
            message.IsExpired = true;
            message.IsNotAccepted = true;
            message.IsRevoked = true;

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.ProbeKeyActivationResponse);

            var savedMessage = serializer.DeserializeProbeKeyActivationResponse(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.IsExists, savedMessage.IsExists);
            Assert.AreEqual(message.IsExpired, savedMessage.IsExpired);
            Assert.AreEqual(message.IsNotAccepted, savedMessage.IsNotAccepted);
            Assert.AreEqual(message.IsRevoked, savedMessage.IsRevoked);

        }

        [Test]
        public void GetDomainUsersRequestTest()
        {
            var message = new GetDomainUsersRequest();
            Assert.AreEqual(message.Kind, MessageKind.GetDomainUsersRequest);

            message.UserID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetDomainUsersRequest);

            var savedMessage = serializer.DeserializeGetDomainUsersRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.UserID, savedMessage.UserID);
        }

        [Test]
        public void GetDomainUsersResponseTest()
        {
            var message = new GetDomainUsersResponse();
            Assert.AreEqual(message.Kind, MessageKind.GetDomainUsersResponse);
            var user1 = new DomainUserDTO();
            user1.Email = "Email";
            user1.LoginName = "LoginName";
            user1.FirstName = "FirstName";
            user1.LastName = "LastName";
            user1.MiddleName = "MiddleName";
            user1.Phone = "Phone";
            user1.ProjectRoleID = 1;
            user1.UserID = Guid.NewGuid();
            
            var user2 = new DomainUserDTO();
            user2.Email = "Email1";
            user2.LoginName = "LoginName1";
            user2.FirstName = "FirstName1";
            user2.LastName = "LastName1";
            user2.MiddleName = "MiddleName1";
            user2.Phone = "Phone1";
            user2.ProjectRoleID = 2;
            user2.UserID = Guid.NewGuid();

            message.Users.Add(user1);
            message.Users.Add(user2);

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);

            var savedMessage = serializer.DeserializeGetDomainUsersResponse(data);

            Assert.IsNotNull(savedMessage);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetDomainUsersResponse);
            Assert.AreEqual(savedMessage.Users.Count,2);

            var savedUser1 = savedMessage.Users[0];
            var savedUser2 = savedMessage.Users[1];

            Assert.AreEqual(savedUser1.Email,user1.Email);
            Assert.AreEqual(savedUser1.LoginName, user1.LoginName);
            Assert.AreEqual(savedUser1.FirstName, user1.FirstName);
            Assert.AreEqual(savedUser1.LastName, user1.LastName);
            Assert.AreEqual(savedUser1.MiddleName, user1.MiddleName);
            Assert.AreEqual(savedUser1.Phone, user1.Phone);
            Assert.AreEqual(savedUser1.ProjectRoleID, user1.ProjectRoleID);
            Assert.AreEqual(savedUser1.UserID, user1.UserID);

            Assert.AreEqual(savedUser2.Email, user2.Email);
            Assert.AreEqual(savedUser2.LoginName, user2.LoginName);
            Assert.AreEqual(savedUser2.FirstName, user2.FirstName);
            Assert.AreEqual(savedUser2.LastName, user2.LastName);
            Assert.AreEqual(savedUser2.MiddleName, user2.MiddleName);
            Assert.AreEqual(savedUser2.Phone, user2.Phone);
            Assert.AreEqual(savedUser2.ProjectRoleID, user2.ProjectRoleID);
            Assert.AreEqual(savedUser2.UserID, user2.UserID);
        }

        [Test]
        public void GetUserBranchesRequestTest()
        {
            var message = new GetUserBranchesRequest();
            Assert.AreEqual(message.Kind, MessageKind.GetUserBranchesRequest);

            message.UserID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetUserBranchesRequest);

            var savedMessage = serializer.DeserializeGetUserBranchesRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.UserID, savedMessage.UserID);
        }

        [Test]
        public void GetUserBranchesResponseTest()
        {
            var message = new GetUserBranchesResponse();
            Assert.AreEqual(message.Kind, MessageKind.GetUserBranchesResponse);
            var branch = new BranchDTO();
            branch.Address = "Address";
            branch.BranchID = Guid.NewGuid();
            branch.LegalName = "LegalName";
            branch.Title = "Title";

            var item = new UserBranchMapItemDTO();
            item.BranchID = branch.BranchID;
            item.EventDate = new DateTime(2015,06,07);
            item.UserBranchMapID = Guid.Empty;
            item.UserID = Guid.NewGuid();

            message.Branches.Add(branch);
            message.UserBranchMapItems.Add(item);

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);

            var savedMessage = serializer.DeserializeGetUserBranchesResponse(data);

            Assert.IsNotNull(savedMessage);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetUserBranchesResponse);
            Assert.AreEqual(savedMessage.Branches.Count, 1);
            Assert.AreEqual(savedMessage.UserBranchMapItems.Count, 1);

            var savedBranch = savedMessage.Branches[0];
            var savedItem = savedMessage.UserBranchMapItems[0];

            Assert.AreEqual(savedBranch.Address, savedBranch.Address);
            Assert.AreEqual(savedBranch.BranchID, savedBranch.BranchID);
            Assert.AreEqual(savedBranch.LegalName, savedBranch.LegalName);
            Assert.AreEqual(savedBranch.Title, savedBranch.Title);


            Assert.AreEqual(savedItem.BranchID, item.BranchID);
            Assert.AreEqual(savedItem.EventDate, item.EventDate);
            Assert.AreEqual(savedItem.UserBranchMapID, item.UserBranchMapID);
            Assert.AreEqual(savedItem.UserID, item.UserID);
        }

        [Test]
        public void FinancialGroupBranchesRequestTest()
        {
            var message = new GetFinancialGroupBranchesRequest();
            Assert.AreEqual(message.Kind, MessageKind.GetFinancialGroupBranchesRequest);

            message.UserID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetFinancialGroupBranchesRequest);

            var savedMessage = serializer.DeserializeGetFinancialGroupBranchesRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.UserID, savedMessage.UserID);
        }

        [Test]
        public void FinancialGroupBranchesResponseTest()
        {
            var message = new GetFinancialGroupBranchesResponse();
            Assert.AreEqual(message.Kind, MessageKind.GetFinancialGroupBranchesResponse);
            var financialGroupItem = new FinancialGroupItemDTO();
            financialGroupItem.Trademark = "Trademark";
            financialGroupItem.FinancialGroupID = Guid.NewGuid();
            financialGroupItem.LegalName = "LegalName";
            financialGroupItem.Title = "Title";

            var item = new FinancialGroupBranchMapItemDTO();
            item.FinancialGroupID = financialGroupItem.FinancialGroupID;
            item.BranchID = Guid.NewGuid();
            item.FinancialGroupBranchMapID = Guid.Empty;

            message.FinancialGroupItems.Add(financialGroupItem);
            message.FinancialGroupBranchMapItems.Add(item);

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);

            var savedMessage = serializer.DeserializeGetFinancialGroupBranchesResponse(data);

            Assert.IsNotNull(savedMessage);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetFinancialGroupBranchesResponse);
            Assert.AreEqual(savedMessage.FinancialGroupItems.Count, 1);
            Assert.AreEqual(savedMessage.FinancialGroupBranchMapItems.Count, 1);

            var savedfinancialGroupItem = savedMessage.FinancialGroupItems[0];
            var savedItem = savedMessage.FinancialGroupBranchMapItems[0];

            Assert.AreEqual(savedfinancialGroupItem.Trademark, savedfinancialGroupItem.Trademark);
            Assert.AreEqual(savedfinancialGroupItem.FinancialGroupID, savedfinancialGroupItem.FinancialGroupID);
            Assert.AreEqual(savedfinancialGroupItem.LegalName, savedfinancialGroupItem.LegalName);
            Assert.AreEqual(savedfinancialGroupItem.Title, savedfinancialGroupItem.Title);


            Assert.AreEqual(savedItem.BranchID, item.BranchID);
            Assert.AreEqual(savedItem.FinancialGroupID, item.FinancialGroupID);
            Assert.AreEqual(savedItem.FinancialGroupBranchMapID, item.FinancialGroupBranchMapID);
        }

        [Test]
        public void GetWarehousesRequestTest()
        {
            var message = new GetWarehousesRequest();
            Assert.AreEqual(message.Kind, MessageKind.GetWarehousesRequest);

            message.UserID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetWarehousesRequest);

            var savedMessage = serializer.DeserializeGetWarehousesRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.UserID, savedMessage.UserID);
        }

        [Test]
        public void GetWarehousesResponseTest()
        {
            var message = new GetWarehousesResponse();
            Assert.AreEqual(message.Kind, MessageKind.GetWarehousesResponse);
            var warehouse = new WarehouseDTO();
            warehouse.WarehouseID= Guid.NewGuid();
            warehouse.Title = "Title";

            var item = new FinancialGroupWarehouseMapItemDTO();
            item.FinancialGroupID = warehouse.WarehouseID;
            item.FinancialGroupWarehouseMapID = Guid.NewGuid();
            item.FinancialGroupID = Guid.Empty;

            message.Warehouses.Add(warehouse);
            message.MapItems.Add(item);

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);

            var savedMessage = serializer.DeserializeGetWarehousesResponse(data);

            Assert.IsNotNull(savedMessage);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetWarehousesResponse);
            Assert.AreEqual(savedMessage.Warehouses.Count, 1);
            Assert.AreEqual(savedMessage.MapItems.Count, 1);

            var savedWarehouse = savedMessage.Warehouses[0];
            var savedItem = savedMessage.MapItems[0];

            Assert.AreEqual(savedWarehouse.WarehouseID, savedWarehouse.WarehouseID);
            Assert.AreEqual(savedWarehouse.Title, savedWarehouse.Title);


            Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);
            Assert.AreEqual(savedItem.FinancialGroupID, item.FinancialGroupID);
            Assert.AreEqual(savedItem.FinancialGroupWarehouseMapID, item.FinancialGroupWarehouseMapID);
        }

        [Test]
        public void GetGoodsItemRequestTest()
        {
            var message = new GetGoodsItemRequest();
            Assert.AreEqual(message.Kind, MessageKind.GetGoodsItemRequest);

            message.UserID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetGoodsItemRequest);

            var savedMessage = serializer.DeserializeGetGoodsItemRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.UserID, savedMessage.UserID);
        }

        [Test]
        public void GetGoodsItemResponseTest()
        {
            var message = new GetGoodsItemResponse();
            Assert.AreEqual(message.Kind, MessageKind.GetGoodsItemResponse);

            var item = new ItemCategoryDTO();
            item.ItemCategoryID =  Guid.NewGuid();
            item.Title = "Title";

            var goodsItem = new GoodsItemDTO();
            goodsItem.GoodsItemID = Guid.NewGuid();
            goodsItem.Title = "Title";
            goodsItem.ItemCategoryID = item.ItemCategoryID;
            goodsItem.BarCode = "BarCode";
            goodsItem.Description = "Description";
            goodsItem.DimensionKindID = 1;
            goodsItem.Particular = "Particular";
            goodsItem.UserCode = "UserCode";
           
            message.GoodsItems.Add(goodsItem);
            message.ItemCategories.Add(item);

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);

            var savedMessage = serializer.DeserializeGetGoodsItemResponse(data);

            Assert.IsNotNull(savedMessage);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetGoodsItemResponse);
            Assert.AreEqual(savedMessage.GoodsItems.Count, 1);
            Assert.AreEqual(savedMessage.ItemCategories.Count, 1);

            var savedGoodsItems = savedMessage.GoodsItems[0];
            var savedItem = savedMessage.ItemCategories[0];

            Assert.AreEqual(goodsItem.BarCode, savedGoodsItems.BarCode);
            Assert.AreEqual(goodsItem.Title, savedGoodsItems.Title);
            Assert.AreEqual(goodsItem.Description, savedGoodsItems.Description);
            Assert.AreEqual(goodsItem.DimensionKindID, savedGoodsItems.DimensionKindID);
            Assert.AreEqual(goodsItem.GoodsItemID, savedGoodsItems.GoodsItemID);
            Assert.AreEqual(goodsItem.ItemCategoryID, savedGoodsItems.ItemCategoryID);
            Assert.AreEqual(goodsItem.Particular, savedGoodsItems.Particular);
            Assert.AreEqual(goodsItem.UserCode, savedGoodsItems.UserCode);


            Assert.AreEqual(savedItem.ItemCategoryID, item.ItemCategoryID);
            Assert.AreEqual(savedItem.Title, item.Title);
        }

        [Test]
        public void GetWarehouseItemsRequestTest()
        {
            var message = new GetWarehouseItemsRequest();
            Assert.AreEqual(message.Kind, MessageKind.GetWarehouseItemsRequest);

            message.UserID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetWarehouseItemsRequest);

            var savedMessage = serializer.DeserializeGetWarehouseItemsRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.UserID, savedMessage.UserID);
        }

        [Test]
        public void GetWarehouseItemsResponseTest()
        {
            var message = new GetWarehouseItemsResponse();
            Assert.AreEqual(message.Kind, MessageKind.GetWarehouseItemsResponse);

            var item = new WarehouseItemDTO();
            item.WarehouseItemID = Guid.NewGuid();
            item.GoodsItemID = Guid.NewGuid();
            item.WarehouseID = Guid.Empty;
            item.RepairPrice = 11.22M;
            item.SalePrice = 22.65M;
            item.StartPrice = 55.64M;
            item.Total = 10.23M;
            
            message.WarehouseItems.Add(item);

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);

            var savedMessage = serializer.DeserializeGetWarehouseItemsResponse(data);

            Assert.IsNotNull(savedMessage);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetWarehouseItemsResponse);
            Assert.AreEqual(savedMessage.WarehouseItems.Count, 1);
            
            var savedItem = savedMessage.WarehouseItems[0];
            
            Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);
            Assert.AreEqual(savedItem.WarehouseItemID, item.WarehouseItemID);
            Assert.AreEqual(savedItem.GoodsItemID, item.GoodsItemID);
            Assert.AreEqual(savedItem.RepairPrice, item.RepairPrice);
            Assert.AreEqual(savedItem.SalePrice, item.SalePrice);
            Assert.AreEqual(savedItem.StartPrice, item.StartPrice);
            Assert.AreEqual(savedItem.Total, item.Total);
        }

        [Test]
        public void GetOrderStatusesRequestTest()
        {
            var message = new GetOrderStatusesRequest();
            Assert.AreEqual(message.Kind, MessageKind.GetOrderStatusesRequest);

            message.UserID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetOrderStatusesRequest);

            var savedMessage = serializer.DeserializeGetOrderStatusesRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.UserID, savedMessage.UserID);
        }

        [Test]
        public void GetOrderStatusesResponseTest()
        {
            var message = new GetOrderStatusesResponse();
            Assert.AreEqual(message.Kind, MessageKind.GetOrderStatusesResponse);

            var item = new OrderKindDTO();
            item.OrderKindID = Guid.NewGuid();
            item.Title = "Title";

            var orderStatus = new OrderStatusDTO();
            orderStatus.OrderStatusID = Guid.NewGuid();
            orderStatus.Title = "Title";
            orderStatus.StatusKindID = 1;

            message.OrderKinds.Add(item);
            message.OrderStatuses.Add(orderStatus);

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);

            var savedMessage = serializer.DeserializeGetOrderStatusesResponse(data);

            Assert.IsNotNull(savedMessage);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetOrderStatusesResponse);
            Assert.AreEqual(savedMessage.OrderKinds.Count, 1);
            Assert.AreEqual(savedMessage.OrderStatuses.Count, 1);

            var savedOrderStatus = savedMessage.OrderStatuses[0];
            var savedOrderKind = savedMessage.OrderKinds[0];

            Assert.AreEqual(orderStatus.StatusKindID, savedOrderStatus.StatusKindID);
            Assert.AreEqual(orderStatus.Title, savedOrderStatus.Title);
            Assert.AreEqual(orderStatus.OrderStatusID, savedOrderStatus.OrderStatusID);



            Assert.AreEqual(savedOrderKind.OrderKindID, item.OrderKindID);
            Assert.AreEqual(savedOrderKind.Title, item.Title);
        }

         [Test]
        public void GetServerRepairOrderHashesRequestTest()
        {
            var message = new GetServerRepairOrderHashesRequest();
            Assert.AreEqual(message.Kind, MessageKind.GetServerRepairOrderHashesRequest);

            message.UserID = Guid.NewGuid();
            message.LastRepairOrderID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetServerRepairOrderHashesRequest);

            var savedMessage = serializer.DeserializeGetServerRepairOrderHashesRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.UserID, savedMessage.UserID);
            Assert.AreEqual(message.LastRepairOrderID, savedMessage.LastRepairOrderID);
        }

         [Test]
         public void GetServerRepairOrderHashesResponseTest()
         {
             var message = new GetServerRepairOrderHashesResponse();

             Assert.AreEqual(message.Kind, MessageKind.GetServerRepairOrderHashesResponse);


             message.TotalCount = 55;

             var order = new RepairOrderServerHashDTO();
             order.DataHash = "HashData111";
             order.OrderTimelinesCount = 112;
             order.RepairOrderID = Guid.NewGuid();

             var workItem = new WorkItemServerHashDTO();
             workItem.WorkItemID = Guid.NewGuid();
             workItem.DataHash = "Titl1231231e";

             var deviceItem = new DeviceItemServerHashDTO();
             deviceItem.DeviceItemID = Guid.NewGuid();
             deviceItem.DataHash = "Tiqsdqw1233123tle";


             order.WorkItems.Add(workItem);
             order.DeviceItems.Add(deviceItem);
             message.RepairOrderServerHashes.Add(order);

             var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
             var data = serializer.Serialize(message);
             Assert.IsNotNull(data);

             var savedMessage = serializer.DeserializeGetServerRepairOrderHashesResponse(data);

             Assert.IsNotNull(savedMessage);
             Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetServerRepairOrderHashesResponse);
             Assert.AreEqual(savedMessage.RepairOrderServerHashes.Count, 1);
             Assert.AreEqual(savedMessage.RepairOrderServerHashes[0].DeviceItems.Count, 1);
             Assert.AreEqual(savedMessage.RepairOrderServerHashes[0].WorkItems.Count, 1);
             Assert.AreEqual(savedMessage.TotalCount, message.TotalCount);

             var savedDeviceItem = savedMessage.RepairOrderServerHashes[0].DeviceItems[0];
             var savedWorkItem = savedMessage.RepairOrderServerHashes[0].WorkItems[0];
             var savedOrder = savedMessage.RepairOrderServerHashes[0];

             Assert.AreEqual(savedOrder.DataHash, order.DataHash);
             Assert.AreEqual(savedOrder.RepairOrderID, order.RepairOrderID);
             Assert.AreEqual(savedOrder.OrderTimelinesCount, order.OrderTimelinesCount);


             Assert.AreEqual(deviceItem.DeviceItemID, savedDeviceItem.DeviceItemID);
             Assert.AreEqual(deviceItem.DataHash, savedDeviceItem.DataHash);

             Assert.AreEqual(savedWorkItem.DataHash, workItem.DataHash);
             Assert.AreEqual(savedWorkItem.WorkItemID, workItem.WorkItemID);
         }

         [Test]
         public void GetRepairOrdersRequestTest()
         {
             var message = new GetRepairOrdersRequest();
             Assert.AreEqual(message.Kind, MessageKind.GetRepairOrdersRequest);

             message.UserID = Guid.NewGuid();
             message.RepairOrderIds.Add( Guid.NewGuid());
             message.RepairOrderIds.Add( Guid.NewGuid());
             message.RepairOrderIds.Add( Guid.NewGuid());

             var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
             var data = serializer.Serialize(message);
             Assert.IsNotNull(data);
             Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetRepairOrdersRequest);

             var savedMessage = serializer.DeserializeGetRepairOrdersRequest(data);

             Assert.IsNotNull(savedMessage);

             Assert.AreEqual(message.Kind, savedMessage.Kind);
             Assert.AreEqual(message.UserID, savedMessage.UserID);
             Assert.AreEqual(message.RepairOrderIds.Count, savedMessage.RepairOrderIds.Count);
             Assert.AreEqual(message.RepairOrderIds[0], savedMessage.RepairOrderIds[0]);
             Assert.AreEqual(message.RepairOrderIds[1], savedMessage.RepairOrderIds[1]);
             Assert.AreEqual(message.RepairOrderIds[2], savedMessage.RepairOrderIds[2]);
         }

         [Test]
         public void GetGetRepairOrdersResponseTest()
         {
             var message = new GetRepairOrdersResponse();

             Assert.AreEqual(message.Kind, MessageKind.GetRepairOrdersResponse);

             
             var order = new RepairOrderDTO();
              order.BranchID = Guid.NewGuid();
                 order.CallEventDate = new DateTime(2015, 06, 01);
                 order.ClientAddress = "ClientAddress";
                 order.ClientEmail = "ClientEmail";
                 order.ClientFullName = "ClientFullName";
                 order.ClientPhone = "ClientPhone";
                 order.DateOfBeReady = new DateTime(2015, 07, 07);
                 order.Defect = "Defect";
                 order.DeviceAppearance = "DeviceAppearance";
                 order.DeviceModel = "DeviceModel";
                 order.DeviceSN = "DeviceSN";
                 order.DeviceTitle = "DeviceTitle";
                 order.DeviceTrademark = "DeviceTrademark";
                 order.EngineerID = Guid.NewGuid();
                 order.EventDate = new DateTime(2014, 02, 05);
                 order.GuidePrice = 44;
                 order.IsUrgent = true;
                 order.IssueDate = new DateTime(2013, 05, 04);
                 order.IssuerID = Guid.NewGuid();
                 order.ManagerID = Guid.NewGuid();
                 order.Notes = "Notes";
                 order.Number = "Number" + Guid.NewGuid();
                 order.Options = "Options";
                 order.OrderKindID = Guid.NewGuid();
                 order.OrderStatusID = Guid.NewGuid();
                 order.PrePayment = 55;
                 order.Recommendation = "Recommendation";
                 order.WarrantyTo = new DateTime(2017, 01, 2);

             var workItem = new WorkItemDTO();
             workItem.WorkItemID = Guid.NewGuid();
             workItem.EventDate = new DateTime(2015,10,7);
             workItem.Price = 55.66M;
             workItem.RepairOrderID = Guid.NewGuid();
             workItem.UserID = Guid.NewGuid();

             var deviceItem = new DeviceItemDTO();
             deviceItem.DeviceItemID = Guid.NewGuid();
             deviceItem.Count = 33;
             deviceItem.EventDate = new DateTime(2015,01,01);
             deviceItem.Price = 55.33M;
             deviceItem.RepairOrderID = Guid.NewGuid();
             deviceItem.Title = "Title";

             var timeLine = new OrderTimelineDTO();
             timeLine.EventDateTime = new DateTime(2015,06,07,01,2,3);
             timeLine.OrderTimelineID = Guid.NewGuid();
             timeLine.RepairOrderID = Guid.NewGuid();
             timeLine.TimelineKindID = 1;
             timeLine.Title = "Title";

             order.WorkItems.Add(workItem);
             order.OrderTimelines.Add(timeLine);
             order.DeviceItems.Add(deviceItem);
             message.RepairOrders.Add(order);

             var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
             var data = serializer.Serialize(message);
             Assert.IsNotNull(data);

             var savedMessage = serializer.DeserializeGetRepairOrdersResponse(data);

             Assert.IsNotNull(savedMessage);
             Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetRepairOrdersResponse);
             Assert.AreEqual(savedMessage.RepairOrders.Count, 1);
             Assert.AreEqual(savedMessage.RepairOrders[0].DeviceItems.Count, 1);
             Assert.AreEqual(savedMessage.RepairOrders[0].WorkItems.Count, 1);
             Assert.AreEqual(savedMessage.RepairOrders[0].OrderTimelines.Count, 1);


             var savedDeviceItem = savedMessage.RepairOrders[0].DeviceItems[0];
             var savedWorkItem = savedMessage.RepairOrders[0].WorkItems[0];
             var savedTimeline = savedMessage.RepairOrders[0].OrderTimelines[0];
             var savedItem = savedMessage.RepairOrders[0];

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



             Assert.AreEqual(deviceItem.DeviceItemID, savedDeviceItem.DeviceItemID);
             Assert.AreEqual(deviceItem.CostPrice, savedDeviceItem.CostPrice);
             Assert.AreEqual(deviceItem.EventDate, savedDeviceItem.EventDate);
             Assert.AreEqual(deviceItem.Price, savedDeviceItem.Price);
             Assert.AreEqual(deviceItem.RepairOrderID, savedDeviceItem.RepairOrderID);
             Assert.AreEqual(deviceItem.Title, savedDeviceItem.Title);
             Assert.AreEqual(deviceItem.UserID, savedDeviceItem.UserID);
             Assert.AreEqual(deviceItem.WarehouseItemID, savedDeviceItem.WarehouseItemID);

             Assert.AreEqual(savedWorkItem.EventDate, workItem.EventDate);
             Assert.AreEqual(savedWorkItem.Price, workItem.Price);
             Assert.AreEqual(savedWorkItem.RepairOrderID, workItem.RepairOrderID);
             Assert.AreEqual(savedWorkItem.Title, workItem.Title);
             Assert.AreEqual(savedWorkItem.UserID, workItem.UserID);
             Assert.AreEqual(savedWorkItem.WorkItemID, workItem.WorkItemID);

             Assert.AreEqual(savedTimeline.EventDateTime, timeLine.EventDateTime);
             Assert.AreEqual(savedTimeline.OrderTimelineID, timeLine.OrderTimelineID);
             Assert.AreEqual(savedTimeline.RepairOrderID, timeLine.RepairOrderID);
             Assert.AreEqual(savedTimeline.TimelineKindID, timeLine.TimelineKindID);
             Assert.AreEqual(savedTimeline.Title, timeLine.Title);
             
         }

        [Test]
         public void SaveRepairOrderRequestTest()
         {
             var message = new SaveRepairOrderRequest();

             Assert.AreEqual(message.Kind, MessageKind.SaveRepairOrderRequest);

             var order = new RepairOrderDTO();
             order.BranchID = Guid.NewGuid();
             order.CallEventDate = new DateTime(2015, 06, 01);
             order.ClientAddress = "ClientAddress";
             order.ClientEmail = "ClientEmail";
             order.ClientFullName = "ClientFullName";
             order.ClientPhone = "ClientPhone";
             order.DateOfBeReady = new DateTime(2015, 07, 07);
             order.Defect = "Defect";
             order.DeviceAppearance = "DeviceAppearance";
             order.DeviceModel = "DeviceModel";
             order.DeviceSN = "DeviceSN";
             order.DeviceTitle = "DeviceTitle";
             order.DeviceTrademark = "DeviceTrademark";
             order.EngineerID = Guid.NewGuid();
             order.EventDate = new DateTime(2014, 02, 05);
             order.GuidePrice = 44;
             order.IsUrgent = true;
             order.IssueDate = new DateTime(2013, 05, 04);
             order.IssuerID = Guid.NewGuid();
             order.ManagerID = Guid.NewGuid();
             order.Notes = "Notes";
             order.Number = "Number" + Guid.NewGuid();
             order.Options = "Options";
             order.OrderKindID = Guid.NewGuid();
             order.OrderStatusID = Guid.NewGuid();
             order.PrePayment = 55;
             order.Recommendation = "Recommendation";
             order.WarrantyTo = new DateTime(2017, 01, 2);

             var workItem = new WorkItemDTO();
             workItem.WorkItemID = Guid.NewGuid();
             workItem.EventDate = new DateTime(2015, 10, 7);
             workItem.Price = 55.66M;
             workItem.RepairOrderID = Guid.NewGuid();
             workItem.UserID = Guid.NewGuid();

             var deviceItem = new DeviceItemDTO();
             deviceItem.DeviceItemID = Guid.NewGuid();
             deviceItem.Count = 33;
             deviceItem.EventDate = new DateTime(2015, 01, 01);
             deviceItem.Price = 55.33M;
             deviceItem.RepairOrderID = Guid.NewGuid();
             deviceItem.Title = "Title";

             var timeLine = new OrderTimelineDTO();
             timeLine.EventDateTime = new DateTime(2015, 06, 07, 01, 2, 3);
             timeLine.OrderTimelineID = Guid.NewGuid();
             timeLine.RepairOrderID = Guid.NewGuid();
             timeLine.TimelineKindID = 1;
             timeLine.Title = "Title";

             order.WorkItems.Add(workItem);
             order.OrderTimelines.Add(timeLine);
             order.DeviceItems.Add(deviceItem);
             message.RepairOrder =order;

             var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
             var data = serializer.Serialize(message);
             Assert.IsNotNull(data);

             var savedMessage = serializer.DeserializeSaveRepairOrderRequest(data);

             Assert.IsNotNull(savedMessage);
             Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.SaveRepairOrderRequest);
             Assert.IsNotNull(savedMessage.RepairOrder);
             Assert.AreEqual(savedMessage.RepairOrder.DeviceItems.Count, 1);
             Assert.AreEqual(savedMessage.RepairOrder.WorkItems.Count, 1);
             Assert.AreEqual(savedMessage.RepairOrder.OrderTimelines.Count, 1);


             var savedDeviceItem = savedMessage.RepairOrder.DeviceItems[0];
             var savedWorkItem = savedMessage.RepairOrder.WorkItems[0];
             var savedTimeline = savedMessage.RepairOrder.OrderTimelines[0];
             var savedItem = savedMessage.RepairOrder;

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



             Assert.AreEqual(deviceItem.DeviceItemID, savedDeviceItem.DeviceItemID);
             Assert.AreEqual(deviceItem.CostPrice, savedDeviceItem.CostPrice);
             Assert.AreEqual(deviceItem.EventDate, savedDeviceItem.EventDate);
             Assert.AreEqual(deviceItem.Price, savedDeviceItem.Price);
             Assert.AreEqual(deviceItem.RepairOrderID, savedDeviceItem.RepairOrderID);
             Assert.AreEqual(deviceItem.Title, savedDeviceItem.Title);
             Assert.AreEqual(deviceItem.UserID, savedDeviceItem.UserID);
             Assert.AreEqual(deviceItem.WarehouseItemID, savedDeviceItem.WarehouseItemID);

             Assert.AreEqual(savedWorkItem.EventDate, workItem.EventDate);
             Assert.AreEqual(savedWorkItem.Price, workItem.Price);
             Assert.AreEqual(savedWorkItem.RepairOrderID, workItem.RepairOrderID);
             Assert.AreEqual(savedWorkItem.Title, workItem.Title);
             Assert.AreEqual(savedWorkItem.UserID, workItem.UserID);
             Assert.AreEqual(savedWorkItem.WorkItemID, workItem.WorkItemID);

             Assert.AreEqual(savedTimeline.EventDateTime, timeLine.EventDateTime);
             Assert.AreEqual(savedTimeline.OrderTimelineID, timeLine.OrderTimelineID);
             Assert.AreEqual(savedTimeline.RepairOrderID, timeLine.RepairOrderID);
             Assert.AreEqual(savedTimeline.TimelineKindID, timeLine.TimelineKindID);
             Assert.AreEqual(savedTimeline.Title, timeLine.Title);
         }
        [Test]
        public void SaveRepairOrderResponseTest()
        {
            var message = new SaveRepairOrderResponse();
            Assert.AreEqual(message.Kind, MessageKind.SaveRepairOrderResponse);

            message.Success = true;

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.SaveRepairOrderResponse);

            var savedMessage = serializer.DeserializeSaveRepairOrderResponse(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.Success, savedMessage.Success);
        }

        [Test]
        public void GetCustomReportItemRequestTest()
        {
            var message = new GetCustomReportItemRequest();
            Assert.AreEqual(message.Kind, MessageKind.GetCustomReportItemRequest);

            message.UserID = Guid.NewGuid();

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetCustomReportItemRequest);

            var savedMessage = serializer.DeserializeGetCustomReportItemRequest(data);

            Assert.IsNotNull(savedMessage);

            Assert.AreEqual(message.Kind, savedMessage.Kind);
            Assert.AreEqual(message.UserID, savedMessage.UserID);
        }

        [Test]
        public void GetCustomReportItemResponseTest()
        {
            var message = new GetCustomReportItemResponse();
            Assert.AreEqual(message.Kind, MessageKind.GetCustomReportItemResponse);
            var item1 = new CustomReportItemDTO();
            item1.CustomReportID = Guid.NewGuid();
            item1.DocumentKindID = 1;
            item1.Title = "Title";
            item1.HtmlContent = "HtmlContent";

            var item2 = new CustomReportItemDTO();
            item2.CustomReportID = Guid.NewGuid();
            item2.DocumentKindID = 1;
            item2.Title = "Title2";
            item2.HtmlContent = "HtmlConte323nt";

            message.CustomReportItems.Add(item1);
            message.CustomReportItems.Add(item2);
            

            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);
            var data = serializer.Serialize(message);
            Assert.IsNotNull(data);

            var savedMessage = serializer.DeserializeGetCustomReportItemResponse(data);

            Assert.IsNotNull(savedMessage);
            Assert.AreEqual(serializer.GetMessageInfoOrNull(data).Kind, MessageKind.GetCustomReportItemResponse);
            Assert.AreEqual(savedMessage.CustomReportItems.Count, 2);

            var savedItem1 = savedMessage.CustomReportItems[0];
            var savedItem2 = savedMessage.CustomReportItems[1];

            Assert.AreEqual(savedItem1.CustomReportID, item1.CustomReportID);
            Assert.AreEqual(savedItem1.DocumentKindID, item1.DocumentKindID);
            Assert.AreEqual(savedItem1.HtmlContent, item1.HtmlContent);
            Assert.AreEqual(savedItem1.Title, item1.Title);

            Assert.AreEqual(savedItem2.CustomReportID, item2.CustomReportID);
            Assert.AreEqual(savedItem2.DocumentKindID, item2.DocumentKindID);
            Assert.AreEqual(savedItem2.HtmlContent, item2.HtmlContent);
            Assert.AreEqual(savedItem2.Title, item2.Title);

        }
    }
}
