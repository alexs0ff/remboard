using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Базовый контроллер для переопределения асинхронных контроллеров.
    /// </summary>
    public class BaseAsyncController : AsyncController
    {
        /// <summary>
        /// Отображате контект левого меню категорий.
        /// </summary>
        [ChildActionOnly]
        public Task<PartialViewResult> LeftMenu()
        {

            var userName = string.Empty;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userName = HttpContext.User.Identity.Name;
            } //if
            return
                Task.Factory.StartNew(() =>
                                      {
                                          ProjectRole role = null;
                                          var token = GetToken(userName);
                                          if (token != null)
                                          {
                                              role = ProjectRoleSet.GetKindByID(token.User.ProjectRoleID);
                                          }
                                          return role;
                                      }).ContinueWith(
                    t =>
                    {
                        return PartialView("LeftMenu", t.Result);
                    }
                    );
        }

        /// <summary>
        ///  Получает текущий маркер пользователя.
        /// </summary>
        /// <param name="name">Имя текущего принципиала.</param>
        /// <returns></returns>
        protected SecurityToken GetToken(string name)
        {
            return RemontinkaServer.Instance.SecurityService.TokenManager.GetCurrentToken(name);
        }
    }
}