using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Wpf.Controllers.Controls;
using Remontinka.Client.Wpf.Controls;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Интерфейс для управления пунктами выбора комбобокса.
    /// </summary>
    public interface IComboBoxItemController
    {
        /// <summary>
        /// Возвращает контроллер комбобокса.
        /// </summary>
        /// <returns>Контроллер.</returns>
        ComboBoxController GetController();

        /// <summary>
        /// Устанавливает представление комбобокса.
        /// </summary>
        /// <param name="comboBox">Объект представления.</param>
        /// <param name="allowNull">Допустимость Null значений. </param>
        /// <param name="showNullValue">Показывать ли стандартное Null значение. </param>
        void SetView(ComboBoxControl comboBox, bool allowNull, bool showNullValue);

        /// <summary>
        /// Реинициализация значений на основе выбранного пункта.
        /// </summary>
        void ReInitialize(object value);
    }
}
