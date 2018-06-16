using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.UnitOfWorks;
using Romontinka.Server.WebSite.Models;
using Romontinka.Server.WebSite.Models.Account;
using log4net;

namespace Romontinka.Server.WebSite.Controllers
{
    public class AccountController : BaseController
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AccountController));

        //
        // GET: /Account/

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                if (RemontinkaServer.Instance.DataStore.UserDomainLoginIsExistsAndNonActivated(model.UserName))
                {
                    ModelState.AddModelError("", "Данный логин существует, но не активирован.");
                }
                else
                {
                    ModelState.AddModelError("", "Имя пользователя или пароль не верны.");    
                }
                
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Неверный текущий пароль или некорректный новый.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Activate(string id)
        {
            _logger.InfoFormat("Попытка активации домена \"{0}\" c ip \"{1}\"", id, HttpContext.Request.UserHostName);
            if (string.IsNullOrWhiteSpace(id))
            {
                return
                    View(new ActivateResultModel
                         {Success = false, ErrorDescription = "Код активации не может быть пустым"});
            } //if

            Guid userDomainID;

            if (!Guid.TryParse(id,out userDomainID))
            {
                return
                    View(new ActivateResultModel { Success = false, ErrorDescription = "Ошибка кода активации" });
            } //if

            try
            {

                var result = RemontinkaServer.Instance.SecurityService.Activate(userDomainID, HttpContext.Request.UserHostName);

                if (!result.Success)
                {
                    return View(new ActivateResultModel { Success = false, ErrorDescription = result.Description });
                }
                else
                {
                    return View(new ActivateResultModel { Success = true, LoginName = result.Login });
                } //else
            }
            catch (Exception ex)
            {
                var inner = string.Empty;
                if (ex.InnerException != null)
                {
                    inner = ex.InnerException.Message;
                }
                _logger.ErrorFormat("Произошла ошибка во время активации домена {0} {1} {2} {3} {4}", userDomainID, inner, ex.Message,
                                    ex.GetType(), ex.StackTrace);
                return View(new ActivateResultModel { Success = false, ErrorDescription = "Произошла ошибка активации, повторите запрос чуть позже" });
            }

        }

        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);    
            }

            if (RemontinkaServer.Instance.DataStore.UserLoginExists(model.Login))
            {
                ModelState.AddModelError("Login","Логин уже существует");
                return View(model);
            }

            if (RemontinkaServer.Instance.DataStore.UserDomainEmailIsExists(model.Email))
            {
                ModelState.AddModelError("Email", "Такой email уже есть");
                return View(model);
            }

            try
            {
                var registrationUnit = new RegistrationUnit
                                           {
                                               ClientIdentifier = HttpContext.Request.UserHostName,
                                               Email = model.Email.ToLower(),
                                               FirstName = model.FirstName,
                                               LastName = model.LastName,
                                               LegalName = model.LegalName,
                                               Login = model.Login,
                                               Password = model.Password,
                                               Trademark = model.Trademark,
                                               Address = model.Address
                                           };
                var result = RemontinkaServer.Instance.SecurityService.Register(registrationUnit);

                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Description);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                var inner = string.Empty;
                if (ex.InnerException!=null)
                {
                    inner = ex.InnerException.Message;
                }
                _logger.ErrorFormat("Произошла ошибка во время регистрации пользователя {0} {1} {2} {3} {4}",model.Login, inner, ex.Message,
                                    ex.GetType(), ex.StackTrace);
                ModelState.AddModelError(string.Empty, "Произошла ошибка регистрации, повторите запрос чуть позже");
                return View(model); 
            }

            return View("RegisterSuccess");
        }

        /// <summary>
        /// восстановление пароля
        /// </summary>
        [HttpGet]
        public ActionResult RecoveryLogin()
        {
            return View(new RecoveryLoginModel());
        }

        /// <summary>
        /// Восстановление пароля
        /// TODO: сделать асинхронным.
        /// </summary>
        [HttpPost]
        public ActionResult RecoveryLogin(RecoveryLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            } //if

            if (!VerifyRecaptcha())
            {
                return View(model);
            }

            RemontinkaServer.Instance.SecurityService.RecoveryLogin(model.Login, HttpContext.Request.UserHostAddress);
            return View("RecoveryEmailSent");
        }

        /// <summary>
        /// Форма для смены пароля
        /// </summary>
        [HttpGet]
        public ActionResult RecoveryPassword(string number)
        {
            if (RemontinkaServer.Instance.SecurityService.CheckRecoveryNumber(number))
            {
                return View(new RecoveryPasswordModel { RecoveryNumber = number});
            } //if

            return View("RecoveryFail");
        }

        /// <summary>
        /// Результаты смены пароля.
        /// </summary>
        [HttpPost]
        public ActionResult RecoveryPassword(RecoveryPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            } //if

            if (RemontinkaServer.Instance.SecurityService.RecoveryPassword(model.NewPassword,model.RecoveryNumber,Request.UserHostAddress))
            {
                return View("ChangePasswordSuccess");
            } //if

            return View("RecoveryFail");
        }
    }
}
