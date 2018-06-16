using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для документа о списании.
    /// </summary>
    public class CancellationDocDTO : CancellationDoc
    {
        /// <summary>
        /// Задает или получает имя создателя.
        /// </summary>
        public string CreatorFirstName { get; set; }

        /// <summary>
        /// Задает или получает фамилию создателя.
        /// </summary>
        public string CreatorLastName { get; set; }

        /// <summary>
        /// Задает или получает отчетство создателя.
        /// </summary>
        public string CreatorMiddleName { get; set; }

        /// <summary>
        /// Задает или получает название склада.
        /// </summary>
        public string WarehouseTitle { get; set; }

        /// <summary>
        /// Задает или получает признак обработки документа.
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// Получает ФИО создателя.
        /// </summary>
        public string CreatorFullName
        {
            get
            {
                return string.Concat(CreatorLastName, " ", CreatorFirstName, " ", CreatorMiddleName);
            }
        }
    }
}
