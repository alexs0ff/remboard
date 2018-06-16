using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Настраеваемый отчет.
    /// </summary>
    public class CustomReportItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код отчета.
        /// </summary>
        public Guid? CustomReportID { get; set; }

        /// <summary>
        /// Задает или получает название документа.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает тип настраеваемого документа.
        /// </summary>
        public byte? DocumentKindID { get; set; }

        /// <summary>
        /// Задает или получает контент документа.
        /// </summary>
        public string HtmlContent { get; set; }

        /// <summary>
        /// Задает или получает код домена пользователя.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (CustomReportItem) entityBase;
            entity.DocumentKindID = DocumentKindID;
            entity.HtmlContent = HtmlContent;
            entity.Title = Title;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return CustomReportID;
        }
    }
}
