using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Настраеваемый отчет.
    /// </summary>
    public class CustomReportItem : EntityBase<Guid>
    {
        private string _customReportID;

        /// <summary>
        /// Задает или получает код отчета.
        /// </summary>
        public string CustomReportID
        {
            get { return _customReportID; }
            set { FormatUtils.ExchangeFields(ref _customReportIDGuid, ref _customReportID, value); ; }
        }

        private Guid? _customReportIDGuid;

        /// <summary>
        /// Задает или получает код отчета.
        /// </summary>
        public Guid? CustomReportIDGuid
        {
            get { return _customReportIDGuid; }
            set { FormatUtils.ExchangeFields(ref _customReportIDGuid, ref _customReportID, value); }
        }

        /// <summary>
        /// Задает или получает название документа.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает тип настраеваемого документа.
        /// </summary>
        public long? DocumentKindID { get; set; }

        /// <summary>
        /// Задает или получает контент документа.
        /// </summary>
        public string HtmlContent { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (CustomReportItem)entityBase;
            entity.DocumentKindID = DocumentKindID;
            entity.HtmlContent = HtmlContent;
            entity.Title = Title;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return CustomReportIDGuid;
        }
    }
}
