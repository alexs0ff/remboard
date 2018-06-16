using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.Core.ServiceEntities;
using Romontinka.Server.Core.UnitOfWorks;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Helpers;
using log4net;

namespace Romontinka.Server.WebSite.Services
{
    /// <summary>
    /// Реализация системного сервиса.
    /// </summary>
    public class SystemService : ISystemService
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SystemService));

        #region Export 
        
        /// <summary>
        /// Экспортирует сущности по определенным параметрам.
        /// </summary>
        /// <param name="token">Токен безопасности. </param>
        /// <param name="exportParams">Параметры экспорта.</param>
        /// <returns>Результат.</returns>
        public byte[] Export(SecurityToken token, ExportParams exportParams)
        {
            _logger.InfoFormat("Старт экспортирования типа {0} с {1} по {2}", exportParams.Kind.KindID,
                               exportParams.BeginDate, exportParams.EndDate);

            byte[] content;

            if (exportParams.Kind == null)
            {
                return new byte[0];
            } //if

            using (var stream = new MemoryStream())
            {
                switch (exportParams.Kind.KindID)
                {
                    case 1: //ExportKindSet.OnlyOrders.KindID:
                        ExportOnlyOrders(token, exportParams, stream);
                        break;

                }

                var position = (int) stream.Position;
                stream.Seek(0, SeekOrigin.Begin);
                content = new byte[position];
                stream.Read(content, 0, position);
            } //using

            return content;
        }

        /// <summary>
        /// Производит процесс экпортирования только заказов.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="exportParams">Параметры экспортирования.</param>
        /// <param name="stream"></param>
        private void ExportOnlyOrders(SecurityToken token, ExportParams exportParams, MemoryStream stream)
        {

            var items = RemontinkaServer.Instance.EntitiesFacade.GetRepairOrders(token, exportParams.BeginDate,
                                                                                 exportParams.EndDate);

            var writer = new StreamWriter(stream, Encoding.UTF8);

            foreach (var repairOrder in items)
            {
                writer.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21}",
                    NormilizeCsvString(repairOrder.Number),
                    NormilizeCsvString(GetBranch(repairOrder.BranchID,repairOrder.BranchID).Title),
                    NormilizeCsvString(Utils.DateTimeToStringWithTime(repairOrder.EventDate)),
                    NormilizeCsvString(repairOrder.ClientFullName),
                    NormilizeCsvString(repairOrder.ClientAddress),
                    NormilizeCsvString(repairOrder.ClientPhone),
                    NormilizeCsvString(repairOrder.DeviceTitle),
                    NormilizeCsvString(repairOrder.DeviceModel),
                    NormilizeCsvString(repairOrder.DeviceTrademark),
                    NormilizeCsvString(repairOrder.Defect),
                    NormilizeCsvString(repairOrder.DeviceAppearance),
                    NormilizeCsvString(repairOrder.DeviceSN),
                    NormilizeCsvString(repairOrder.Notes),
                    NormilizeCsvString(Utils.DecimalToString(repairOrder.PrePayment)),
                    NormilizeCsvString(Utils.DecimalToString(repairOrder.GuidePrice)),
                    NormilizeCsvString(Utils.DateTimeToString(repairOrder.DateOfBeReady)),
                    NormilizeCsvString(repairOrder.WarrantyTo==null?string.Empty:Utils.DateTimeToString(repairOrder.WarrantyTo.Value)),
                    NormilizeCsvString(GetUser( repairOrder.ManagerID,repairOrder.UserDomainID).LoginName),
                    NormilizeCsvString(GetUser( repairOrder.EngineerID,repairOrder.UserDomainID).LoginName),
                    NormilizeCsvString(repairOrder.IssueDate == null ? string.Empty : Utils.DateTimeToString(repairOrder.IssueDate.Value)),
                    NormilizeCsvString(GetUser( repairOrder.IssuerID,repairOrder.UserDomainID).LoginName),
                    NormilizeCsvString(repairOrder.WarrantyTo == null ? string.Empty : Utils.DateTimeToString(repairOrder.WarrantyTo.Value))
                    );
            } //foreach


            writer.Flush();
        }

        /// <summary>
        /// Содержит хэшированые значения филиалов.
        /// </summary>
        private readonly IDictionary<Guid?, Branch> _branches = new Dictionary<Guid?, Branch>();

        /// <summary>
        /// Получает филиал.
        /// </summary>
        /// <param name="branchId">Код филиала.</param>
        /// <param name="userDomainID">Код домена. </param>
        /// <returns>Пользователь.</returns>
        public Branch GetBranch(Guid? branchId, Guid? userDomainID)
        {
            if (branchId == null)
            {
                return new Branch();
            } //if

            if (_users.ContainsKey(branchId))
            {
                return _branches[branchId];
            } //if
            else
            {
                var branch = RemontinkaServer.Instance.DataStore.GetBranch(branchId, userDomainID);
                try
                {
                    if (branch != null)
                    {
                        _branches.Add(branchId, branch);
                    } //if

                }
                catch (Exception)
                {


                } //try

                if (branch == null)
                {
                    return new Branch();
                } //if

                return branch;
            } //else
        }

        /// <summary>
        /// Содержит хэшированые значения пользователей.
        /// </summary>
        private readonly IDictionary<Guid?, User> _users = new Dictionary<Guid?, User>();

        /// <summary>
        /// Получает пользователя.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <param name="userDomainID">Код домена. </param>
        /// <returns>Пользователь.</returns>
        public User GetUser(Guid? userId, Guid? userDomainID)
        {
            if (userId == null)
            {
                return new User ();
            } //if

            if (_users.ContainsKey(userId))
            {
                return _users[userId];
            } //if
            else
            {
                var user = RemontinkaServer.Instance.DataStore.GetUser(userId, userDomainID);
                try
                {
                    if (user!=null)
                    {
                        _users.Add(userId, user);
                    } //if
                    
                }
                catch (Exception)
                {
                    
                    
                } //try

                if (user==null)
                {
                    return new User();
                } //if

                return user;
            } //else
        }

        /// <summary>
        /// Нормализация csv значения.
        /// </summary>
        private string NormilizeCsvString(string value)
        {
            if (value != null && value.Contains(";"))
            {
                return string.Concat("\"", value, "\"");
            } //if

            return value;
        }

        #endregion Export
    }
}