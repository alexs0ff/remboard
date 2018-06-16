using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.WarehouseItemSingleLookupForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер выбора остатков на складе.
    /// </summary>
    [ExtendedAuthorize]
    public class WarehouseItemSingleLookupController : JSingleLookupControllerBase<Guid, JLookupWarehouseItemModel, JLookupWarehouseItemSearchModel>
    {
        /// <summary>
        /// Переопределяется для заполнение списка пунктов для отображения лукапа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="search">Модель поиска. </param>
        /// <param name="list">Список для заполнения.</param>
        /// <param name="itemsPerPage">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        protected override void PopulateItems(SecurityToken token, JLookupWarehouseItemSearchModel search, List<JLookupWarehouseItemModel> list, int itemsPerPage, out int count)
        {
            foreach (var warehouseItem in RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItems(token, search.JLookupWarehouseItemWarehouseID,search.JLookupWarehouseItemName, search.Page, itemsPerPage, out count))
            {
                var itemCount = string.Empty;

                if (warehouseItem.DimensionKindID==DimensionKindSet.Thing.DimensionKindID)
                {
                    itemCount = Utils.IntToString((int)warehouseItem.Total);
                }
                else
                {
                    itemCount = Utils.DecimalToString(warehouseItem.Total);
                }

                list.Add(new JLookupWarehouseItemModel
                {
                    GoodsItemTitle = warehouseItem.GoodsItemTitle,
                    ItemID = warehouseItem.WarehouseItemID.ToString(),
                    ItemCount = itemCount,
                    DimensionKindTitle = DimensionKindSet.GetKindByID(warehouseItem.DimensionKindID).ShortTitle,
                    ItemPrice = Utils.DecimalToString(warehouseItem.RepairPrice)
                });
            } //foreach
        }

        /// <summary>
        /// Получает строковой контент для отображения в поле ввода реестра.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="id">Код элемента.</param>
        /// <param name="parent">Значение связанного элемента. </param>
        /// <returns>Строковое предстовление.</returns>
        protected override string GetItemContent(SecurityToken token, Guid? id, string parent)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItem(token, id);
            if (item == null)
            {
                return null;
            } //if

            return item.GoodsItemTitle;
        }
    }
}