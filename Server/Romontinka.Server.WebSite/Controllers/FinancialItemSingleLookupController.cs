using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.FinancialItemSingleLookupForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Луккап контроллер для поиска бюджетных статей.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class FinancialItemSingleLookupController : JSingleLookupControllerBase<Guid, JLookupFinancialItemModel, JLookupFinancialItemSearchModel>
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
            var item = RemontinkaServer.Instance.EntitiesFacade.GetFinancialItem(token, id);
            if (item==null)
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
        protected override void PopulateItems(SecurityToken token, JLookupFinancialItemSearchModel search, List<JLookupFinancialItemModel> list, int itemsPerPage, out int count)
        {
            foreach (var financialItem in RemontinkaServer.Instance.EntitiesFacade.GetFinancialItems(token,search.JLookupFinancialItemName,search.Page,itemsPerPage,out count))
            {
                list.Add(new JLookupFinancialItemModel
                         {
                             ItemID = financialItem.FinancialItemID.ToString(),
                             FinancialItemTitle = financialItem.Title,
                             TransactionKindTitle = TransactionKindSet.GetKindByID(financialItem.TransactionKindID).Title
                         });
            } //foreach
        }
    }
}