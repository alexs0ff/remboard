using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.BranchForm
{
    /// <summary>
    /// Сервис данных для грида филиалов.
    /// </summary>
    public class BranchGridDataAdapter : DataAdapterBase<Guid, BranchGridModel, BranchCreateModel, BranchEditModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override BranchGridModel CreateGridModel(SecurityToken token)
        {
            return new BranchGridModel();
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token,string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetBranches(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override BranchCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new BranchCreateModel();
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override BranchEditModel CreateEditModel(SecurityToken token, Guid? entityId, BranchGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetBranch(token, entityId);
            RiseExceptionIfNotFound(entity, entityId, "Филиал");

            return new BranchEditModel
            {
                Title = entity.Title,
                Address = entity.Address,
                LegalName = entity.LegalName,
                BranchID = entity.BranchID
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, BranchCreateModel model, GridSaveModelResult result)
        {
            var branch = new Branch();
            branch.Title = model.Title;
            branch.Address = model.Address;
            branch.LegalName = model.LegalName;
            RemontinkaServer.Instance.EntitiesFacade.SaveBranch(token, branch);
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanBranches(token);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, BranchEditModel model, GridSaveModelResult result)
        {
            var branch = new Branch();
            branch.Title = model.Title;
            branch.Address = model.Address;
            branch.LegalName = model.LegalName;
            branch.BranchID = model.BranchID;
            RemontinkaServer.Instance.EntitiesFacade.SaveBranch(token, branch);
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanBranches(token);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteBranch(token, entityId);
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanBranches(token);
        }
    }
}