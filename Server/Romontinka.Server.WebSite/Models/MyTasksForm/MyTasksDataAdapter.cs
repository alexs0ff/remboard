using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Models.RepairOrderForm;

namespace Romontinka.Server.WebSite.Models.MyTasksForm
{
    /// <summary>
    /// Адаптер данных для моих задач.
    /// </summary>
    public class MyTasksDataAdapter : JGridDataAdapterBase<Guid, RepairOrderGridItemModel, RepairOrderCreateModel, RepairOrderEditModel, MyTasksSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override RepairOrderEditModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            return RepairOrderDataAdapter.CreateRepairOrderEditModel(token, entityId);
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override RepairOrderCreateModel CreateNewModel(SecurityToken token, MyTasksSearchModel searchModel)
        {
            return RepairOrderDataAdapter.RepairOrderCreateModel(token,searchModel.CopyFromRepairOrderID);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RepairOrderDataAdapter.DeleteRepairOrder(token,entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<RepairOrderGridItemModel> GetPageableGridItems(SecurityToken token, MyTasksSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetWorkRepairOrders(token, searchModel.Name,
                                                                                searchModel.Page, itemsPerPage,
                                                                                out totalCount).Select(RepairOrderDataAdapter.CreateItemModel);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override RepairOrderGridItemModel SaveCreateModel(SecurityToken token, RepairOrderCreateModel model, JGridSaveModelResult result)
        {
            return RepairOrderDataAdapter.SaveCreateRepairOrderGridItemModel(token, model,result);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override RepairOrderGridItemModel SaveEditModel(SecurityToken token, RepairOrderEditModel model, JGridSaveModelResult result)
        {
            return RepairOrderDataAdapter.SaveEditRepairOrderGridItemModel(token, model, result);
        }
    }
}