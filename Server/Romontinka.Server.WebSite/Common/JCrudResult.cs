﻿
using System;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Общий результат выполнения конструкции.
    /// </summary>
    public class JCrudResult
    {
        /// <summary>
        /// Задает или получает тип результата.
        /// </summary>
        public CrudResultKind ResultState { get; set; }

        /// <summary>
        /// Получает строковое состояние результата.
        /// </summary>
        public string Result { get { return Enum.GetName(typeof (CrudResultKind), ResultState); } }
    }
}