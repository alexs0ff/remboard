using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Metadata;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Базовый класс для параметров отчета.
    /// </summary>
    public abstract class ReportParametersModelBase
    {
        /// <summary>
        /// Содержит токен безопасности.
        /// </summary>
        public SecurityToken Token { get; set; }
       

        /// <summary>
        /// Возвращает список наименований параметров.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportParameterDescriptor> GetParameters()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(ReportParameterAttribute), true);
                if (attributes.Any())
                {
                    var attribute = (ReportParameterAttribute)attributes[0];
                    var kind = ReportParameterKind.Common;
                    if (!attribute.IsHidden)
                    {
                        if (propertyInfo.PropertyType == typeof(DateTime) ||
                            propertyInfo.PropertyType == typeof(DateTime?))
                        {
                            kind = ReportParameterKind.DateTime;
                        }
                    }
                    else
                    {
                        kind = ReportParameterKind.Hidden;
                    }
                    yield return new ReportParameterDescriptor { Name = propertyInfo.Name,Kind = kind };
                }
            }
        }
    }
}