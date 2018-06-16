using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хелпер для применения автодополнений.
    /// </summary>
    public static class AutocompleteHelper
    {
        /// <summary>
        /// Устанавливает для токен бокса автокомплит.
        /// </summary>
        /// <param name="settings">Настройки токен бокса.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="kind">Тип пункта автодополнения.</param>
        public static void SetAutocomplete(this TokenBoxSettings settings, SecurityToken token, AutocompleteKind kind)
        {
            settings.Properties.AllowCustomTokens = true;
            settings.Properties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            settings.Properties.ShowDropDownOnFocus = ShowDropDownOnFocusMode.Always;
            settings.Properties.DataSource = RemontinkaServer.Instance.EntitiesFacade.GetAutocompleteItems(token, kind.
                AutocompleteKindID).Select(
                i => i.Title).ToList();
        }

        /// <summary>
        /// Разделители токенов.
        /// </summary>
        private static readonly char[] TokenSeparators = {';', ','};

        /// <summary>
        /// Соединяет массив автодополнений в единую строку.
        /// </summary>
        /// <param name="field">Массив.</param>
        /// <returns>Соединенная строка.</returns>
        public static string JoinField(string[] field)
        {
            var result = string.Empty;
            if (field != null)
            {
                result = string.Join(TokenSeparators[0].ToString(), field);
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Разъединяет строку автодополнений в массив.
        /// </summary>
        /// <param name="field">Строка.</param>
        /// <returns>Массив.</returns>
        public static string[] SplitField(string field)
        {
            string[] result;

            if (!string.IsNullOrWhiteSpace(field))
            {
                result = field.Split(TokenSeparators);
            }
            else
            {
                result = null;
            }

            return result;
        }
    }
}