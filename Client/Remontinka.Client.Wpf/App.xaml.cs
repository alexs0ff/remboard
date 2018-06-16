using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using Remontinka.Client.Core;
using log4net;
using log4net.Config;

namespace Remontinka.Client.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger4 = LogManager.GetLogger(typeof(App));

        /// <summary>
        /// Содержит мютекс для приложения.
        /// </summary>
        private static Mutex _mutex;

        /// <summary>
        /// Вызывает событие <see cref="E:System.Windows.Application.Startup"/>.
        /// </summary>
        /// <param name="e">Объект <see cref="T:System.Windows.StartupEventArgs"/>, содержащий данные, относящиеся к событию.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            _mutex = new Mutex(true, "563EF1C1-18B3-46DC-88BA-5C24E682C016");

            if (!_mutex.WaitOne(TimeSpan.Zero, true))
            {
                MessageBox.Show("Только один экземпляр приложения может быть запущен");
                _mutex = null;
                Application.Current.Shutdown();
            } //if

            if (!e.Args.Contains("updatecompleted"))
            {
                MessageBox.Show("Данный модуль не предназначен для прямого запуска");
                Application.Current.Shutdown();
                return;
            } //if

            Current.DispatcherUnhandledException += CurrentDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            try
            {
                Configurate();
            }
            catch (Exception ex)
            {
                string inner = string.Empty;
                if (ex.InnerException!=null)
                {
                    inner = ex.InnerException.Message;

                } //if
                string error = string.Format("Перехвачена ошибка конфигурации: Версия {0} {1} {2} {3}: {4}", Assembly.GetExecutingAssembly().GetName().Version, ex.GetType(),inner,
                                             ex.Message, ex.StackTrace);
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger4.Error(error);
                Application.Current.Shutdown();

            } //try
            

        }

        /// <summary>
        /// Вызывается при непойманном исключении.
        /// </summary>
        void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                string error = string.Format("Перехвачена неизвестная ошибка: Версия {0} : {1} {2}: {3}", Assembly.GetExecutingAssembly().GetName().Version, exception.GetType(),
                                             exception.Message, exception.StackTrace);
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger4.Error(error);
                //var errorsender = new ErrorReportSender();
                //errorsender.SendReport(exception);
            } //if
            else
            {
                _logger4.ErrorFormat("Ошибка {0}", e.ExceptionObject);
            } //else

        }

        /// <summary>
        /// Вызывается при непойманном исключении.
        /// </summary>
        void CurrentDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            string error = string.Format("Перехвачена доп неизвестная ошибка: Версия {0} {1} {2}: {3}", Assembly.GetExecutingAssembly().GetName().Version, e.Exception.GetType(),
                                             e.Exception.Message, e.Exception.StackTrace);
            MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            _logger4.Error(error);
            //var errorsender = new ErrorReportSender();
            //errorsender.SendReport(e.Exception);
        }


        /// <summary>
        /// Производит конфигурацию сервисов..
        /// </summary>
        private void Configurate()
        {
            XmlConfigurator.Configure();
            
            
            ClientCore.SetConfiguration(new ClientConfiguration());

            ClientCore.Instance.DataStore.DeployIfNeeded();

            ClientCore.Instance.ApplicationNeedExit += ApplicationNeedExit;

            MainWindow window = new MainWindow();

            ArmController.Instance.SetMainWindow(window);
            ArmController.Instance.ShowAuthForm();
            

            window.Show();
        }

        /// <summary>
        /// Вызывается при необходимости закрыть приложение.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicationNeedExit(object sender, EventArgs e)
        {
            Shutdown();
        }

        /// <summary>
        /// Генерирует событие <see cref="E:System.Windows.Application.Exit"/>.
        /// </summary>
        /// <param name="e">Объект <see cref="T:System.Windows.ExitEventArgs"/>, содержащий данные события.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex = null;
            }
            base.OnExit(e);
        }
    }
}
