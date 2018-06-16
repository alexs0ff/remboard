using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель события заказа.
    /// </summary>
    public class OrderTimelineModel
    {
        public OrderTimelineModel(OrderTimeline orderTimeline)
        {
            EventDate = WpfUtils.DateTimeToString(orderTimeline.EventDateTimeDateTime);
            EventDateTime = WpfUtils.DateTimeToStringWithTime(orderTimeline.EventDateTimeDateTime);
            Title = orderTimeline.Title;
        }

        /// <summary>
        /// Задает или получает дату события.
        /// </summary>
        public string EventDate { get; set; }

        /// <summary>
        /// Задает или получает дату события со временем.
        /// </summary>
        public string EventDateTime { get; set; }

        /// <summary>
        /// Задает или получает описание события.
        /// </summary>
        public string Title { get; set; }
    }
}
