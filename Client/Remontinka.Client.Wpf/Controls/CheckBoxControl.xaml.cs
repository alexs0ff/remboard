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
    /// Interaction logic for CheckBoxControl.xaml
    /// </summary>
    public partial class CheckBoxControl : UserControl, IControlDescriptor
    {
        public CheckBoxControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Задает или получает заголовок контрола.
        /// </summary>
        public string Title
        {
            get { return titleBox.Text; }
            set { titleBox.Text = value; }
        }

        /// <summary>
        /// Задает или получает ширину заголовка контрола.
        /// </summary>
        public GridLength TitleWidth
        {
            get { return leftCell.Width; }
            set { leftCell.Width = value; }
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
            get { return ControlType.CheckBoxControl; }
        }
    }
}
