using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using DevExpress.Web.Mvc;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хелпер для пунктов лайаута.
    /// </summary>
    public static class FormLayoutItemHelper
    {
        /// <summary>
        /// Конфигурирование названия заголовка.
        /// </summary>
        /// <param name="item">Конфигурируемый пункт.</param>
        /// <param name="model">Модель данных</param>
        /// <param name="propertyLambda">Свойство для конфигурации.</param>
        public static void ConfigureHiddenItem<T>(this MVCxFormLayoutItem item, object model,Expression<Func<T>> propertyLambda)
        {
            item.ConfigureItem(model, propertyLambda);
            //item.SetNestedContent();
        }

        /// <summary>
        /// Конфигурирование названия заголовка.
        /// </summary>
        /// <param name="item">Конфигурируемый пункт.</param>
        /// <param name="model">Модель данных</param>
        /// <param name="propertyLambda">Свойство для конфигурации.</param>
        public static void ConfigureItem<T>(this MVCxFormLayoutItem item, object model,
            Expression<Func<T>> propertyLambda)
        {
            item.ConfigureItem(model, propertyLambda,false);
        }

        /// <summary>
        /// Конфигурирование названия заголовка.
        /// </summary>
        /// <param name="item">Конфигурируемый пункт.</param>
        /// <param name="model">Модель данных</param>
        /// <param name="propertyLambda">Свойство для конфигурации.</param>
        /// <param name="forceRename">Признак принудительного переназначения имени.</param>
        public static void ConfigureItem<T>(this MVCxFormLayoutItem item,object model, Expression<Func<T>> propertyLambda, bool forceRename)
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                return;
            }

            var memberName = me.Member.Name;
            item.FieldName = memberName;
            item.Name = memberName;

            if (string.IsNullOrWhiteSpace(item.Caption) || forceRename)
            {
                var displayName =
                    model.GetType().GetPropertyAttributeValue((DisplayNameAttribute dna) => dna.DisplayName, memberName);

                if (!string.IsNullOrWhiteSpace(displayName))
                {
                    item.Caption = displayName;
                }
            }

        }
    }
}