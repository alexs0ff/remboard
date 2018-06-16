using System.Web.Mvc;
using System.Web.Routing;
using Romontinka.Server.Core.Context;
using Romontinka.Server.DataLayer.Entities;
using Zen.Barcode;
using Zen.Barcode.Web.Mvc;

namespace Romontinka.Server.WebSite.Services
{
    /// <summary>
    /// Контекст для штрихкода по данным заказа.
    /// </summary>
    public class BarcodeContextItem : ContextItemBase
    {
        /// <summary>
        /// Контекст для заказа.
        /// </summary>
        /// <param name="order">Заказ.</param>
        public BarcodeContextItem(RepairOrderDTO order)
        {
            var imageTagBuilder = new TagBuilder("img");
            var url = BarcodeHtmlHelper.CreateBarcodeImageUrl(order.Number, BarcodeSymbology.Code128);
            url = url.Substring(2,url.Length-2);//Убираем ~/
            imageTagBuilder.MergeAttribute("src", url);
            imageTagBuilder.MergeAttribute("alt", order.Number);
            _values[ContextConstants.RepairOrderNumberCode128] = imageTagBuilder.ToString(TagRenderMode.SelfClosing);
        }
    }
}
