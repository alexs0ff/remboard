﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Ajax контроллер комбобокса с контрагентами.
    /// </summary>
    public class AjaxContractorComboBoxController : AjaxComboBoxBaseController<Guid>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token, List<JSelectListItem<Guid>> list, Guid? selectedId, string parentValue)
        {
            foreach (var contractor in RemontinkaServer.Instance.EntitiesFacade.GetContractors(token))
            {
                list.Add(new JSelectListItem<Guid>
                {
                    Text = contractor.LegalName,
                    Value = contractor.ContractorID,
                    Selected = contractor.ContractorID == selectedId
                });
            } //foreach
        }
    }
}