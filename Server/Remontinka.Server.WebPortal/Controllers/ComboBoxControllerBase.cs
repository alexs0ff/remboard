using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Базовый котроллер для комбобоксов с данными.
    /// </summary>
    public abstract class ComboBoxControllerBase:BaseController
    {
        /// <summary>
        /// Получает пункт по Id.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код.</param>
        /// <returns>Найденный пункт.</returns>
        protected abstract object GetItem(SecurityToken token,object entityId);

        /// <summary>
        /// Получает список пунктов по филитру.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="skip">Количество пропущенных пунктов.</param>
        /// <param name="take">Взять количество.</param>
        /// <param name="filter">Фильтр.</param>
        /// <returns>Результат.</returns>
        protected abstract IQueryable GetItems(SecurityToken token,int skip,int take,string filter);

        public object GetItems(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            return GetItems(GetToken(), skip, take, args.Filter);
        }

        public object GetItemByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value == null)
            {
                return null;
            }
            return GetItem(GetToken(),args.Value);

        }
    }
}