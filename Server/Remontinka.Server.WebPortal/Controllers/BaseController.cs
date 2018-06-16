using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.ASPxHtmlEditor.Internal;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Models.Menu;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Базовый контроллер для всех контроллеров приложения.
    /// </summary>
    public class BaseController:Controller
    {
        /// <summary>
        /// Верхнее меню.
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public Task<PartialViewResult> MainMenu()
        {
            List < MainMenuItem > list = null;

            if (Request.IsAuthenticated)
            {
                list = MenuProvider.GetMenu(GetToken());
            }
            else
            {
                list = new List<MainMenuItem>();
            }

            return Task.FromResult(PartialView("MainMenu", list));
        }


        /// <summary>
        /// Левое меню.
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult LeftPanel()
        {
            var model = new LeftMenuModel();
            return PartialView("LeftPanel", model);
        }


        public ActionResult LeftPanelEditDockManagerPartial()
        {
            return PartialView("LeftPanelEditDockManagerPartial");
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
        /// Метод указывающий необходимо ли использовать лайаут по-умолчанию.
        /// </summary>
        /// <returns>Признак использования лайаута по-умолчанию.</returns>
        public virtual bool UseDefaultLayout()
        {
            return true;
        }

        /// <summary>
        /// Возвращает Json результат.
        /// </summary>
        /// <param name="result">Результат.</param>
        /// <returns>Результат проверки.</returns>
        [NonAction]
        protected JsonResult JCrud(JCrudResult result)
        {
            return Json(result);
        }

        /// <summary>
        /// Возвращает данные.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Json format.</returns>
        [NonAction]
        protected JsonResult JCrudData(string data)
        {
            return Json(new JCrudDataResult {ResultState = CrudResultKind.Success,Data = data });
        }

        /// <summary>
        /// Возвращает данные.
        /// </summary>
        /// <param name="errorMessage">сообщение об ошибке.</param>
        /// <returns>Json format.</returns>
        [NonAction]
        protected JsonResult JCrudError(string errorMessage)
        {
            return Json(new JCrudErrorResult { ResultState = CrudResultKind.Error, Description = errorMessage });
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