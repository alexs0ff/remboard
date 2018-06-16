using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Адаптер данных для грида.
    /// </summary>
    public abstract class JGridDataAdapterBase<TKey,TGridItemModel,TCreateModel,TEditModel, TSearchModel>
        where TKey : struct
        where TSearchModel : JGridSearchBaseModel
        where TCreateModel : JGridDataModelBase<TKey>
        where TEditModel : JGridDataModelBase<TKey>
        where TGridItemModel:JGridItemModel<TKey>
    {
        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public abstract TCreateModel CreateNewModel(SecurityToken token, TSearchModel searchModel);

        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public abstract TEditModel CreateEditedModel(SecurityToken token, TKey entityId);

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public abstract IEnumerable<TGridItemModel> GetPageableGridItems(SecurityToken token, TSearchModel searchModel,
                                                                         int itemsPerPage, out int totalCount);

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public abstract TGridItemModel SaveCreateModel(SecurityToken token, TCreateModel model, JGridSaveModelResult result);

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public abstract TGridItemModel SaveEditModel(SecurityToken token, TEditModel model, JGridSaveModelResult result);

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public abstract void DeleteEntity(SecurityToken token,TKey entityId);

        /// <summary>
        /// Вызывает исключение при не нахождении определенного элемента.
        /// </summary>
        /// <param name="instance">Элемент который необходимо проверить.</param>
        /// <param name="id">Код элемента.</param>
        /// <param name="name">Наименование элемента.</param>
        protected static void RiseExceptionIfNotFound(object instance,TKey? id,string name)
        {
            if (instance == null)
            {
                //TODO определить специальное исключение с перехватом.
                throw new Exception(string.Format("Для сущности {0} не найден id {1}", name, id));
            }
        }
    }
}