using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.GoodsItemSingleLookupForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Лукап контроллер для номенклатуры.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class GoodsItemSingleLookupController : JSingleLookupControllerBase<Guid, JLookupGoodsItemModel, JLookupGoodsItemSearchModel>
    {
        /// <summary>
        /// Получает строковой контент для отображения в поле ввода реестра.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="id">Код элемента.</param>
        /// <param name="parent">Значение связанного элемента. </param>
        /// <returns>Строковое предстовление.</returns>
        protected override string GetItemContent(SecurityToken token, Guid? id, string parent)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetGoodsItem(token, id);
            if (item == null)
            {
                return null;
            } //if

            return item.Title;
        }

        /// <summary>
        /// Переопределяется для заполнение списка пунктов для отображения лукапа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="search">Модель поиска. </param>
        /// <param name="list">Список для заполнения.</param>
        /// <param name="itemsPerPage">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        protected override void PopulateItems(SecurityToken token, JLookupGoodsItemSearchModel search, List<JLookupGoodsItemModel> list, int itemsPerPage, out int count)
        {
            foreach (var item in RemontinkaServer.Instance.EntitiesFacade.GetGoodsItems(token, search.JLookupGoodsItemName, search.Page, itemsPerPage, out count))
            {
                list.Add(new JLookupGoodsItemModel
                {
                    ItemID = item.GoodsItemID.ToString(),
                    ItemCategoryTitle = item.ItemCategoryTitle,
                    Particular = item.Particular,
                    Title = item.Title,
                    UserCode = item.UserCode
                });
            } //foreach
        }
    }
}