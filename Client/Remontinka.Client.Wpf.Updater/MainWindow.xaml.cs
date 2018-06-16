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

namespace Remontinka.Client.Wpf.Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
        }

        private readonly RemboardUpdater _updater = new RemboardUpdater();

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var model = new MainModel();
            DataContext = model;
            _updater.StartUpdate(model);
            _updater.NeedExit += UpdaterNeedExit;
        }

        void UpdaterNeedExit(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(Application.Current.Shutdown));
                return;
            } //if
            Application.Current.Shutdown();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StartOfflineModeButtonClick(object sender, RoutedEventArgs e)
        {
            if(!_updater.StartClient(true))
            {
                MessageBox.Show("Невозможно запустить клиент, отсутствует файл для запуска","Ошибка запуска");
            }

            Close();
        }
    }
}
