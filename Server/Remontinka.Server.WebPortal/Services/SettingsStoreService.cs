using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using log4net;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Services;

namespace Remontinka.Server.WebPortal.Services
{
    /// <summary>
    /// Сервис данных для хранения настроек.
    /// </summary>
    public class SettingsStoreService: IWebSiteSettingsService
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SettingsStoreService));

        /// <summary>
        /// Создает ключ в кэше для определенного пользователя.
        /// </summary>
        /// <param name="key">Код ключа</param>
        /// <param name="loginName">Имя логина</param>
        /// <returns>Ключ в кэше.</returns>
        private string MakeCacheId(string key, string loginName)
        {
            return key + loginName;
        }

        #region DevExpress Theme

        /// <summary>
        /// Тема по умолчанию для контролов devexpress.
        /// </summary>
        private const string DefaultDevexpressTheme = "Default";

        /// <summary>
        /// Содержит имя настройки темы devexpress.
        /// </summary>
        private const string DevexpressThemeSettingsKey = "UserDevexpressThemeName";

        /// <summary>
        /// Создает ключ в кэше c настройками текущей темы для определенного пользователя.
        /// </summary>
        /// <param name="loginName">Имя логина</param>
        /// <returns>Ключ в кэше.</returns>
        private string MakeDevexpressThemeCacheId( string loginName)
        {
            return MakeCacheId(DevexpressThemeSettingsKey, loginName);
        }

        /// <summary>
        /// Получает название темы по-умолчанию для Devexpress.
        /// </summary>
        /// <returns>Название темы.</returns>
        public string GetDevexpressThemeDefault()
        {
            return DefaultDevexpressTheme;
        }

        /// <summary>
        /// Получает текущую тему для пользователей devexpress.
        /// </summary>
        /// <param name="userLogin">Имя логина.</param>
        /// <returns>Имя темы.</returns>
        public string GetDevexpressTheme(string userLogin)
        {
            var id = MakeDevexpressThemeCacheId(userLogin);

            string theme = HttpContext.Current.Cache[id] as string;

            if (string.IsNullOrWhiteSpace(theme))
            {
                _logger.InfoFormat("Получаем тему devexpress для пользователя из базы {0}", userLogin);

                theme = RemontinkaServer.Instance.DataStore.GetUserSettingsItem(userLogin, DevexpressThemeSettingsKey);

                if (string.IsNullOrWhiteSpace(theme))
                {
                    theme = DefaultDevexpressTheme;
                }

                HttpContext.Current.Cache[id] = theme;
            }

            return theme;
        }

        /// <summary>
        /// Устанавливает тему devexpress.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <param name="theme">Тема.</param>
        public void SetDevexpressTheme(string userLogin,string theme)
        {
           _logger.InfoFormat("Устанавливаем тему devexpress Для пользователя {0}:{1}", userLogin, theme);
            var id = MakeDevexpressThemeCacheId(userLogin);
            HttpContext.Current.Cache[id] = theme;
            RemontinkaServer.Instance.DataStore.SaveUserSettingsItem(userLogin, DevexpressThemeSettingsKey, theme);
        }

        #endregion DevExpress Theme

        #region LeftPanel widgets

        /// <summary>
        /// Содержит имя данных лайаута по предварительным настройкам левой панели.
        /// </summary>
        private const string LeftPanelWidgetsSettingsTemporaryKey = "LeftPanelWidgetsSettingsTemporary";

        /// <summary>
        /// Содержит имя данных хранимых настройках левой панели.
        /// </summary>
        private const string LeftPanelWidgetsSettingsKey = "LeftPanelWidgetsSettings";

        /// <summary>
        /// Получает из базы временные настройки левой панели.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <returns>Настройки лайаута.</returns>
        public string GetLeftPanelWidgetsSettingsTemporary(string userLogin)
        {
            _logger.InfoFormat("Получаем временные настройки для левой панели из базы {0}", userLogin);

            return RemontinkaServer.Instance.DataStore.GetUserSettingsItem(userLogin,
                LeftPanelWidgetsSettingsTemporaryKey);
        }

        /// <summary>
        /// Сохранение временных настроек левой панели виджетов в базу.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <param name="data">Данные лайаута.</param>
        public void SetLeftPanelWidgetsSettingsTemporary(string userLogin,string data)
        {
            _logger.InfoFormat("Сохраняем временные настройки для левой панели в базу {0}", userLogin);
            RemontinkaServer.Instance.DataStore.SaveUserSettingsItem(userLogin, LeftPanelWidgetsSettingsTemporaryKey, data);
        }

        /// <summary>
        /// Получает из базы постоянные настройки левой панели.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <returns>Настройки лайаута.</returns>
        public string GetLeftPanelWidgetsSettings(string userLogin)
        {
            var id = MakeCacheId(LeftPanelWidgetsSettingsKey, userLogin);

            string data = HttpContext.Current.Cache[id] as string;

            if (string.IsNullOrWhiteSpace(data))
            {
                _logger.InfoFormat("Получаем настройки для левой панели из базы {0}", userLogin);

                data = RemontinkaServer.Instance.DataStore.GetUserSettingsItem(userLogin, LeftPanelWidgetsSettingsKey);
                if (!string.IsNullOrWhiteSpace(data))
                {
                    HttpContext.Current.Cache[id] = data;
                }
            }

            return data;
        }

        /// <summary>
        /// Сохранение постоянных настроек левой панели виджетов в базу.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <param name="data">Данные лайаута.</param>
        public void SetLeftPanelWidgetsSettings(string userLogin, string data)
        {
            _logger.InfoFormat("Сохраняем постоянные настройки для левой панели в базу {0}", userLogin);
            var id = MakeCacheId(LeftPanelWidgetsSettingsKey, userLogin);
            HttpContext.Current.Cache[id] = data;
            RemontinkaServer.Instance.DataStore.SaveUserSettingsItem(userLogin, LeftPanelWidgetsSettingsKey, data);
        }


        #endregion LeftPanel widgets

        #region Security Token

        private const string TokenKeyPrefix = "UserTokenNameKey";

        /// <summary>
        /// Получает токен для пользователя.
        /// </summary>
        /// <param name="loginName">Имя логина.</param>
        /// <returns>Токен.</returns>
        public SecurityToken GetTokenForUser(string loginName)
        {
            if (string.IsNullOrWhiteSpace(loginName))
            {
                throw new Exception("Пользователь с пустым логином существовать не может");
            }
            var id = TokenKeyPrefix + loginName;
            SecurityToken token = HttpContext.Current.Cache[id] as SecurityToken;

            if (token == null)
            {
                _logger.InfoFormat("Получение токена пользователя {0}",loginName);
                token = RemontinkaServer.Instance.SecurityService.GetToken(loginName);
                HttpContext.Current.Cache[id] = token;
            }

            return token;
        }

        /// <summary>
        /// Очищает кэш для токена пользователя.
        /// </summary>
        /// <param name="loginName">Имя логина.</param>
        public void CleanTokenForUser(string loginName)
        {
            var id = TokenKeyPrefix + loginName;
            HttpContext.Current.Cache.Remove(id);
        }

        #endregion Security Token

        #region UserList

        private const string ProjectUserRoleListKeyFormat = "ProjectUserRoleList_{0}_{1}";

        /// <summary>
        /// Получает список пользователей для определенного токена и роли.
        /// </summary>
        /// <param name="token">Токен пользователя.</param>
        /// <param name="projectRoleKindID">Роль в проекте.</param>
        /// <returns>Список пользователей.</returns>
        public List<SelectListItem<Guid>> GetUserList(SecurityToken token, byte projectRoleKindID)
        {
            var id = string.Format(ProjectUserRoleListKeyFormat,
                projectRoleKindID.ToString(CultureInfo.InvariantCulture), token.User.UserDomainID);

            List<SelectListItem<Guid>> list = HttpContext.Current.Cache[id] as List<SelectListItem<Guid>>;

            if (list==null)
            {
                _logger.InfoFormat("Получение пользователей определенной роли для домена {0} {1}",projectRoleKindID,token.User.UserDomainID);
                list = UserHelper.GetUserList(token, projectRoleKindID);
                HttpContext.Current.Cache[id] = list;
            }

            return list;
        }

        /// <summary>
        /// Очищает пользовательские списки по ролям.
        /// </summary>
        /// <param name="token">Токен.</param>
        public void CleanUserLists(SecurityToken token)
        {
            _logger.InfoFormat("Очищение списков всех ролей для домена {0}", token.User.UserDomainID);

            foreach (var projectRole in ProjectRoleSet.Roles)
            {
                var id = string.Format(ProjectUserRoleListKeyFormat,
                projectRole.ProjectRoleID.Value.ToString(CultureInfo.InvariantCulture), token.User.UserDomainID);

                HttpContext.Current.Cache.Remove(id);
            }
        }

        #endregion UserList

        #region Branches

        private const string BranchListDomainKeyFormat = "BranchListDomainKey_{0}";

        /// <summary>
        /// Получение списка филиалов для домена.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список филиалов.</returns>
        public IEnumerable<Branch> GetBranches(SecurityToken token)
        {
            var id = string.Format(BranchListDomainKeyFormat, token.User.UserID);

            IEnumerable<Branch> list = HttpContext.Current.Cache[id] as IEnumerable<Branch>;

            if (list == null)
            {
                _logger.InfoFormat("Получение списков филиалов в кэш для пользователя {0}", token.User.UserID);
                list = RemontinkaServer.Instance.EntitiesFacade.GetBranches(token).ToList();
                HttpContext.Current.Cache[id] = list;
            }

            return list;
        }

        /// <summary>
        /// Очищает в кеше списки филиалов.
        /// </summary>
        /// <param name="token">Токен.</param>
        public void CleanBranches(SecurityToken token)
        {
            _logger.InfoFormat("Очищение списка всех филифалов для домена {0}", token.User.UserDomainID);

            foreach (var user in RemontinkaServer.Instance.DataStore.GetUsers(token.User.UserDomainID))
            {
                var id = string.Format(BranchListDomainKeyFormat, user.UserID);
                HttpContext.Current.Cache.Remove(id);
            }
            
        }

        #endregion Branches

        #region OrderKinds

        private const string OrderKindListDomainKeyFormat = "OrderKindListDomainKey_{0}";

        /// <summary>
        /// Получение списка типов заказа для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список типов заказа.</returns>
        public IEnumerable<OrderKind> GetOrderKinds(SecurityToken token)
        {
            var id = string.Format(OrderKindListDomainKeyFormat, token.User.UserID);

            IEnumerable<OrderKind> list = HttpContext.Current.Cache[id] as IEnumerable<OrderKind>;

            if (list == null)
            {
                _logger.InfoFormat("Получение списка типов заказа в кэш для пользователя {0}", token.User.UserID);
                list = RemontinkaServer.Instance.EntitiesFacade.GetOrderKinds(token).ToList();
                HttpContext.Current.Cache[id] = list;
            }

            return list;
        }

        /// <summary>
        /// Очищает в кеше списки типов заказа.
        /// </summary>
        /// <param name="token">Токен.</param>
        public void CleanOrderKinds(SecurityToken token)
        {
            _logger.InfoFormat("Очищение списков всех типов заказа для домена {0}", token.User.UserDomainID);

            foreach (var user in RemontinkaServer.Instance.DataStore.GetUsers(token.User.UserDomainID))
            {
                var id = string.Format(OrderKindListDomainKeyFormat, user.UserID);
                HttpContext.Current.Cache.Remove(id);
            }

        }

        #endregion OrderKinds

        #region OrderStatuses

        private const string OrderStatusListDomainKeyFormat = "OrderStatusListDomainKey_{0}";

        /// <summary>
        /// Получение списка статусов заказа для пользователя.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Список статусов заказа.</returns>
        public IEnumerable<OrderStatus> GetOrderStatuses(SecurityToken token)
        {
            var id = string.Format(OrderStatusListDomainKeyFormat, token.User.UserID);

            IEnumerable<OrderStatus> list = HttpContext.Current.Cache[id] as IEnumerable<OrderStatus>;

            if (list == null)
            {
                _logger.InfoFormat("Получение списка статусов заказа в кэш для пользователя {0}", token.User.UserID);
                list = RemontinkaServer.Instance.EntitiesFacade.GetOrderStatuses(token).ToList();
                HttpContext.Current.Cache[id] = list;
            }

            return list;
        }

        /// <summary>
        /// Очищает в кеше списки статусов заказа.
        /// </summary>
        /// <param name="token">Токен.</param>
        public void CleanOrderStatuses(SecurityToken token)
        {
            _logger.InfoFormat("Очищение списков всех статусов заказа для домена {0}", token.User.UserDomainID);

            foreach (var user in RemontinkaServer.Instance.DataStore.GetUsers(token.User.UserDomainID))
            {
                var id = string.Format(OrderStatusListDomainKeyFormat, user.UserID);
                HttpContext.Current.Cache.Remove(id);
            }

        }

        #endregion OrderStatuses
    }
}