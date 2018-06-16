using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.RepairOrderGridForm;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер для грида с заказами.
    /// </summary>
    [ExtendedAuthorize]
    public class RepairOrderGridController:GridControllerBase<Guid, RepairOrderGridModel, RepairOrderCreateModel, RepairOrderEditModel>
    {
        /// <summary>
        /// TODO переделать в DI.
        /// </summary>
        public RepairOrderGridController() : base(new RepairOrderGridDataAdapter())
        {
        }

        public const string  ControllerName = "RepairOrderGrid";

        public const string  GridName = "RepairOrdersGrid";

        /// <summary>
        /// Получает название контроллера.
        /// </summary>
        /// <returns>Название контроллера.</returns>
        protected override string GetControllerName()
        {
            return ControllerName;
        }

        /// <summary>
        /// Получает название грида.
        /// </summary>
        /// <returns>Название грида.</returns>
        protected override string GetGridName()
        {
            return GridName;
        }

        /// <summary>
        /// Получает контент для детализации.
        /// </summary>
        /// <param name="repairOrderID">Код основного заказа.</param>
        /// <returns>Результат.</returns>
        [ChildActionOnly]
        public ActionResult MasterDetailDetailPartial(Guid? repairOrderID)
        {
            var model = new RepairOrderGridDetailModel();
            model.Number = "1111";
            model.ClientFullName = "3333";
            model.RepairOrderId = repairOrderID;
            model.Documents = RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItems(GetToken(),
                    DocumentKindSet.OrderReportDocument.DocumentKindID).Select(i => new RepairOrderDocumentModel
                    {
                        CustomReportID = i.CustomReportID,
                        Title = i.Title
                    });
            return PartialView("details", model);
        }

        private readonly UserGridFilter CustomTodayFilter = new UserGridFilter
        {
            UserGridFilterID = new Guid("54E4CA32-68D8-4C58-BDA1-B09A2B70D7EB"),
            Title = "Мои задачи",
            FilterData = "[StatusKind]<=#11.06/2016#"
        };

        /// <summary>
        /// Инициализация фильтров пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="filters">Фильтры.</param>
        protected override void InitializeCustomFilters(SecurityToken token, IList<UserGridFilter> filters)
        {
            //TODO:Настроить фильтр
            //filters.Add(CustomTodayFilter);
            var filter = new UserGridFilter();
            filter.Title = "Мои задачи";
            filter.FilterData = "[StatusKind]!=5" ;
            var fieldName = string.Empty;
            if (token.User.ProjectRoleID ==ProjectRoleSet.Engineer.ProjectRoleID)
            {
                fieldName = "EngineerID";
            }
            else if (token.User.ProjectRoleID == ProjectRoleSet.Manager.ProjectRoleID)
            {
                fieldName = "ManagerID";
            }

            if (!string.IsNullOrWhiteSpace(fieldName))
            {
                filter.FilterData += string.Format(" && [{0}] ='{1}'",fieldName,token.User.UserID);
            }

            filter.UserGridFilterID = new Guid("25B04D3E-7AC2-400D-B999-2269FC0BA35B");

            filters.Add(filter);
        }
    }
}