using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.User;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер пользователей.
    /// </summary>
    //[ExtendedAuthorize]
    public class UserSingleLookupController : JSingleLookupControllerBase<Guid,JLookupUserItemModel,JLookupUserSearchModel>
    {
        /// <summary>
        /// Получает строковой контент для отображения в поле ввода реестра.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="id">Код элемента.</param>
        /// <param name="parent">Значение связанного элемента.</param>
        /// <returns>Строковое предстовление.</returns>
        protected override string GetItemContent(SecurityToken token, Guid? id, string parent)
        {
            var user = new User {FirstName = "Пользователь22",UserID = id};
            if (user.UserID == null)
            {
                return null;
            } //if

            return user.ToString();
        }

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(IEnumerable<TSource> source, int page, int pageSize)
        {
            return pageSize == int.MaxValue ? source : source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Переопределяется для заполнение списка пунктов для отображения лукапа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="search">Модель поиска. </param>
        /// <param name="list">Список для заполнения.</param>
        /// <param name="itemsPerPage">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        protected override void PopulateItems(SecurityToken token, JLookupUserSearchModel search, List<JLookupUserItemModel> list, int itemsPerPage, out int count)
        {
            //var users = RemontinkaServer.Instance.EntitiesFacade.GetUsers(token, search.Name, search.Page, itemsPerPage,out count);
            var users = new List<User>();
            users.Add(new User { FirstName = "Пользователь 1",UserID = Guid.NewGuid()});
            users.Add(new User { FirstName = "Пользователь 2",UserID = Guid.NewGuid()});
            users.Add(new User { FirstName = "Пользователь 3",UserID = Guid.NewGuid()});
            users.Add(new User { FirstName = "Пользователь 4",UserID = Guid.NewGuid()});
            users.Add(new User { FirstName = "Пользователь 5",UserID = Guid.NewGuid()});
            users.Add(new User { FirstName = "Пользователь 6", UserID = Guid.NewGuid() });
            users.Add(new User { FirstName = "Пользователь 7", UserID = Guid.NewGuid() });
            users.Add(new User { FirstName = "Пользователь 8", UserID = Guid.NewGuid() });
            //users.Add(new User { FirstName = "Пользователь 9", UserID = Guid.NewGuid() });
            //users.Add(new User { FirstName = "Пользователь 10", UserID = Guid.NewGuid() });
            //users.Add(new User { FirstName = "Пользователь 11", UserID = Guid.NewGuid() });
            //users.Add(new User { FirstName = "Пользователь 12", UserID = Guid.NewGuid() });

            count = users.Count;

            foreach (var user in Page(users.OrderBy(u => u.FirstName),search.Page,itemsPerPage))
            {
                list.Add(new JLookupUserItemModel
                         {
                             FullName = user.ToString(),
                             ItemID = user.UserID.ToString()
                         });
            } //foreach
        }

        
    }
}