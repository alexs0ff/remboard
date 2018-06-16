using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.ContractorGridForm
{
    /// <summary>
    /// Модель данных грида контрагентов.
    /// </summary>
    public class ContractorCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код контрагента.
        /// </summary>
        public Guid? ContractorID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return ContractorID;
        }

        /// <summary>
        /// Задает или получает юрназвание.
        /// </summary>
        [DisplayName("Юр наименование")]
        [Required]
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку.
        /// </summary>
        [DisplayName("Торговая марка")]
        [Required]
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает адрес.
        /// </summary>
        [DisplayName("Адрес")]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает дату заведения.
        /// </summary>
        [DisplayName("Телефон")]
        [Mask("+0(000) 000-0000", IncludeLiterals = MaskIncludeLiteralsMode.None, ErrorMessage = "Ошибочный телефонный номер")]
        public string Phone { get; set; }

        /// <summary>
        /// Задает или получает дату заведения.
        /// </summary>
        [DisplayName("Дата заведения")]
        [Required]
        public DateTime EventDate { get; set; }
    }
}