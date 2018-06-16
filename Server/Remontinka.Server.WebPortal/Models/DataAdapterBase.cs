using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models
{
    public abstract class DataAdapterBase<TKey,TGridModel,TCreateModel,TEditModel>
        where TKey : struct
        where TGridModel: GridModelBase
        where TCreateModel: GridDataModelBase<TKey>
        where TEditModel : GridDataModelBase<TKey>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public abstract TGridModel CreateGridModel(SecurityToken token);

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public abstract IQueryable GedData(SecurityToken token,string parentId);

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public abstract TCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters);

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public abstract TEditModel CreateEditModel(SecurityToken token,TKey? entityId,TGridModel gridModel ,GridCreateParameters createParameters);

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public abstract void SaveCreateModel(SecurityToken token, TCreateModel model, GridSaveModelResult result);

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public abstract void SaveEditModel(SecurityToken token, TEditModel model, GridSaveModelResult result);

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public abstract void DeleteEntity(SecurityToken token, TKey? entityId);

        /// <summary>
        /// Вызывает исключение при не нахождении определенного элемента.
        /// </summary>
        /// <param name="instance">Элемент который необходимо проверить.</param>
        /// <param name="id">Код элемента.</param>
        /// <param name="name">Наименование элемента.</param>
        protected static void RiseExceptionIfNotFound(object instance, TKey? id, string name)
        {
            if (instance == null)
            {
                throw new Exception(string.Format("Для сущности {0} не найден id {1}", name, id));
            }
        }

        /// <summary>
        /// Переопределяется для обработки несвязанных полей для формы редактирования сущности.
        /// </summary>
        /// <param name="editModel">Модель редактирования.</param>
        /// <param name="parentId">Код родительской сущности.</param>
        /// <param name="modelState">Состояние модели.</param>
        public virtual void ProcessEditUnboundItems(TEditModel editModel, string parentId, System.Web.Mvc.ModelStateDictionary modelState)
        {
            
        }

        /// <summary>
        /// Переопределяется для обработки несвязанных полей для формы создания сущности.
        /// </summary>
        /// <param name="createModel">Модель создания.</param>
        /// <param name="parentId">Код родительской сущности.</param>
        /// <param name="modelState">Состояние модели.</param>
        public virtual void ProcessCreateUnboundItems(TCreateModel createModel, string parentId, System.Web.Mvc.ModelStateDictionary modelState)
        {

        }
    }
}