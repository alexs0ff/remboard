using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.SystemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления системой.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class SystemController:BaseController
    {
        /// <summary>
        /// Содержит наимнование контроллера.
        /// </summary>
        public const string ControllerName = "System";

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
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
                                              Trademark = currentDomain.Trademark
                                          };

            model.ExportModel = new ExportModel
                                {
                                    BeginDate = DateTime.Today,
                                    EndDate = DateTime.Today
                                };

            return View(model);
        }
    }
}