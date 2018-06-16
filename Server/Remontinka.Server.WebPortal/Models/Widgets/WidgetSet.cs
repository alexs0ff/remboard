using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Widgets
{
    /// <summary>
    /// Набор виджетов.
    /// </summary>
    public static class WidgetSet
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        static WidgetSet()
        {
            Calendar = new WidgetItem {Id = "CalendarWidgetID",ImageName = "Calendar.png",PartialName = "CalendarWidgetPartial",Title = "Календарь",ShowOnLoad = true};
            DateTime = new WidgetItem {Id = "DateTimeWidgetID", ImageName = "DateTime.png", PartialName = "DateTimeWidgetPartial", Title = "Дата и время"};
            Widgets = new List<WidgetItem>
            {
                Calendar,
                DateTime
            };
        }

        /// <summary>
        /// Получает коллекцию доступных виджетов.
        /// </summary>
        public static ICollection<WidgetItem> Widgets { get; private set; }

        /// <summary>
        /// Получает описатель виджета - календарь.
        /// </summary>
        public static WidgetItem Calendar { get; private set; }

        /// <summary>
        /// Задает или получает описатель виджета - дата и время.
        /// </summary>
        public static WidgetItem DateTime { get; private set; }
    }
}