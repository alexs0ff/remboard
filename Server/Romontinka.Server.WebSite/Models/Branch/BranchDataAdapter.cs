using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.Branch
{
    /// <summary>
    /// Адаптер для управления филиалами.
    /// </summary>
    public class BranchDataAdapter : JGridDataAdapterBase<Guid, BranchGridItemModel, BranchCreateModel, BranchCreateModel, BranchSearchModel>
    {
        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override BranchCreateModel CreateNewModel(SecurityToken token, BranchSearchModel searchModel)
        {
            return new BranchCreateModel();
        }

        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override BranchCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetBranch(token, entityId);

            RiseExceptionIfNotFound(item,entityId,"Филиал");

            return new BranchCreateModel
                       {
                           Address = item.Address,
                           Id = item.BranchID,
                           Title = item.Title,
                           LegalName = item.LegalName
                       };
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<BranchGridItemModel> GetPageableGridItems(SecurityToken token, BranchSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetBranches(token,searchModel.Name,searchModel.Page,itemsPerPage,out totalCount).Select(
                i=>new BranchGridItemModel
                       {
                           Address = i.Address,
                           Id = i.BranchID,
                           Title = i.Title,
                           LegalName = i.LegalName
                       }
                );
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override BranchGridItemModel SaveCreateModel(SecurityToken token, BranchCreateModel model, JGridSaveModelResult result)
        {
            var entity = new DataLayer.Entities.Branch
                             {
                                 Address = model.Address,
                                 Title = model.Title,
                                 BranchID = model.Id,
                                 LegalName = model.LegalName
                             };
            RemontinkaServer.Instance.EntitiesFacade.SaveBranch(token,entity);
            return new BranchGridItemModel
                       {
                           Address = entity.Address,
                           Id = entity.BranchID,
                           Title = entity.Title,
                           LegalName = entity.LegalName
                       };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override BranchGridItemModel SaveEditModel(SecurityToken token, BranchCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="intityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid intityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteBranch(token,intityId);
        }
    }
}