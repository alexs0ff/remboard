using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Remontinka.Client.Core;
using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель пункта грида заказа.
    /// </summary>
    public class RepairOrderItemModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public RepairOrderItemModel(RepairOrderDTO entity)
        {
            _order = entity;
            ClientFullName = entity.ClientFullName;
            DeviceTitle = entity.DeviceTitle;
            StatusTitle = entity.IsUrgentBoolean ? "Срочный " + entity.OrderStatusTitle : entity.OrderStatusTitle;
            ManagerFullName = entity.ManagerFullName;
            EngineerFullName = entity.EngineerFullName ?? "Не назначен";
            EventDate = WpfUtils.DateTimeToString(entity.EventDateDateTime);
            EventDateOfBeReady = WpfUtils.DateTimeToString(entity.DateOfBeReadyDateTime);
            Id = entity.RepairOrderIDGuid;
            Number = entity.Number;
            var deviceSum = ClientCore.Instance.DataStore.GetDeviceItemsSum(entity.RepairOrderIDGuid) ?? decimal.Zero;
            var workSum = ClientCore.Instance.DataStore.GetWorkItemsSum(entity.RepairOrderIDGuid) ?? decimal.Zero;
            Totals = string.Format("Общая:{0:0.00}; запчасти: {1:0.00}; работа: {2:0.00}", deviceSum + workSum,
                                   deviceSum, workSum);
        }

        /// <summary>
        /// Содержит текущий заказ.
        /// </summary>
        private readonly RepairOrderDTO _order;

        /// <summary>
        /// Содержит схему цветов согласно статусам платежей.
        /// </summary>
        private static readonly IDictionary<long?, Brush> _paymentViewBackColors = new Dictionary<long?, Brush>
                                                                                          {
                                                                                              {StatusKindSet.New.StatusKindID,Brushes.LawnGreen},
                                                                                              {StatusKindSet.OnWork.StatusKindID, Brushes.Aqua},
                                                                                              {StatusKindSet.Suspended.StatusKindID, Brushes.Magenta},
                                                                                              {StatusKindSet.Completed.StatusKindID, Brushes.Azure},
                                                                                              {StatusKindSet.Closed.StatusKindID, Brushes.Lavender},
                                                                                          };

        /// <summary>
        /// Получает цвет пункта.
        /// </summary>
        public Brush BackColor
        {
            get
            {
                Brush color = Brushes.Aqua;

                if (_order.StatusKind!=null)
                {
                    color = _paymentViewBackColors[_order.StatusKind];
                } //if

                var diff = _order.DateOfBeReadyDateTime - DateTime.Today;
                if (diff.Days < 3 && (_order.StatusKind != StatusKindSet.Completed.StatusKindID) && _order.StatusKind != StatusKindSet.Closed.StatusKindID)
                {
                    color = Brushes.Red;
                }

                return color;;
            }
        }

        /// <summary>
        /// Задает или получает номер заказа.
        /// </summary>
        public string Number { get; private set; }

        /// <summary>
        /// Задает или получает статус заказа.
        /// </summary>
        public string StatusTitle { get; private set; }

        /// <summary>
        /// Задает или получает назначеного инженера.
        /// </summary>
        public string EngineerFullName { get; private set; }

        /// <summary>
        /// Задает или получает назначеного менеджера.
        /// </summary>
        public string ManagerFullName { get; private set; }

        /// <summary>
        /// Задает или получает дату заказа.
        /// </summary>
        public string EventDate { get; private set; }

        /// <summary>
        /// Задает или получает дату готовности.
        /// </summary>
        public string EventDateOfBeReady { get; private set; }

        /// <summary>
        /// Задает или получает ФИО клиента.
        /// </summary>
        public string ClientFullName { get; private set; }

        /// <summary>
        /// Задает или получает название девайса.
        /// </summary>
        public string DeviceTitle { get; private set; }

        /// <summary>
        /// Задает или получает список общих сумм.
        /// </summary>
        public string Totals { get; private set; }

        /// <summary>
        /// Задает или получает идентификатор сущности.
        /// </summary>
        public Guid? Id { get; set; }
    }
}
