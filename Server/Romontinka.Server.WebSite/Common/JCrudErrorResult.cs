﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Ошибочный результат.
    /// </summary>
    public class JCrudErrorResult : JCrudResult
    {
        public JCrudErrorResult(string description):this()
        {
            Description = description;
        }

        public JCrudErrorResult()
        {
            ResultState = CrudResultKind.Error;
        }

        /// <summary>
        /// Задает или получает описание ошибки.
        /// </summary>
        public string Description { get; set; }
    }
}