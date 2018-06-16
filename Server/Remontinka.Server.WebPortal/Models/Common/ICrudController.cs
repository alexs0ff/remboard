using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Интерфейс для поддержки методов взаимодействия с системой crud.
    /// </summary>
    public interface ICrudController
    {
        /// <summary>
        /// Создает модель, если старая не опредлена.
        /// </summary>
        /// <param name="dependedModel">Зависимая модель.</param>
        /// <param name="gridModel">Новая модель данных.</param>
        /// <returns>Созданная модель.</returns>
        object CreateNewModel(object dependedModel, object gridModel);

        /// <summary>
        /// Создает модель данных для передачи в представление создания сущности.
        /// </summary>
        /// <param name="formLayoutSettings">Настройки лайаута devexpress.</param>
        /// <param name="dependedModel">Зависимая модель.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="html">Текущий Html хелпер.</param>
        /// <returns>Модель.</returns>
        object CreateNewEditSettingsModel(object formLayoutSettings, object dependedModel, object gridModel, object html);

        /// <summary>
        /// Создает модель данных для передачи в представление обновления сущности.
        /// </summary>
        /// <param name="formLayoutSettings">Настройки лайаута devexpress.</param>
        /// <param name="dependedModel">Зависимая модель.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="html">Текущий Html хелпер.</param>
        /// <returns>Модель.</returns>
        object CreateUpdateEditSettingsModel(object formLayoutSettings, object dependedModel, object gridModel, object html);

        /// <summary>
        /// Подготовливает модель грида для упаковки в состояние представления.
        /// </summary>
        /// <param name="dependedModel">Зависимая модель.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <returns>Подготовленая модель.</returns>
        object PrepareUpdateEditModel(object dependedModel, object gridModel);

        /// <summary>
        /// Возвращает название представления для создания сущности.
        /// </summary>
        /// <returns>Название представления.</returns>
        string GetCreateViewName();

        /// <summary>
        /// Возвращает название представления для редактирования сущности.
        /// </summary>
        /// <returns>Название представления.</returns>
        string GetUpdateViewName();
    }
}