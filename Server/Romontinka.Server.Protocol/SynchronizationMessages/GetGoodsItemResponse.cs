using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ на запрос о номенклатуре и категориях товаров.
    /// </summary>
    public class GetGoodsItemResponse : MessageBase
    {
        public GetGoodsItemResponse()
        {
            Kind = MessageKind.GetGoodsItemResponse;
            ItemCategories = new List<ItemCategoryDTO>();
            GoodsItems = new List<GoodsItemDTO>();
        }

        /// <summary>
        /// Получает список категорий товаров.
        /// </summary>
        public IList<ItemCategoryDTO> ItemCategories { get; private set; }

        /// <summary>
        /// Получает список номенклатуры товаров.
        /// </summary>
        public IList<GoodsItemDTO> GoodsItems { get; private set; }
    }
}
