using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.SystemForm;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер управления системой.
    /// </summary>
    [ExtendedAuthorize]
    public class SystemController : BaseController
    {
        /// <summary>
        /// Содержит наимнование контроллера.
        /// </summary>
        public const string ControllerName = "System";

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = UserRole.Admin)]
        public ActionResult Index()
        {
            var model = new SystemModel();

            var token = GetToken();

            var currentDomain = RemontinkaServer.Instance.DataStore.GetUserDomain(token.User.UserDomainID);

            model.RegistrationInfoModel = new RegistrationInfoModel
            {
                Address = currentDomain.Address,
                Email = currentDomain.RegistredEmail,
                LegalName = currentDomain.LegalName,
                Login = currentDomain.UserLogin,
                Trademark = currentDomain.Trademark,
                Token = token
            };

            return View(model);
        }

        /// <summary>
        /// Обновляет настроки регистрации.
        /// </summary>
        /// <param name="reristrationModel">Модель регистрации.</param>
        /// <returns></returns>
        [Authorize(Roles = UserRole.Admin)]
        public ActionResult UpdateDomainSettings(RegistrationInfoModel reristrationModel)
        {
            if (ModelState.IsValid)
            {
                var token = GetToken();
                var userDomain = new UserDomain
                {
                    Address = reristrationModel.Address,
                    LegalName = reristrationModel.LegalName,
                    Trademark = reristrationModel.Trademark,
                    RegistredEmail = reristrationModel.Email
                };
                RemontinkaServer.Instance.EntitiesFacade.UpdateUserDomain(token, userDomain);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Показывает настройки отображения интерфейса пользователей.
        /// </summary>
        /// <returns>Результат.</returns>
        [Authorize]
        public ActionResult InterfaceSettings()
        {
            var model = new UserInterfaceModel();
            model.Token = GetToken();
            model.CurrentTheme =
                RemontinkaServer.Instance.GetService<IWebSiteSettingsService>()
                    .GetDevexpressTheme(model.Token.LoginName);
            return View(model);
        }

        /// <summary>
        /// Показывает настройки отображения интерфейса пользователей.
        /// </summary>
        /// <returns>Результат.</returns>
        [HttpPost]
        public ActionResult InterfaceSettings(UserInterfaceModel model)
        {
            model.Token = GetToken();
            if (ModelState.IsValid)
            {
                RemontinkaServer.Instance.GetService<IWebSiteSettingsService>()
                    .SetDevexpressTheme(model.Token.LoginName, model.CurrentTheme);
                DevExpressHelper.Theme = model.CurrentTheme;
            }
            return View(model);
        }

        /// <summary>
        /// Редактирование левой панели.
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult LeftPanelLayoutEdit()
        {
            LeftPanelLayoutModel model = new LeftPanelLayoutModel();
            model.Token = GetToken();

            //Работаем с лайаутом из левой панели
            var data = RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().GetLeftPanelWidgetsSettings(model.Token.LoginName);
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>()
                .SetLeftPanelWidgetsSettingsTemporary(model.Token.LoginName, data);

            return PartialView("EditLeftPanelLayout", model);
        }
        
    }
}