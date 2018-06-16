using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Набор типов автодополнений.
    /// </summary>
    public static class AutocompleteKindSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static AutocompleteKindSet()
        {
            DeviceTrademark = new AutocompleteKind {AutocompleteKindID = 1, Title = "Бренд"};
            DeviceOptions = new AutocompleteKind { AutocompleteKindID = 2, Title = "Комплектация" };
            DeviceAppearance = new AutocompleteKind { AutocompleteKindID = 3, Title = "Внешний вид" };
            Kinds = new[] { DeviceTrademark, DeviceOptions, DeviceAppearance };
        }

        /// <summary>
        ///   Список всех типов автодополнений.
        /// </summary>
        public static ICollection<AutocompleteKind> Kinds { get; private set; }

        /// <summary>
        /// Бренд устройства.
        /// </summary>
        public static AutocompleteKind DeviceTrademark { get; private set; }

        /// <summary>
        /// Комплектация.
        /// </summary>
        public static AutocompleteKind DeviceOptions { get; private set; }

        /// <summary>
        /// Внешний вид устройства.
        /// </summary>
        public static AutocompleteKind DeviceAppearance { get; private set; }

        /// <summary>
        ///   Возвращает тип автодополнения по его коду.
        /// </summary>
        /// <returns> Тип автодополнения или null. </returns>
        public static AutocompleteKind GetKindByID(byte? id)
        {
            return Kinds.FirstOrDefault(a => a.AutocompleteKindID == id);
        }
    }
}
