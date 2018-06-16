using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using NUnit.Framework;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.EntityFramework;

namespace Remontinka.Server.DataLayer.Test
{
    [TestFixture]
    public class EntityDeployTest
    {
        private RemontinkaStore CreateStore()
        {
            return new RemontinkaStore();
        }

        /*[Test]
        public void DeployTest()
        {
            var entity = new UserDomain();
            var store = CreateStore();
            entity.EventDate = new DateTime(2013, 1, 01);
            entity.UserDomainID = Guid.NewGuid();
            entity.IsActive = true;
            entity.LegalName = "LegalName12312344312";
            entity.RegistredEmail = "ya12312312eqqwe@ya.ru";
            entity.Trademark = "Trademark23123123";
            entity.UserLogin = "UserLogin22286786";
            entity.PasswordHash = "PasswordHash6";

            store.SaveUserDomain(entity);

            store.Deploy(entity.UserDomainID);

            var i = store.GetNewOrderNumber(entity.UserDomainID);
            i = store.GetNewOrderNumber(entity.UserDomainID);
            i++;
        }*/

        [Test]
        public void UserTest()
        {
            var domain = CreateTestUserDomain();
            try
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

                item.UserDomainID = domain.UserDomainID;

                store.SaveUser(item);

                var savedItem = store.GetUser(item.UserID,domain.UserDomainID);

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

                store.DeleteUser(item.UserID);
                savedItem = store.GetUser(item.UserID, domain.UserDomainID);

                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestUserDomain();
            }
            
        }

        private readonly Guid _testUserID = new Guid("78BA3B14-3D94-414C-9001-432F812DD19B");

        private User CreateTestUser()
        {
            var domain = CreateTestUserDomain();

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
            item.UserID = _testUserID;

            item.UserDomainID = domain.UserDomainID;
            store.SaveUser(item);

            return item;
        }

        private void DeleteTestUser()
        {
            var store = CreateStore();

            store.DeleteUser(_testUserID);
            DeleteTestUserDomain();
        }

        private readonly Guid _testBranchID = new Guid("3E7E3663-972B-4517-A234-27F1EE74DB81");

        [Test]
        public void BranchTest()
        {
            try
            {
                var store = CreateStore();
                var item = new Branch();
                var domain = CreateTestUserDomain();

                item.Address = "Address";
                item.Title = "Title";
                item.LegalName = "LegalName";
                item.UserDomainID = domain.UserDomainID;

                store.SaveBranch(item);

                var savedItem = store.GetBranch(item.BranchID,domain.UserDomainID);

                Assert.IsNotNull(savedItem);
                Assert.AreEqual(item.Address, savedItem.Address);
                Assert.AreEqual(item.BranchID, savedItem.BranchID);
                Assert.AreEqual(item.Title, savedItem.Title);
                Assert.AreEqual(item.LegalName, savedItem.LegalName);
                Assert.AreEqual(item.UserDomainID, savedItem.UserDomainID);

                store.DeleteBranch(item.BranchID);

                savedItem = store.GetBranch(item.BranchID,domain.UserDomainID);

                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestUserDomain();
            }
        }

        private Branch CreateTestBranch()
        {
            var domain = CreateTestUserDomain();
            var item = new Branch();
            var store = CreateStore();
            item.BranchID = _testBranchID;
            item.Address = "Address";
            item.Title = "Title";
            item.LegalName = "LegalName";
            item.UserDomainID = domain.UserDomainID;

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
                item.EventDate = new DateTime(2015, 01, 01);
                item.BranchID = branch.BranchID;
                item.UserID = user.UserID;

                var store = CreateStore();

                store.SaveUserBranchMapItem(item);

                var savedItem = store.GetUserBranchMapItem(item.UserBranchMapID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(item.UserBranchMapID, savedItem.UserBranchMapID);
                Assert.AreEqual(item.UserID, savedItem.UserID);
                Assert.AreEqual(item.EventDate, savedItem.EventDate);
                Assert.AreEqual(item.BranchID, savedItem.BranchID);

                store.DeleteUserBranchMapItem(item.UserBranchMapID);

                savedItem = store.GetUserBranchMapItem(item.UserBranchMapID);
                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestBranch();
                DeleteTestUser();
            }
        }

