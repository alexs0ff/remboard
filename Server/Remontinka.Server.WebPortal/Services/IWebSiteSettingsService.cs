using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Services
{
    /// <summary>
    /// Сервис сохранения настроек вэб сайта.
    /// </summary>
    public interface IWebSiteSettingsService
    {
        /// <summary>
        /// Устанавливает тему devexpress.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <param name="theme">Тема.</param>
        void SetDevexpressTheme(string userLogin,string theme);

        /// <summary>
        /// Получает текущую тему для пользователей devexpress.
        /// </summary>
        /// <param name="userLogin">Имя логина.</param>
        /// <returns>Имя темы.</returns>
        string GetDevexpressTheme(string userLogin);

        /// <summary>
        /// Получает название темы по-умолчанию для Devexpress.
        /// </summary>
        /// <returns>Название темы.</returns>
        string GetDevexpressThemeDefault();

        /// <summary>
        /// Получает из базы временные настройки левой панели.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <returns>Настройки лайаута.</returns>
        string GetLeftPanelWidgetsSettingsTemporary(string userLogin);

        /// <summary>
        /// Сохранение временных настроек левой панели виджетов в базу.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <param name="data">Данные лайаута.</param>
        void SetLeftPanelWidgetsSettingsTemporary(string userLogin,string data);

        /// <summary>
        /// Получает из базы постоянные настройки левой панели.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <returns>Настройки лайаута.</returns>
        string GetLeftPanelWidgetsSettings(string userLogin);

        /// <summary>
        /// Сохранение постоянных настроек левой панели виджетов в базу.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <param name="data">Данные лайаута.</param>
        void SetLeftPanelWidgetsSettings(string userLogin, string data);

        /// <summary>
        /// Получает токен для пользователя.
        /// </summary>
        /// <param name="loginName">Имя логина.</param>
        /// <returns>Токен.</returns>
        SecurityToken GetTokenForUser(string loginName);

        /// <summary>
        /// Получает список пользователей для определенного токена и роли.
        /// </summary>
        /// <param name="token">Токен пользователя.</param>
        /// <param name="projectRoleKindID">Роль в проекте.</param>
        /// <returns>Список пользователей.</returns>
        List<SelectListItem<Guid>> GetUserList(SecurityToken token, byte projectRoleKindID);

        /// <summary>
        /// Очищает пользовательские списки по ролям.
        /// </summary>
        /// <param name="token">Токен.</param>
        void CleanUserLists(SecurityToken token);

        /// <summary>
        /// Очищает кэш для токена пользователя.
        /// </summary>
        /// <param name="loginName">Имя логина.</param>
        void CleanTokenForUser(string loginName);

        /// <summary>
        /// Очищает в кеше списки филиалов.
        /// </summary>
        /// <param name="token">Токен.</param>
        void CleanBranches(SecurityToken token);

        /// <summary>
        /// Получение списка филиалов для домена.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список филиалов.</returns>
        IEnumerable<Branch> GetBranches(SecurityToken token);

        /// <summary>
        /// Очищает в кеше списки типов заказа.
        /// </summary>
        /// <param name="token">Токен.</param>
        void CleanOrderKinds(SecurityToken token);

        /// <summary>
        /// Получение списка типов заказа для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список типов зказака.</returns>
        IEnumerable<OrderKind> GetOrderKinds(SecurityToken token);

        /// <summary>
        /// Получение списка статусов заказа для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список статусов заказа.</returns>
        IEnumerable<OrderStatus> GetOrderStatuses(SecurityToken token);

        /// <summary>
        /// Очищает в кеше списки статусов заказа.
        /// </summary>
        /// <param name="token">Токен.</param>
        void CleanOrderStatuses(SecurityToken token);
    }
}