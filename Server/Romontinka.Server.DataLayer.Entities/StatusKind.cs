﻿namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Тип статусов заказов.
    /// </summary>
    public class StatusKind:EntityBase<byte>
    {
        /// <summary>
        /// Задает или получает тип статуса заказа.
        /// </summary>
        public byte? StatusKindID { get; set; }

        /// <summary>
        /// Задает или получает заголовок статуса заказа.
        /// </summary>
        public string Title { get; set; }
    }
}
