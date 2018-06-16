using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for TextBlockControl.xaml
    /// </summary>
    public partial class TextBlockControl : UserControl, IControlDescriptor
    {
        public TextBlockControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Получает заголовок контрола.
        /// </summary>
        public string Title
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Получает имя контрола.
        /// </summary>
        public string ControlName
        {
            get { return Name; }
        }

        /// <summary>
        /// Получает тип контрола.
        /// </summary>
        public ControlType ControlType
        {
            get { return ControlType.TextBlockControl; }
        }
    }
}