        [Test]
        public void OrderKindTest()
        {
            var domain = CreateTestUserDomain();
            try
            {
                var store = CreateStore();

                var item = new OrderKind();
                item.Title = "Title";
                item.UserDomainID = domain.UserDomainID;
                store.SaveOrderKind(item);

                var savedItem = store.GetOrderKind(item.OrderKindID,domain.UserDomainID);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.Title, savedItem.Title);
                Assert.AreEqual(item.UserDomainID, savedItem.UserDomainID);

                store.DeleteOrderKind(savedItem.OrderKindID);
                savedItem = store.GetOrderKind(item.OrderKindID,domain.UserDomainID);
                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestUserDomain();
            }
            
        }

        [Test]
        public void OrderStatusTest()
        {
            var domain = CreateTestUserDomain();
            try
            {
                var store = CreateStore();

                var item = new OrderStatus();
                item.Title = "Title";
                item.StatusKindID = StatusKindSet.Completed.StatusKindID;
                item.UserDomainID = domain.UserDomainID;
                store.SaveOrderStatus(item);

                var savedItem = store.GetOrderStatus(item.OrderStatusID,domain.UserDomainID);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.Title, savedItem.Title);
                Assert.AreEqual(item.StatusKindID, savedItem.StatusKindID);

                store.DeleteOrderStatus(savedItem.OrderStatusID);
                savedItem = store.GetOrderStatus(item.OrderStatusID,domain.UserDomainID);
                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestUserDomain();
            }
        }

        [Test]
        public void RepairOrderTest()
        {
            var store = CreateStore();
            var domain = CreateTestUserDomain();

            var orderStatus = new OrderStatus();
            orderStatus.Title = "Title";
            orderStatus.StatusKindID = StatusKindSet.Completed.StatusKindID;
            orderStatus.UserDomainID = domain.UserDomainID;
            store.SaveOrderStatus(orderStatus);

            var item = new OrderKind();
            item.Title = "Title";
            item.UserDomainID = domain.UserDomainID;
            store.SaveOrderKind(item);

            var branch = CreateTestBranch();
            var user = CreateTestUser();

            try
            {
                var order = new RepairOrder();
                order.BranchID = branch.BranchID;
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
                order.EngineerID = user.UserID;
                order.EventDate = new DateTime(2014, 02, 05);
                order.GuidePrice = 44M;
                order.IsUrgent = true;
                order.IssueDate = new DateTime(2013, 05, 04);
                order.IssuerID = user.UserID;
                order.ManagerID = user.UserID;
                order.Notes = "Notes";
                order.Number = "Number" + Guid.NewGuid();
                order.Options = "Options";
                order.OrderKindID = item.OrderKindID;
                order.OrderStatusID = orderStatus.OrderStatusID;
                order.PrePayment = 55M;
                order.Recommendation = "Recommendation";
                order.WarrantyTo = new DateTime(2017, 01, 2);
                order.UserDomainID = domain.UserDomainID;
                order.AccessPassword = "asdasdasd2123";

                store.SaveRepairOrder(order);

                var savedItem = store.GetRepairOrder(order.RepairOrderID, domain.UserDomainID);

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
                Assert.AreEqual(order.UserDomainID, savedItem.UserDomainID);
                Assert.AreEqual(order.AccessPassword, savedItem.AccessPassword);

                store.DeleteRepairOrder(order.RepairOrderID);

                savedItem = store.GetRepairOrder(order.RepairOrderID, domain.UserDomainID);

                Assert.IsNull(savedItem);
            }
            finally
            {
                store.DeleteOrderStatus(orderStatus.OrderStatusID);
                store.DeleteOrderKind(item.OrderKindID);
                DeleteTestBranch();
                DeleteTestUser();
            }

        }

        private readonly Guid _repairOrderId = new Guid("6ECA4833-4906-4235-A37F-EF8077867CD4");

        public RepairOrder CreateTestRepairOrder()
        {
            var store = CreateStore();
            var domain = CreateTestUserDomain();
            var orderStatus = new OrderStatus();
            orderStatus.Title = "Title";
            orderStatus.StatusKindID = StatusKindSet.Completed.StatusKindID;
            orderStatus.UserDomainID = domain.UserDomainID;

            store.SaveOrderStatus(orderStatus);

            var item = new OrderKind();
            item.Title = "Title";
            item.UserDomainID = domain.UserDomainID;
            store.SaveOrderKind(item);

            var branch = CreateTestBranch();
            var user = CreateTestUser();

            var order = new RepairOrder();
            order.BranchID = branch.BranchID;
            order.CallEventDate = new DateTime(2015, 06, 01);
            order.ClientAddress = "ClientAddress33323";
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
            order.EngineerID = user.UserID;
            order.EventDate = new DateTime(2014, 02, 05);
            order.GuidePrice = 44M;
            order.IsUrgent = true;
            order.IssueDate = new DateTime(2013, 05, 04);
            order.IssuerID = user.UserID;
            order.ManagerID = user.UserID;
            order.Notes = "Notes";
            order.Number = "Number" + Guid.NewGuid();
            order.Options = "Options";
            order.OrderKindID = item.OrderKindID;
            order.OrderStatusID = orderStatus.OrderStatusID;
            order.PrePayment = 55M;
            order.Recommendation = "Recommendation";
            order.WarrantyTo = new DateTime(2017, 01, 2);
            order.RepairOrderID = _repairOrderId;
            order.UserDomainID = domain.UserDomainID;
            store.SaveRepairOrder(order);

            return order;
        }

        private void DeleteTestRepairOrder()
        {
            var store = CreateStore();

            var savedItem = store.GetRepairOrder(_repairOrderId,_testUserDomainId);
            var orderStatusId = savedItem.OrderStatusID;
            var orderKindId = savedItem.OrderKindID;

            store.DeleteRepairOrder(_repairOrderId);

            DeleteTestBranch();
            store.DeleteOrderKind(orderKindId);
            store.DeleteOrderStatus(orderStatusId);
            DeleteTestUser();
            
        }

        [Test]
        public void WorkItemTest()
        {
            var store = CreateStore();
            var order = CreateTestRepairOrder();

            try
            {
                var item = new WorkItem();
                item.EventDate = new DateTime(2015,06,01);
                item.Price = 10M;
                item.Title = "Title";
                item.Notes = "Notes1";
                item.UserID = order.IssuerID;
                item.RepairOrderID = order.RepairOrderID;

                store.SaveWorkItem(item);

                var savedItem = store.GetWorkItem(item.WorkItemID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.EventDate,savedItem.EventDate);
                Assert.AreEqual(item.Price, savedItem.Price);
                Assert.AreEqual(item.Title, savedItem.Title);
                Assert.AreEqual(item.Notes, savedItem.Notes);
                Assert.AreEqual(item.UserID, savedItem.UserID);

                store.DeleteWorkItem(item.WorkItemID);
                savedItem = store.GetWorkItem(item.WorkItemID);

                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestRepairOrder();
                
            }
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
                item.CostPrice = 22.3M;
                item.Count = 22;
                item.Price = 56M;
                item.Title = "Title";
                item.RepairOrderID = order.RepairOrderID;
                item.EventDate = new DateTime(2015,06,04);
                item.UserID = order.ManagerID;

                store.SaveDeviceItem(item);

                var savedItem = store.GetDeviceItem(item.DeviceItemID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.CostPrice, savedItem.CostPrice);
                Assert.AreEqual(item.Count, savedItem.Count);
                Assert.AreEqual(item.Price, savedItem.Price);
                Assert.AreEqual(item.Title, savedItem.Title);
                Assert.AreEqual(item.UserID, savedItem.UserID);
                Assert.AreEqual(item.EventDate, savedItem.EventDate);
                Assert.AreEqual(item.WarehouseItemID, savedItem.WarehouseItemID);

                item.CostPrice = 25.3M;
                item.Count = 12;
                item.Price = 54M;
                item.Title = "Title2";
                item.WarehouseItemID = warehouseItem.WarehouseItemID;

                store.SaveDeviceItem(item);

                savedItem = store.GetDeviceItem(item.DeviceItemID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.CostPrice, savedItem.CostPrice);
                Assert.AreEqual(item.Count, savedItem.Count);
                Assert.AreEqual(item.Price, savedItem.Price);
                Assert.AreEqual(item.Title, savedItem.Title);
                Assert.AreEqual(item.UserID, savedItem.UserID);
                Assert.AreEqual(item.EventDate, savedItem.EventDate);
                Assert.AreEqual(item.WarehouseItemID, savedItem.WarehouseItemID);

                store.DeleteDeviceItem(item.DeviceItemID);

                savedItem = store.GetDeviceItem(item.DeviceItemID);

                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestWarehouseItem();
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
                item.EventDateTime = new DateTime(2016,03,01);
                item.RepairOrderID = order.RepairOrderID;
                item.TimelineKindID = TimelineKindSet.Completed.TimelineKindID;
                item.Title = "Title";

                store.SaveOrderTimeline(item);
                var savedItem = store.GetOrderTimeline(item.OrderTimelineID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.EventDateTime,savedItem.EventDateTime);
                Assert.AreEqual(item.TimelineKindID, savedItem.TimelineKindID);
                Assert.AreEqual(item.Title, savedItem.Title);

                store.DeleteOrderTimeline(item.OrderTimelineID);
                savedItem = store.GetOrderTimeline(item.OrderTimelineID);

                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestRepairOrder();
            }
        }

        [Test]
        public void CustomReportTest()
        {
            var domain = CreateTestUserDomain();

            try
            {
                var store = CreateStore();
                var entity = new CustomReportItem();
                entity.DocumentKindID = DocumentKindSet.OrderReportDocument.DocumentKindID;
                entity.Title = "Title";
                entity.HtmlContent = "HtmlContent";
                entity.UserDomainID = domain.UserDomainID;

                store.SaveCustomReportItem(entity);

                var savedItem = store.GetCustomReportItem(entity.CustomReportID,domain.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(entity.DocumentKindID, savedItem.DocumentKindID);
                Assert.AreEqual(entity.HtmlContent, savedItem.HtmlContent);
                Assert.AreEqual(entity.Title, savedItem.Title);

                store.DeleteCustomReportItem(entity.CustomReportID);

                savedItem = store.GetCustomReportItem(entity.CustomReportID,domain.UserDomainID);

                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestUserDomain();
            }
        }

        private readonly Guid _testUserDomainId = new Guid("B31A37BF-E182-4942-B753-43BB893C0347");

        private UserDomain CreateTestUserDomain()
        {
            var store = CreateStore();
            var entity = new UserDomain();
            entity.EventDate = new DateTime(2013, 1, 01);
            entity.UserDomainID = _testUserDomainId;
            entity.IsActive = true;
            entity.LegalName = "LegalName12312344312";
            entity.RegistredEmail = "ya12312312eqqwe@ya.ru";
            entity.Trademark = "Trademark23123123";
            entity.UserLogin = "UserLogin22286786";
            entity.PasswordHash = "PasswordHash6";
            entity.Address = "Address22323";

            store.SaveUserDomain(entity);

            return entity;
        }

        private void DeleteTestUserDomain()
        {
            var store = CreateStore();
            store.DeleteUserDomain(_testUserDomainId);
        }

        [Test]
        public void UserDomainTest()
        {
            var store = CreateStore();
            var entity = new UserDomain();
            entity.EventDate = new DateTime(2014,05,07);
            entity.IsActive = true;
            entity.LegalName = "LegalName";
            entity.RegistredEmail = "ya@ya.ru";
            entity.Trademark = "Trademark";
            entity.UserLogin = "UserLogin";
            entity.PasswordHash = "PasswordHash";
            entity.Address = "Address";
            
            store.SaveUserDomain(entity);

            var savedEntity = store.GetUserDomain(entity.UserDomainID);

            Assert.IsNotNull(savedEntity);
            Assert.AreEqual(savedEntity.EventDate,entity.EventDate);
            Assert.AreEqual(savedEntity.IsActive, entity.IsActive);
            Assert.AreEqual(savedEntity.LegalName, entity.LegalName);
            Assert.AreEqual(savedEntity.Trademark, entity.Trademark);
            Assert.AreEqual(savedEntity.UserLogin, entity.UserLogin);
            Assert.AreEqual(savedEntity.PasswordHash, entity.PasswordHash);
            Assert.AreEqual(savedEntity.Address, entity.Address);

            entity.EventDate = new DateTime(2015, 06, 07);
            entity.IsActive = false;
            entity.LegalName = "Leg2alName";
            entity.RegistredEmail = "y3a@ya.ru";
            entity.Trademark = "Tradem333ark";
            entity.UserLogin = "UserLogi2123n";
            entity.Address = "Address1212";

            store.SaveUserDomain(entity);

            savedEntity = store.GetUserDomain(entity.UserDomainID);

            Assert.IsNotNull(savedEntity);
            Assert.AreEqual(savedEntity.EventDate, entity.EventDate);
            Assert.AreEqual(savedEntity.IsActive, entity.IsActive);
            Assert.AreEqual(savedEntity.LegalName, entity.LegalName);
            Assert.AreEqual(savedEntity.Trademark, entity.Trademark);
            Assert.AreEqual(savedEntity.UserLogin, entity.UserLogin);
            Assert.AreEqual(savedEntity.Address, entity.Address);

            store.DeleteUserDomain(entity.UserDomainID);

            savedEntity = store.GetUserDomain(entity.UserDomainID);

            Assert.IsNull(savedEntity);
        }

        private readonly Guid _financialGroupItemId = new Guid("4B88797B-6878-4786-B247-215F623DF68E");

        private FinancialGroupItem CreateTestFinancialGroupItem()
        {
            var domain = CreateTestUserDomain();
            var item = new FinancialGroupItem();
            item.LegalName = "LegalName";
            item.Trademark = "Trademark";
            item.Title = "Title";
            item.UserDomainID = domain.UserDomainID;
            item.FinancialGroupID = _financialGroupItemId;

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
            var domain = CreateTestUserDomain();

            try
            {
                var item = new FinancialGroupItem();
                item.LegalName = "LegalName";
                item.Trademark = "Trademark";
                item.Title = "Title";
                item.UserDomainID = domain.UserDomainID;

                var store = CreateStore();

                store.SaveFinancialGroupItem(item);

                var savedItem = store.GetFinancialGroupItem(item.FinancialGroupID, domain.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.FinancialGroupID,item.FinancialGroupID);
                Assert.AreEqual(savedItem.LegalName,item.LegalName);
                Assert.AreEqual(savedItem.Title,item.Title);
                Assert.AreEqual(savedItem.Trademark,item.Trademark);
                Assert.AreEqual(savedItem.UserDomainID,item.UserDomainID);

                item.LegalName = "LegalName2";
                item.Trademark = "Tradem3ark";
                item.Title = "Titl4e";

                store.SaveFinancialGroupItem(item);

                savedItem = store.GetFinancialGroupItem(item.FinancialGroupID, domain.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.FinancialGroupID, item.FinancialGroupID);
                Assert.AreEqual(savedItem.LegalName, item.LegalName);
                Assert.AreEqual(savedItem.Title, item.Title);
                Assert.AreEqual(savedItem.Trademark, item.Trademark);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);

                store.DeleteFinancialGroupItem(item.FinancialGroupID);

                savedItem = store.GetFinancialGroupItem(item.FinancialGroupID, domain.UserDomainID);

                Assert.IsNull(savedItem);

            }
            finally 
            {
                
                DeleteTestUserDomain();
            } //try

        }

        private readonly Guid _financialItemId = new Guid("0CDEFF9F-B125-4FC5-88FE-168581CAED54");

        private FinancialItem CreateTestFinancialItem()
        {
            var domain = CreateTestUserDomain();
            var item = new FinancialItem();
            item.Description = "Description";
            item.EventDate = new DateTime(2015, 06, 03);
            item.FinancialItemKindID = FinancialItemKindSet.Custom.FinancialItemKindID;
            item.TransactionKindID = TransactionKindSet.Expenditure.TransactionKindID;
            item.Title = "Title";
            item.UserDomainID = domain.UserDomainID;
            item.FinancialItemID = _financialItemId;

            var store = CreateStore();

            store.SaveFinancialItem(item);

            return item;
        }

        private void DeleteTestFinancialItem()
        {
            var store = CreateStore();
            store.DeleteFinancialItem(_financialItemId);
        }

        [Test]
        public void FinancialItemTest()
        {
            var domain = CreateTestUserDomain();

            try
            {
                var item = new FinancialItem();
                item.Description = "Description";
                item.EventDate = new DateTime(2015,06,03);
                item.FinancialItemKindID = FinancialItemKindSet.Custom.FinancialItemKindID;
                item.TransactionKindID = TransactionKindSet.Expenditure.TransactionKindID;
                item.Title = "Title";
                item.UserDomainID = domain.UserDomainID;

                var store = CreateStore();

                store.SaveFinancialItem(item);

                var savedItem = store.GetFinancialItem(item.FinancialItemID, domain.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.FinancialItemID, item.FinancialItemID);
                Assert.AreEqual(savedItem.Description, item.Description);
                Assert.AreEqual(savedItem.Title, item.Title);
                Assert.AreEqual(savedItem.FinancialItemKindID, item.FinancialItemKindID);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);
                Assert.AreEqual(savedItem.TransactionKindID, item.TransactionKindID);
                Assert.AreEqual(savedItem.EventDate, item.EventDate);

                item.Description = "Description2";
                item.EventDate = new DateTime(2015, 08, 03);
                item.FinancialItemKindID = FinancialItemKindSet.OrderPaid.FinancialItemKindID;
                item.TransactionKindID = TransactionKindSet.Revenue.TransactionKindID;
                item.Title = "Title2";

                store.SaveFinancialItem(item);

                savedItem = store.GetFinancialItem(item.FinancialItemID, domain.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.FinancialItemID, item.FinancialItemID);
                Assert.AreEqual(savedItem.Description, item.Description);
                Assert.AreEqual(savedItem.Title, item.Title);
                Assert.AreEqual(savedItem.FinancialItemKindID, item.FinancialItemKindID);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);
                Assert.AreEqual(savedItem.TransactionKindID, item.TransactionKindID);
                Assert.AreEqual(savedItem.EventDate, item.EventDate);

                store.DeleteFinancialItem(item.FinancialItemID);

                savedItem = store.GetFinancialItem(item.FinancialItemID, domain.UserDomainID);

                Assert.IsNull(savedItem);

            }
            finally
            {

                DeleteTestUserDomain();
            } //try

        }

        [Test]
        public void FinancialItemValueTest()
        {
            var domain = CreateTestUserDomain();
            var finItem = CreateTestFinancialItem();
            var finGroup = CreateTestFinancialGroupItem();
            
            try
            {
                var item = new FinancialItemValue();
                item.Amount = 52.33M;
                item.CostAmount = 11.54M;
                item.Description = "Description";
                item.EventDate = new DateTime(2015,06,01);
                item.FinancialGroupID = finGroup.FinancialGroupID;
                item.FinancialItemID = finItem.FinancialItemID;

                var store = CreateStore();
                
                store.SaveFinancialItemValue(item);

                var savedItem = store.GetFinancialItemValue(item.FinancialItemValueID, domain.UserDomainID);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.Amount,savedItem.Amount);
                Assert.AreEqual(item.CostAmount, savedItem.CostAmount);
                Assert.AreEqual(item.Description, savedItem.Description);
                Assert.AreEqual(item.EventDate, savedItem.EventDate);
                Assert.AreEqual(item.FinancialGroupID, savedItem.FinancialGroupID);
                Assert.AreEqual(item.FinancialItemID, savedItem.FinancialItemID);
                Assert.AreEqual(item.FinancialItemValueID, savedItem.FinancialItemValueID);

                item.Amount = 72.13M;
                item.CostAmount = 14.84M;
                item.Description = "Description2";
                item.EventDate = new DateTime(2016, 01, 01);

                store.SaveFinancialItemValue(item);

                savedItem = store.GetFinancialItemValue(item.FinancialItemValueID, domain.UserDomainID);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.Amount, savedItem.Amount);
                Assert.AreEqual(item.CostAmount, savedItem.CostAmount);
                Assert.AreEqual(item.Description, savedItem.Description);
                Assert.AreEqual(item.EventDate, savedItem.EventDate);
                Assert.AreEqual(item.FinancialGroupID, savedItem.FinancialGroupID);
                Assert.AreEqual(item.FinancialItemID, savedItem.FinancialItemID);
                Assert.AreEqual(item.FinancialItemValueID, savedItem.FinancialItemValueID);

                store.DeleteFinancialItemValue(item.FinancialItemValueID);
                savedItem = store.GetFinancialItemValue(item.FinancialItemValueID, domain.UserDomainID);
                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestFinancialGroupItem();
                DeleteTestFinancialItem();
                DeleteTestUserDomain();
            }
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

                var savedItem = store.GetFinancialGroupMapBranchItem(item.FinancialGroupBranchMapID);
                Assert.IsNotNull(savedItem);

                store.DeleteFinancialGroupBranchMapItem(item.FinancialGroupBranchMapID);
                savedItem = store.GetFinancialGroupMapBranchItem(item.FinancialGroupBranchMapID);
                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestBranch();
                DeleteTestFinancialGroupItem();
                DeleteTestUserDomain();
            }
        }

        private readonly Guid _itemCategoryId = new Guid("5D5DD943-1648-4410-9C68-8091B62AE34E");

        private ItemCategory CreateTestItemCategory()
        {
            var domain = CreateTestUserDomain();
            var item = new ItemCategory();
            item.Title = "Title";
            item.ItemCategoryID = _itemCategoryId;
            item.UserDomainID = domain.UserDomainID;

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
            var domain = CreateTestUserDomain();
            try
            {
                var item = new ItemCategory();
                item.Title = "Title";
                item.UserDomainID = domain.UserDomainID;

                var store = CreateStore();
                store.SaveItemCategory(item);

                var savedItem = store.GetItemCategory(item.ItemCategoryID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.Title,item.Title);
                item.Title = "Title22";
                store.SaveItemCategory(item);

                savedItem = store.GetItemCategory(item.ItemCategoryID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.Title, item.Title);

                store.DeleteItemCategory(item.ItemCategoryID);
                savedItem = store.GetItemCategory(item.ItemCategoryID, item.UserDomainID);
                Assert.IsNull(savedItem);

            }
            finally
            {
                DeleteTestUserDomain();
            }
        }

        private readonly Guid _warehouseId = new Guid("126208C7-53C0-4BA4-9C87-CD8214842B37");

        private Warehouse CreateTestWarehouse()
        {
            var domain = CreateTestUserDomain();
            var item = new Warehouse();
            item.Title = "Title";
            item.UserDomainID = domain.UserDomainID;
            item.WarehouseID = _warehouseId;

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
            var domain = CreateTestUserDomain();
            try
            {
                var item = new Warehouse();
                item.Title = "Title";
                item.UserDomainID = domain.UserDomainID;

                var store = CreateStore();
                store.SaveWarehouse(item);

                var savedItem = store.GetWarehouse(item.WarehouseID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.Title, item.Title);
                item.Title = "Title22";
                store.SaveWarehouse(item);

                savedItem = store.GetWarehouse(item.WarehouseID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.Title, item.Title);

                store.DeleteWarehouse(item.WarehouseID);
                savedItem = store.GetWarehouse(item.WarehouseID, item.UserDomainID);
                Assert.IsNull(savedItem);

            }
            finally
            {
                DeleteTestUserDomain();
            }
        }


        private readonly Guid _goodsItemId = new Guid("999AD6A9-6E81-4089-BF3A-C71B2428D535");

        private GoodsItem CreateTestGoodsItem()
        {
            var domain = CreateTestUserDomain();
            var category = CreateTestItemCategory();
            var item = new GoodsItem();
            item.GoodsItemID = _goodsItemId;
            item.Title = "Title";
            item.Particular = "Particular";
            item.ItemCategoryID = category.ItemCategoryID;
            item.UserDomainID = domain.UserDomainID;
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
            var domain = CreateTestUserDomain();
            var category = CreateTestItemCategory();
            try
            {
                var item = new GoodsItem();
                item.Title = "Title";
                item.Particular = "Particular";
                item.ItemCategoryID = category.ItemCategoryID;
                item.UserDomainID = domain.UserDomainID;
                item.BarCode = "BarCode";
                item.Description = "Description";
                item.UserCode = "UserCode";
                item.DimensionKindID = DimensionKindSet.Thing.DimensionKindID;

                var store = CreateStore();
                store.SaveGoodsItem(item);

                var savedItem = store.GetGoodsItem(item.GoodsItemID, item.UserDomainID);
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

                savedItem = store.GetGoodsItem(item.GoodsItemID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.Title, item.Title);
                Assert.AreEqual(savedItem.Particular, item.Particular);
                Assert.AreEqual(savedItem.ItemCategoryID, item.ItemCategoryID);
                Assert.AreEqual(savedItem.BarCode, item.BarCode);
                Assert.AreEqual(savedItem.Description, item.Description);
                Assert.AreEqual(savedItem.UserCode, item.UserCode);

                store.DeleteGoodsItem(item.GoodsItemID);
                savedItem = store.GetGoodsItem(item.GoodsItemID, item.UserDomainID);
                Assert.IsNull(savedItem);

            }
            finally
            {
                DeleteTestItemCategory();
                DeleteTestUserDomain();
            }
        }

        private readonly Guid _warehouseItemId = new Guid("C2D4DC21-9488-4666-BABE-378627408902");

        private WarehouseItem CreateTestWarehouseItem()
        {
            var warehouse = CreateTestWarehouse();
            var goodsItem = CreateTestGoodsItem();
            var item = new WarehouseItem();
            item.GoodsItemID = goodsItem.GoodsItemID;
            item.RepairPrice = 22.56M;
            item.SalePrice = 663.44M;
            item.StartPrice = 354.0M;
            item.Total = 33.2M;
            item.WarehouseID = warehouse.WarehouseID;
            item.WarehouseItemID = _warehouseItemId;

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
                item.RepairPrice = 22.56M;
                item.SalePrice = 663.44M;
                item.StartPrice = 354.0M;
                item.Total = 33.2M;
                item.WarehouseID = warehouse.WarehouseID;

                var store = CreateStore();
                store.SaveWarehouseItem(item);

                var savedItem = store.GetWarehouseItem(item.WarehouseItemID, warehouse.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.GoodsItemID, item.GoodsItemID);
                Assert.AreEqual(savedItem.SalePrice, item.SalePrice);
                Assert.AreEqual(savedItem.RepairPrice, item.RepairPrice);
                Assert.AreEqual(savedItem.StartPrice, item.StartPrice);
                Assert.AreEqual(savedItem.Total, item.Total);
                Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);

                item.RepairPrice = 122.56M;
                item.SalePrice = 1663.44M;
                item.StartPrice = 1354.0M;
                item.Total = 333.2M;
                store.SaveWarehouseItem(item);

                savedItem = store.GetWarehouseItem(item.WarehouseItemID, warehouse.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.GoodsItemID, item.GoodsItemID);
                Assert.AreEqual(savedItem.SalePrice, item.SalePrice);
                Assert.AreEqual(savedItem.RepairPrice, item.RepairPrice);
                Assert.AreEqual(savedItem.StartPrice, item.StartPrice);
                Assert.AreEqual(savedItem.Total, item.Total);
                Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);

                store.DeleteWarehouseItem(item.WarehouseItemID);
                savedItem = store.GetWarehouseItem(item.WarehouseItemID, warehouse.UserDomainID);
                Assert.IsNull(savedItem);

            }
            finally
            {
                DeleteTestWarehouse();
                DeleteTestGoodsItem();
                DeleteTestUserDomain();
            }
        }

        private readonly Guid _contractorId = new Guid("FAF275C8-82F3-4543-AEF0-F0BCEB01B559");

        private Contractor CreateTestContractor()
        {
            var domain = CreateTestUserDomain();
            var item = new Contractor();
            item.LegalName = "LegalName";
            item.Phone = "Phone";
            item.Trademark = "Trademark";
            item.Address = "Address";
            item.EventDate = new DateTime(2015, 03, 04);
            item.UserDomainID = domain.UserDomainID;
            item.ContractorID = _contractorId;

            var store = CreateStore();
            store.SaveContractor(item);
            return item;
        }

        private void DeleteTestContractor()
        {
            var store = CreateStore();
            store.DeleteContractor(_contractorId);
        }

        [Test]
        public void ContractorTest()
        {
            var domain = CreateTestUserDomain();
            try
            {
                var item = new Contractor();
                item.LegalName = "LegalName";
                item.Phone = "Phone";
                item.Trademark = "Trademark";
                item.Address = "Address";
                item.EventDate = new DateTime(2015,03,04);
                item.UserDomainID = domain.UserDomainID;

                var store = CreateStore();
                store.SaveContractor(item);

                var savedItem = store.GetContractor(item.ContractorID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.LegalName, item.LegalName);
                Assert.AreEqual(savedItem.Phone, item.Phone);
                Assert.AreEqual(savedItem.Trademark, item.Trademark);
                Assert.AreEqual(savedItem.Address, item.Address);
                Assert.AreEqual(savedItem.EventDate, item.EventDate);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);
                
                store.SaveContractor(item);

                savedItem = store.GetContractor(item.ContractorID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.LegalName, item.LegalName);
                Assert.AreEqual(savedItem.Phone, item.Phone);
                Assert.AreEqual(savedItem.Trademark, item.Trademark);
                Assert.AreEqual(savedItem.Address, item.Address);
                Assert.AreEqual(savedItem.EventDate, item.EventDate);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);

                store.DeleteContractor(item.ContractorID);
                savedItem = store.GetContractor(item.ContractorID, item.UserDomainID);
                Assert.IsNull(savedItem);

            }
            finally
            {
                DeleteTestUserDomain();
            }
        }

        private readonly Guid _incomingDocId = new Guid("D69B36A9-F071-4C93-8EE4-FE5F7BFD6A7A");

        private IncomingDoc CreateTestIncomingDoc()
        {
            var domain = CreateTestUserDomain();
            var contractor = CreateTestContractor();
            var user = CreateTestUser();
            var warehouse = CreateTestWarehouse();
            var item = new IncomingDoc();
            item.ContractorID = contractor.ContractorID;
            item.CreatorID = user.UserID;
            item.DocDate = new DateTime(2012, 02, 03);
            item.DocDescription = "DocDescription";
            item.DocNumber = "DocNumber";
            item.WarehouseID = warehouse.WarehouseID;
            item.UserDomainID = domain.UserDomainID;
            item.IncomingDocID = _incomingDocId;

            var store = CreateStore();
            store.SaveIncomingDoc(item);
            return item;
        }

        private void DeleteTestIncomingDoc()
        {
            var store = CreateStore();
            store.DeleteIncomingDoc(_incomingDocId);
            DeleteTestContractor();
            DeleteTestWarehouse();
            DeleteTestUser();
        }

        [Test]
        public void IncomingDocTest()
        {
            var domain = CreateTestUserDomain();
            var contractor = CreateTestContractor();
            var user = CreateTestUser();
            var warehouse = CreateTestWarehouse();
            try
            {
                var item = new IncomingDoc();
                item.ContractorID = contractor.ContractorID;
                item.CreatorID = user.UserID;
                item.DocDate = new DateTime(2012, 02, 03);
                item.DocDescription = "DocDescription";
                item.DocNumber = "DocNumber";
                item.WarehouseID = warehouse.WarehouseID;
                item.UserDomainID = domain.UserDomainID;

                var store = CreateStore();
                store.SaveIncomingDoc(item);

                var savedItem = store.GetIncomingDoc(item.IncomingDocID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.ContractorID, item.ContractorID);
                Assert.AreEqual(savedItem.CreatorID, item.CreatorID);
                Assert.AreEqual(savedItem.DocDate, item.DocDate);
                Assert.AreEqual(savedItem.DocDescription, item.DocDescription);
                Assert.AreEqual(savedItem.DocNumber, item.DocNumber);
                Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);
                item.DocDate = new DateTime(2011, 03, 03);
                item.DocDescription = "DocDescription3";
                item.DocNumber = "DocNumbe2r";
                store.SaveIncomingDoc(item);

                savedItem = store.GetIncomingDoc(item.IncomingDocID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.ContractorID, item.ContractorID);
                Assert.AreEqual(savedItem.CreatorID, item.CreatorID);
                Assert.AreEqual(savedItem.DocDate, item.DocDate);
                Assert.AreEqual(savedItem.DocDescription, item.DocDescription);
                Assert.AreEqual(savedItem.DocNumber, item.DocNumber);
                Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);

                store.DeleteIncomingDoc(item.IncomingDocID);
                savedItem = store.GetIncomingDoc(item.IncomingDocID, item.UserDomainID);
                Assert.IsNull(savedItem);

            }
            finally
            {
                DeleteTestContractor();
                DeleteTestWarehouse();
                DeleteTestUser();
            }
        }

        [Test]
        public void IncomingDocItemTest()
        {
            var incomingDoc = CreateTestIncomingDoc();
            var goodsItem = CreateTestGoodsItem();
            try
            {
                var item = new IncomingDocItem();
                item.IncomingDocID = incomingDoc.IncomingDocID;
                item.GoodsItemID = goodsItem.GoodsItemID;
                item.Description = "Description";
                item.InitPrice = 63.22M;
                item.RepairPrice = 54.96M;
                item.SalePrice = 52.22M;
                item.StartPrice = 33.12M;
                item.Total = 553.0M;
                var store = CreateStore();

                store.SaveIncomingDocItem(item);

                var savedItem = store.GetIncomingDocItem(item.IncomingDocItemID, incomingDoc.UserDomainID);
                
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.Description,item.Description);
                Assert.AreEqual(savedItem.InitPrice, item.InitPrice);
                Assert.AreEqual(savedItem.RepairPrice, item.RepairPrice);
                Assert.AreEqual(savedItem.SalePrice, item.SalePrice);
                Assert.AreEqual(savedItem.Total, item.Total);
                Assert.AreEqual(savedItem.StartPrice, item.StartPrice);

                item.Description = "Description2";
                item.InitPrice = 73.42M;
                item.RepairPrice = 63.16M;
                item.SalePrice = 452.21M;
                item.StartPrice = 133.12M;
                item.Total = 5.0M;

                store.SaveIncomingDocItem(item);

                savedItem = store.GetIncomingDocItem(item.IncomingDocItemID, incomingDoc.UserDomainID);

                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.Description, item.Description);
                Assert.AreEqual(savedItem.InitPrice, item.InitPrice);
                Assert.AreEqual(savedItem.RepairPrice, item.RepairPrice);
                Assert.AreEqual(savedItem.SalePrice, item.SalePrice);
                Assert.AreEqual(savedItem.Total, item.Total);
                Assert.AreEqual(savedItem.StartPrice, item.StartPrice);

                Assert.AreEqual(incomingDoc.UserDomainID,store.GetIncomingDocItemUserDomainID(item.IncomingDocItemID));

                store.DeleteIncomingDocItem(item.IncomingDocItemID);

            }
            finally
            {
                DeleteTestGoodsItem();
                DeleteTestIncomingDoc();
            }
        }

        private readonly Guid _cancellationDocId = new Guid("C281F805-3980-4D5D-9744-F050EEF4649C");

        private CancellationDoc CreateTestCancellationDoc()
        {
            var domain = CreateTestUserDomain();
            var user = CreateTestUser();
            var warehouse = CreateTestWarehouse();
            var item = new CancellationDoc();
            item.CreatorID = user.UserID;
            item.DocDate = new DateTime(2012, 02, 03);
            item.DocDescription = "DocDescription";
            item.DocNumber = "DocNumber";
            item.WarehouseID = warehouse.WarehouseID;
            item.UserDomainID = domain.UserDomainID;
            item.CancellationDocID = _cancellationDocId;

            var store = CreateStore();
            store.SaveCancellationDoc(item);
            return item;
        }

        private void DeleteTestCancellationDoc()
        {
            var store = CreateStore();
            store.DeleteCancellationDoc(_cancellationDocId);
            DeleteTestWarehouse();
            DeleteTestUser();
        }

        [Test]
        public void CancellationDocTest()
        {
            var domain = CreateTestUserDomain();
            var user = CreateTestUser();
            var warehouse = CreateTestWarehouse();
            try
            {
                var item = new CancellationDoc();
                item.CreatorID = user.UserID;
                item.DocDate = new DateTime(2012, 02, 03);
                item.DocDescription = "DocDescription";
                item.DocNumber = "DocNumber";
                item.WarehouseID = warehouse.WarehouseID;
                item.UserDomainID = domain.UserDomainID;

                var store = CreateStore();
                store.SaveCancellationDoc(item);

                var savedItem = store.GetCancellationDoc(item.CancellationDocID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                
                Assert.AreEqual(savedItem.CreatorID, item.CreatorID);
                Assert.AreEqual(savedItem.DocDate, item.DocDate);
                Assert.AreEqual(savedItem.DocDescription, item.DocDescription);
                Assert.AreEqual(savedItem.DocNumber, item.DocNumber);
                Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);
                item.DocDate = new DateTime(2011, 03, 03);
                item.DocDescription = "DocDescription3";
                item.DocNumber = "DocNumbe2r";
                store.SaveCancellationDoc(item);

                savedItem = store.GetCancellationDoc(item.CancellationDocID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.CreatorID, item.CreatorID);
                Assert.AreEqual(savedItem.DocDate, item.DocDate);
                Assert.AreEqual(savedItem.DocDescription, item.DocDescription);
                Assert.AreEqual(savedItem.DocNumber, item.DocNumber);
                Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);

                store.DeleteCancellationDoc(item.CancellationDocID);
                savedItem = store.GetCancellationDoc(item.CancellationDocID, item.UserDomainID);
                Assert.IsNull(savedItem);

            }
            finally
            {
                DeleteTestWarehouse();
                DeleteTestUser();
            }
        }

        [Test]
        public void CancellationDocItemTest()
        {
            var cancellationDoc = CreateTestCancellationDoc();
            var goodsItem = CreateTestGoodsItem();
            try
            {
                var item = new CancellationDocItem();
                item.CancellationDocID = cancellationDoc.CancellationDocID;
                item.GoodsItemID = goodsItem.GoodsItemID;
                item.Description = "Description";
                item.Total = 553.0M;
                var store = CreateStore();

                store.SaveCancellationDocItem(item);

                var savedItem = store.GetCancellationDocItem(item.CancellationDocItemID, cancellationDoc.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.Description, item.Description);
                Assert.AreEqual(savedItem.Total, item.Total);

                item.Description = "Description2";
                item.Total = 5.0M;

                store.SaveCancellationDocItem(item);

                savedItem = store.GetCancellationDocItem(item.CancellationDocItemID, cancellationDoc.UserDomainID);

                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.Description, item.Description);
                Assert.AreEqual(savedItem.Total, item.Total);
                

                Assert.AreEqual(cancellationDoc.UserDomainID, store.GetCancellationDocItemUserDomainID(item.CancellationDocItemID));

                store.DeleteCancellationDocItem(item.CancellationDocItemID);

            }
            finally
            {
                DeleteTestGoodsItem();
                DeleteTestCancellationDoc();
            }
        }

        private readonly Guid _transferDocId = new Guid("93C0B033-5549-4054-A7A3-55755FBF6A27");

        private TransferDoc CreateTestTransferDoc()
        {
            var domain = CreateTestUserDomain();
            var user = CreateTestUser();
            var warehouse = CreateTestWarehouse();
            var item = new TransferDoc();
            item.CreatorID = user.UserID;
            item.DocDate = new DateTime(2012, 02, 03);
            item.DocDescription = "DocDescription";
            item.DocNumber = "DocNumber";
            item.SenderWarehouseID = warehouse.WarehouseID;
            item.RecipientWarehouseID = warehouse.WarehouseID;
            item.UserDomainID = domain.UserDomainID;
            item.TransferDocID = _transferDocId;

            var store = CreateStore();
            store.SaveTransferDoc(item);
            return item;
        }

        private void DeleteTestTransferDoc()
        {
            var store = CreateStore();
            store.DeleteTransferDoc(_transferDocId);
            DeleteTestWarehouse();
            DeleteTestUser();
        }

        [Test]
        public void TransferDocTest()
        {
            var domain = CreateTestUserDomain();
            var user = CreateTestUser();
            var warehouse = CreateTestWarehouse();
            try
            {
                var item = new TransferDoc();
                item.CreatorID = user.UserID;
                item.DocDate = new DateTime(2012, 02, 03);
                item.DocDescription = "DocDescription";
                item.DocNumber = "DocNumber";
                item.SenderWarehouseID = warehouse.WarehouseID;
                item.RecipientWarehouseID = warehouse.WarehouseID;
                item.UserDomainID = domain.UserDomainID;

                var store = CreateStore();
                store.SaveTransferDoc(item);

                var savedItem = store.GetTransferDoc(item.TransferDocID, item.UserDomainID);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.CreatorID, item.CreatorID);
                Assert.AreEqual(savedItem.DocDate, item.DocDate);
                Assert.AreEqual(savedItem.DocDescription, item.DocDescription);
                Assert.AreEqual(savedItem.DocNumber, item.DocNumber);
                Assert.AreEqual(savedItem.SenderWarehouseID, item.SenderWarehouseID);
                Assert.AreEqual(savedItem.RecipientWarehouseID, item.RecipientWarehouseID);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);
                item.DocDate = new DateTime(2011, 03, 03);
                item.DocDescription = "DocDescription3";
                item.DocNumber = "DocNumbe2r";
                store.SaveTransferDoc(item);

                savedItem = store.GetTransferDoc(item.TransferDocID, item.UserDomainID);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.CreatorID, item.CreatorID);
                Assert.AreEqual(savedItem.DocDate, item.DocDate);
                Assert.AreEqual(savedItem.DocDescription, item.DocDescription);
                Assert.AreEqual(savedItem.DocNumber, item.DocNumber);
                Assert.AreEqual(savedItem.SenderWarehouseID, item.SenderWarehouseID);
                Assert.AreEqual(savedItem.RecipientWarehouseID, item.RecipientWarehouseID);
                Assert.AreEqual(savedItem.UserDomainID, item.UserDomainID);

                store.DeleteTransferDoc(item.TransferDocID);
                savedItem = store.GetTransferDoc(item.TransferDocID, item.UserDomainID);
                Assert.IsNull(savedItem);

            }
            finally
            {
                DeleteTestWarehouse();
                DeleteTestUser();
            }
        }

        [Test]
        public void TransferDocItemTest()
        {
            var transferDoc = CreateTestTransferDoc();
            var goodsItem = CreateTestGoodsItem();
            try
            {
                var item = new TransferDocItem();
                item.TransferDocID = transferDoc.TransferDocID;
                item.GoodsItemID = goodsItem.GoodsItemID;
                item.Description = "Description";
                item.Total = 553.0M;
                var store = CreateStore();

                store.SaveTransferDocItem(item);

                var savedItem = store.GetTransferDocItem(item.TransferDocItemID, transferDoc.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.Description, item.Description);
                Assert.AreEqual(savedItem.Total, item.Total);

                item.Description = "Description2";
                item.Total = 5.0M;

                store.SaveTransferDocItem(item);

                savedItem = store.GetTransferDocItem(item.TransferDocItemID, transferDoc.UserDomainID);

                Assert.IsNotNull(savedItem);
                Assert.AreEqual(savedItem.Description, item.Description);
                Assert.AreEqual(savedItem.Total, item.Total);


                Assert.AreEqual(transferDoc.UserDomainID, store.GetTransferDocItemUserDomainID(item.TransferDocItemID));

                store.DeleteTransferDocItem(item.TransferDocItemID);

            }
            finally
            {
                DeleteTestGoodsItem();
                DeleteTestTransferDoc();
            }
        }

        [Test]
        public void ProcessedWarehouseDocTest()
        {
            var warehouse = CreateTestWarehouse();
            var store = CreateStore();
            try
            {
                var item = new ProcessedWarehouseDoc();
                item.EventDate = new DateTime(2014,06,03);
                item.UTCEventDateTime = new DateTime(2014,03,01);
                item.UserID = Guid.NewGuid();
                item.WarehouseID = warehouse.WarehouseID;
                item.ProcessedWarehouseDocID = Guid.NewGuid();

                store.SaveProcessedWarehouseDoc(item);

                var savedItem = store.GetProcessedWarehouseDoc(item.ProcessedWarehouseDocID);
                
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.EventDate, item.EventDate);
                Assert.AreEqual(savedItem.ProcessedWarehouseDocID, item.ProcessedWarehouseDocID);
                Assert.AreEqual(savedItem.UTCEventDateTime, item.UTCEventDateTime);
                Assert.AreEqual(savedItem.UserID, item.UserID);
                Assert.AreEqual(savedItem.WarehouseID, item.WarehouseID);

                store.DeleteProcessedWarehouseDoc(item.ProcessedWarehouseDocID);
                savedItem = store.GetProcessedWarehouseDoc(item.ProcessedWarehouseDocID);

                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestWarehouse();
            }
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
                store.SaveFinancialGroupMapWarehouseItem(item);

                var savedItem = store.GetFinancialGroupMapWarehouseItem(item.FinancialGroupWarehouseMapID);
                Assert.IsNotNull(savedItem);

                store.DeleteFinancialGroupWarehouseMapItem(item.FinancialGroupWarehouseMapID);
                savedItem = store.GetFinancialGroupMapWarehouseItem(item.FinancialGroupWarehouseMapID);
                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestWarehouse();
                DeleteTestFinancialGroupItem();
                DeleteTestUserDomain();
            }
        }

        [Test]
        public void RecoveryLoginItemTest()
        {
            var item = new RecoveryLoginItem();
            var store = CreateStore();

            item.IsRecovered = true;
            item.LoginName = "LoginName";
            item.RecoveredClientIdentifier = "RecoveredClientIdentifier";
            item.RecoveryClientIdentifier = "RecoveryClientIdentifier";
            item.RecoveryEmail = "RecoveryEmail";
            item.SentNumber = "SentNumber";
            item.UTCEventDate = new DateTime(2015,05,06);
            item.UTCEventDateTime = new DateTime(2015,05,07);
            item.UTCRecoveredDateTime = new DateTime(2015,05,08);

            store.SaveRecoveryLoginItem(item);

            var savedItem = store.GetRecoveryLoginItem(item.RecoveryLoginID);

            Assert.IsNotNull(savedItem);

            Assert.AreEqual(item.IsRecovered,savedItem.IsRecovered);
            Assert.AreEqual(item.LoginName,savedItem.LoginName);
            Assert.AreEqual(item.RecoveredClientIdentifier,savedItem.RecoveredClientIdentifier);
            Assert.AreEqual(item.RecoveryClientIdentifier,savedItem.RecoveryClientIdentifier);
            Assert.AreEqual(item.RecoveryEmail,savedItem.RecoveryEmail);
            Assert.AreEqual(item.SentNumber,savedItem.SentNumber);
            Assert.AreEqual(item.UTCEventDate,savedItem.UTCEventDate);
            Assert.AreEqual(item.UTCEventDateTime,savedItem.UTCEventDateTime);
            Assert.AreEqual(item.UTCRecoveredDateTime, savedItem.UTCRecoveredDateTime);

            item.IsRecovered = false;
            item.LoginName = "LoginName2";
            item.RecoveredClientIdentifier = "RecoveredClientIdentifier3";
            item.RecoveryClientIdentifier = "RecoveryClientIdentifier4";
            item.RecoveryEmail = "RecoveryEma5il";
            item.SentNumber = "SentNumbe3r";
            item.UTCEventDate = new DateTime(2015, 05, 09);
            item.UTCEventDateTime = new DateTime(2015, 05, 10);
            item.UTCRecoveredDateTime = new DateTime(2015, 05, 11);

            store.SaveRecoveryLoginItem(item);

            savedItem = store.GetRecoveryLoginItem(item.RecoveryLoginID);

            Assert.IsNotNull(savedItem);

            Assert.AreEqual(item.IsRecovered, savedItem.IsRecovered);
            Assert.AreEqual(item.LoginName, savedItem.LoginName);
            Assert.AreEqual(item.RecoveredClientIdentifier, savedItem.RecoveredClientIdentifier);
            Assert.AreEqual(item.RecoveryClientIdentifier, savedItem.RecoveryClientIdentifier);
            Assert.AreEqual(item.RecoveryEmail, savedItem.RecoveryEmail);
            Assert.AreEqual(item.SentNumber, savedItem.SentNumber);
            Assert.AreEqual(item.UTCEventDate, savedItem.UTCEventDate);
            Assert.AreEqual(item.UTCEventDateTime, savedItem.UTCEventDateTime);
            Assert.AreEqual(item.UTCRecoveredDateTime, savedItem.UTCRecoveredDateTime);

            store.DeleteRecoveryLoginItem(item.RecoveryLoginID);

            savedItem = store.GetRecoveryLoginItem(item.RecoveryLoginID);

            Assert.IsNull(savedItem);

        }

        [Test]
        public void UserPublicKeyTest()
        {
            var item = new UserPublicKey();
            var store = CreateStore();
            var user = CreateTestUser();
            try
            {
                item.ClientIdentifier = "ClientIdentifier";
                item.EventDate = new DateTime(2015,06,07);
                item.KeyNotes = "KeyNotes";
                item.Number = "Number";
                item.PublicKeyData = "item";
                item.UserID = user.UserID;
                item.IsRevoked = false;

                store.SaveUserPublicKey(item);

                var savedItem = store.GetUserPublicKey(item.UserPublicKeyID,user.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.ClientIdentifier,item.ClientIdentifier);
                Assert.AreEqual(savedItem.EventDate,item.EventDate);
                Assert.AreEqual(savedItem.KeyNotes,item.KeyNotes);
                Assert.AreEqual(savedItem.Number,item.Number);
                Assert.AreEqual(savedItem.PublicKeyData,item.PublicKeyData);
                Assert.AreEqual(savedItem.UserID, item.UserID);
                Assert.AreEqual(savedItem.UserPublicKeyID, item.UserPublicKeyID);
                Assert.AreEqual(savedItem.IsRevoked, item.IsRevoked);

                item.ClientIdentifier = "ClientIdentifier2";
                item.EventDate = new DateTime(2015, 06, 08);
                item.KeyNotes = "KeyNotes2";
                item.Number = "Number3";
                item.PublicKeyData = "ite4m";
                item.IsRevoked = true;
                

                store.SaveUserPublicKey(item);

                savedItem = store.GetUserPublicKey(item.UserPublicKeyID,user.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.ClientIdentifier, item.ClientIdentifier);
                Assert.AreEqual(savedItem.EventDate, item.EventDate);
                Assert.AreEqual(savedItem.KeyNotes, item.KeyNotes);
                Assert.AreEqual(savedItem.Number, item.Number);
                Assert.AreEqual(savedItem.PublicKeyData, item.PublicKeyData);
                Assert.AreEqual(savedItem.UserID, item.UserID);
                Assert.AreEqual(savedItem.UserPublicKeyID, item.UserPublicKeyID);
                Assert.AreEqual(savedItem.IsRevoked, item.IsRevoked);

                store.DeleteUserPublicKey(item.UserPublicKeyID);
                savedItem = store.GetUserPublicKey(item.UserPublicKeyID, user.UserDomainID);

                Assert.IsNull(savedItem);
                
            }
            finally
            {
                DeleteTestUser();
            }
            
        }

        [Test]
        public void UserPublicKeyRequestTest()
        {
            var item = new UserPublicKeyRequest();
            var store = CreateStore();
            var user = CreateTestUser();
            try
            {
                item.ClientIdentifier = "ClientIdentifier";
                item.EventDate = new DateTime(2015, 06, 07);
                item.KeyNotes = "KeyRequestNotes";
                item.Number = "Number";
                item.PublicKeyData = "item";
                item.PublicKeyData = "PublicKeyRequestData";
                item.UserID = user.UserID;

                store.SaveUserPublicKeyRequest(item);

                var savedItem = store.GetUserPublicKeyRequest(item.UserPublicKeyRequestID,user.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.ClientIdentifier, item.ClientIdentifier);
                Assert.AreEqual(savedItem.EventDate, item.EventDate);
                Assert.AreEqual(savedItem.KeyNotes, item.KeyNotes);
                Assert.AreEqual(savedItem.Number, item.Number);
                Assert.AreEqual(savedItem.PublicKeyData, item.PublicKeyData);
                Assert.AreEqual(savedItem.UserID, item.UserID);
                Assert.AreEqual(savedItem.UserPublicKeyRequestID, item.UserPublicKeyRequestID);

                item.ClientIdentifier = "ClientIdentifier2";
                item.EventDate = new DateTime(2015, 06, 08);
                item.KeyNotes = "KeyRequestNotes2";
                item.Number = "Number3";
                item.PublicKeyData = "ite4m";


                store.SaveUserPublicKeyRequest(item);

                savedItem = store.GetUserPublicKeyRequest(item.UserPublicKeyRequestID,user.UserDomainID);

                Assert.IsNotNull(savedItem);

                Assert.AreEqual(savedItem.ClientIdentifier, item.ClientIdentifier);
                Assert.AreEqual(savedItem.EventDate, item.EventDate);
                Assert.AreEqual(savedItem.KeyNotes, item.KeyNotes);
                Assert.AreEqual(savedItem.Number, item.Number);
                Assert.AreEqual(savedItem.PublicKeyData, item.PublicKeyData);
                Assert.AreEqual(savedItem.UserID, item.UserID);
                Assert.AreEqual(savedItem.UserPublicKeyRequestID, item.UserPublicKeyRequestID);

                store.DeleteUserPublicKeyRequest(item.UserPublicKeyRequestID);
                savedItem = store.GetUserPublicKeyRequest(item.UserPublicKeyRequestID,user.UserDomainID);

                Assert.IsNull(savedItem);

            }
            finally
            {
                DeleteTestUser();
            }

        }

        [Test]
        public void AutocompleteItemTest()
        {
            var domain = CreateTestUserDomain();
            try
            {
                var store = CreateStore();

                var item = new AutocompleteItem();
                item.Title = "Title";
                item.UserDomainID = domain.UserDomainID;
                item.AutocompleteKindID = AutocompleteKindSet.DeviceOptions.AutocompleteKindID;
                store.SaveAutocompleteItem(item);

                var savedItem = store.GetAutocompleteItem(item.AutocompleteItemID, domain.UserDomainID);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.Title, savedItem.Title);
                Assert.AreEqual(item.UserDomainID, savedItem.UserDomainID);
                Assert.AreEqual(item.AutocompleteKindID, savedItem.AutocompleteKindID);

                store.DeleteAutocompleteItem(savedItem.AutocompleteItemID);
                savedItem = store.GetAutocompleteItem(item.AutocompleteItemID, domain.UserDomainID);
                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestUserDomain();
            }

        }

        [Test]
        public void UserInterestTest()
        {
            var user = CreateTestUser();
            try
            {
                var store = CreateStore();

                var item = new UserInterest();
                item.Description = "Description";
                item.UserID = user.UserID;
                item.EventDate = new DateTime(2015, 01, 05);
                item.DeviceInterestKindID = InterestKindSet.Percent.InterestKindID;
                item.DeviceValue = 22.33M;
                item.WorkInterestKindID = InterestKindSet.Percent.InterestKindID;
                item.WorkValue = 66.64M;
                
                store.SaveUserInterest(item);

                var savedItem = store.GetUserInterest(item.UserInterestID, user.UserDomainID);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.UserID, savedItem.UserID);
                Assert.AreEqual(item.Description, savedItem.Description);
                Assert.AreEqual(item.DeviceInterestKindID, savedItem.DeviceInterestKindID);
                Assert.AreEqual(item.DeviceValue, savedItem.DeviceValue);
                Assert.AreEqual(item.EventDate, savedItem.EventDate);
                Assert.AreEqual(item.WorkInterestKindID, savedItem.WorkInterestKindID);
                Assert.AreEqual(item.WorkValue, savedItem.WorkValue);

                item.Description = "Descriptio22n";
                item.UserID = user.UserID;
                item.EventDate = new DateTime(2015, 02, 06);
                item.DeviceInterestKindID = InterestKindSet.Empty.InterestKindID;
                item.DeviceValue = 42.33M;
                item.WorkInterestKindID = InterestKindSet.Percent.InterestKindID;
                item.WorkValue =56.64M;

                store.SaveUserInterest(item);

                savedItem = store.GetUserInterest(item.UserInterestID, user.UserDomainID);
                Assert.IsNotNull(savedItem);

                Assert.AreEqual(item.UserID, savedItem.UserID);
                Assert.AreEqual(item.Description, savedItem.Description);
                Assert.AreEqual(item.DeviceInterestKindID, savedItem.DeviceInterestKindID);
                Assert.AreEqual(item.DeviceValue, savedItem.DeviceValue);
                Assert.AreEqual(item.EventDate, savedItem.EventDate);
                Assert.AreEqual(item.WorkInterestKindID, savedItem.WorkInterestKindID);
                Assert.AreEqual(item.WorkValue, savedItem.WorkValue);

                store.DeleteUserInterest(savedItem.UserInterestID);
                savedItem = store.GetUserInterest(item.UserInterestID, user.UserDomainID);
                Assert.IsNull(savedItem);
            }
            finally
            {
                DeleteTestUser();
            }

        }


        [Test]
        public void UserGridFilterTest()
        {
            try
            {
                var store = CreateStore();
                var item = new UserGridFilter();

                item.FilterData = "Address|Test|Other";
                item.Title = "Title";
                item.GridName = "LegalName";
                item.UserID = Guid.NewGuid();

                store.SaveUserGridFilter(item);

                var savedItem = store.GetUserGridFilter(item.UserGridFilterID);

                Assert.IsNotNull(savedItem);
                Assert.AreEqual(item.FilterData, savedItem.FilterData);
                Assert.AreEqual(item.UserGridFilterID, savedItem.UserGridFilterID);
                Assert.AreEqual(item.Title, savedItem.Title);
                Assert.AreEqual(item.GridName, savedItem.GridName);
                Assert.AreEqual(item.UserID, savedItem.UserID);

                store.DeleteUserGridFilter(item.UserGridFilterID);

                savedItem = store.GetUserGridFilter(item.UserGridFilterID);

                Assert.IsNull(savedItem);
            }
            finally
            {
                
            }
        }

        [Test]
        public void UserGridStateTest()
        {
            try
            {
                var store = CreateStore();
                var item = new UserGridState();

                item.StateGrid = "Address|Test|Other";
                item.GridName = "LegalNameTest";
                item.UserID = Guid.NewGuid();

                store.SaveUserGridState(item);

                var savedItem = store.GetUserGridState(item.UserGridStateID);

                Assert.IsNotNull(savedItem);
                Assert.AreEqual(item.StateGrid, savedItem.StateGrid);
                Assert.AreEqual(item.UserGridStateID, savedItem.UserGridStateID);
                Assert.AreEqual(item.GridName, savedItem.GridName);
                Assert.AreEqual(item.UserID, savedItem.UserID);

                store.DeleteUserGridState(item.UserGridStateID);

                savedItem = store.GetUserGridState(item.UserGridStateID);

                Assert.IsNull(savedItem);
            }
            finally
            {

            }
        }

        [Test]
        public void UserSettingsItemTest()
        {
            try
            {
                var store = CreateStore();
                var item = new UserSettingsItem();

                item.Data = "Address|Test|Other";
                item.Number = "LegalNameTest";
                item.UserLogin = "UserLogin";

                store.SaveUserSettingsItem(item);

                var savedItem = store.GetUserSettingsItem(item.UserSettingsID);

                Assert.IsNotNull(savedItem);
                Assert.AreEqual(item.Data, savedItem.Data);
                Assert.AreEqual(item.Number, savedItem.Number);
                Assert.AreEqual(item.UserLogin, savedItem.UserLogin);
                

                store.DeleteUserSettingsItem(item.UserSettingsID);

                savedItem = store.GetUserSettingsItem(item.UserSettingsID);

                Assert.IsNull(savedItem);
            }
            finally
            {

            }
        }

        [Test]
        public void UserDomainSettingsItemTest()
        {
            try
            {
                var store = CreateStore();
                var item = new UserDomainSettingsItem();

                item.Data = "Address|Test|Other";
                item.Number = "LegalNameTest";
                item.UserDomainID = Guid.NewGuid();

                store.SaveUserDomainSettingsItem(item);

                var savedItem = store.GetUserDomainSettingsItem(item.UserDomainSettingsID);

                Assert.IsNotNull(savedItem);
                Assert.AreEqual(item.Data, savedItem.Data);
                Assert.AreEqual(item.Number, savedItem.Number);
                Assert.AreEqual(item.UserDomainID, savedItem.UserDomainID);


                store.DeleteUserDomainSettingsItem(item.UserDomainSettingsID);

                savedItem = store.GetUserDomainSettingsItem(item.UserDomainSettingsID);

                Assert.IsNull(savedItem);
            }
            finally
            {

            }
        }
    }
}

