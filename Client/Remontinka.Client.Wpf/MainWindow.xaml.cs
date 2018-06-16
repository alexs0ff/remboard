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

namespace Remontinka.Client.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Устанавливает на передний план контрол в центр.
        /// </summary>
        /// <param name="control">Контрол к установке.</param>
        public void SetForegroundControl(UserControl control)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(control);

            var focusable = control as IFocusable;
            if (focusable != null)
            {
                control.Loaded += (sender, args) => focusable.SetFocus();
            } //if
        }

        /// <summary>
        /// Отображает список заказов.
        /// </summary>
        private void ShowRepairOrderList(object sender, RoutedEventArgs e)
        {
            ArmController.Instance.ShowRepairOrderList();
        }

        /// <summary>
        /// Закрывает приложение.
        /// </summary>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowSyncForm(object sender, RoutedEventArgs e)
        {
            ArmController.Instance.ShowSyncForm();
        }
    }
}
