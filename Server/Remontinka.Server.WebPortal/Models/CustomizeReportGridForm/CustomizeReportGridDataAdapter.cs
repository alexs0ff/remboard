using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.CustomizeReportGridForm
{
    /// <summary>
    /// Сервис данных для контроллера пользовательских отчетов.
    /// </summary>
    public class CustomizeReportGridDataAdapter : DataAdapterBase<Guid, CustomizeReportGridModel, CustomizeReportCreateModel, CustomizeReportCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override CustomizeReportGridModel CreateGridModel(SecurityToken token)
        {
            return new CustomizeReportGridModel
            {
                DocumentKinds = DocumentKindSet.Kinds
            };
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItems(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override CustomizeReportCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new CustomizeReportCreateModel
            {
                DocumentKindID = DocumentKindSet.OrderReportDocument.DocumentKindID,
                HtmlContent = "<html><body></body></html>"
            };
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override CustomizeReportCreateModel CreateEditModel(SecurityToken token, Guid? entityId, CustomizeReportGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItem(token, entityId);
            RiseExceptionIfNotFound(entity, entityId, "Документ");

            return new CustomizeReportCreateModel
            {
                Title = entity.Title,
                DocumentKindID = entity.DocumentKindID,
                CustomReportID = entity.CustomReportID,
                HtmlContent = entity.HtmlContent
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, CustomizeReportCreateModel model, GridSaveModelResult result)
        {
            RemontinkaServer.Instance.EntitiesFacade.SaveCustomReportItem(token,new CustomReportItem
            {
                Title = model.Title,
                DocumentKindID = model.DocumentKindID,
                CustomReportID = model.CustomReportID,
                HtmlContent = model.HtmlContent
            });
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, CustomizeReportCreateModel model, GridSaveModelResult result)
        {
            SaveCreateModel(token,model,result);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteCustomReportItem(token, entityId);
        }
    }
}