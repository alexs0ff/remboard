using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Аргументы для события смены страницы.
    /// </summary>
    public class PageChangedEventArgs:EventArgs
    {
        public PageChangedEventArgs(int page)
        {
            Page = page;
        }

        /// <summary>
        /// Получает страницу на которую необходимо произвести смену.
        /// </summary>
        public int Page { get; private set; }
    }
}
