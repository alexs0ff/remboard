using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Базовый контроллер для всех котроллеров приложения.
    /// </summary>
    public class BaseController:Controller
    {
        /// <summary>
        /// Отображате контект левого меню категорий.
        /// </summary>
        [ChildActionOnly]
        public ActionResult LeftMenu()
        {
            ProjectRole role = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var token = GetToken();
                if (token!=null)
                {
                    role = ProjectRoleSet.GetKindByID(token.User.ProjectRoleID);
                }
            }
            return View("LeftMenu",role);
        }

        /// <summary>
        ///   Получает текущий маркер пользователя.
        /// </summary>
        /// <returns> Маркер. </returns>
        protected SecurityToken GetToken()
        {
            return RemontinkaServer.Instance.SecurityService.TokenManager.GetCurrentToken();
        }

        /// <summary>
        /// Проверяет текст с капчи и вводит соответсвующие ошибки в модель данных.
        /// </summary>
        /// <returns>Результат.</returns>
        protected bool VerifyRecaptcha()
        {
            var recaptchaHelper = this.GetRecaptchaVerificationHelper();

            if (string.IsNullOrWhiteSpace(recaptchaHelper.Response))
            {
                ModelState.AddModelError(string.Empty, "Отсутствует текст с изображения");
                return false;
            } //if


            var result = recaptchaHelper.VerifyRecaptchaResponse();

            if (result != RecaptchaVerificationResult.Success)
            {
                ModelState.AddModelError(string.Empty, "Введен ошибочный текст");
                return false;
            } //if

            return true;
        }

    }
}