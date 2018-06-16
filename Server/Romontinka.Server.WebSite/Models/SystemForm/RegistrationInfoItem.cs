﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.SystemForm
{
    /// <summary>
    /// Пункт редактируемой регистрации
    /// </summary>
    public class RegistrationInfoItem
    {
        /// <summary>
        /// Задает или получает email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Задает или получает юрназвание фирмы.
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговое наименование фирмы.
        /// </summary>
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает адрес фирмы.
        /// </summary>
        public string Address { get; set; }
    }
}