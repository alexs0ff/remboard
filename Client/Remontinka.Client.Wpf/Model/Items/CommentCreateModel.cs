using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Wpf.Model.Controls;
using Remontinka.Client.Wpf.Model.Forms;

namespace Remontinka.Client.Wpf.Model.Items
{
    /// <summary>
    /// Модель создания комментария.
    /// </summary>
    public class CommentCreateModel : ViewModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        [ModelData]
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает название комментария.
        /// </summary>
        [DisplayName("Комментарий")]
        [RegexValue(Regex = ModelConstants.RequiredRegex)]
        public string CommentTitle { get; set; }
    }
}
