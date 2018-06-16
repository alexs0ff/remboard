namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для заказов.
    /// </summary>
    public class RepairOrderDTO : RepairOrder
    {
        /// <summary>
        /// Полное имя инженера.
        /// </summary>
        public string EngineerFullName { get; set; }

        /// <summary>
        /// Полное имя менеджера.
        /// </summary>
        public string ManagerFullName { get; set; }

        /// <summary>
        /// Название типа заказа.
        /// </summary>
        public string OrderKindTitle { get; set; }

        /// <summary>
        /// Задает или получает текущий статус заказа.
        /// </summary>
        public string OrderStatusTitle { get; set; }

        /// <summary>
        /// Задает или получает тип статуса заказа.
        /// </summary>
        public long? StatusKind { get; set; }

        /// <summary>
        /// Задает или получает название филиала.
        /// </summary>
        public string BranchTitle { get; set; }
    }
}
