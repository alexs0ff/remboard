using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.View;
using WindowStartupLocation = Xceed.Wpf.Toolkit.WindowStartupLocation;

namespace Remontinka.Client.Wpf.Controllers
{
    /// <summary>
    /// Контроллер предпросмотра пользовательских отчетов.
    /// </summary>
    public class CustomReportPreviewController : BaseController
    {
        /// <summary>
        /// Содержит представление.
        /// </summary>
        private CustomReportPreview _view;

        /// <summary>
        /// Инициализирует контроллер.
        /// </summary>
        public override void Initialize()
        {
            _view = new CustomReportPreview();
            _view.printButton.Click+=PrintButtonOnClick;
            _view.closeButton.Click+=CloseButtonOnClick;
        }

        /// <summary>
        /// Получает View для отображения на форме.
        /// </summary>
        /// <returns>View.</returns>
        public override UserControl GetView()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Завершает действие контроллера, освобождая его ресурсы.
        /// </summary>
        public override void Terminate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Показывает отчет для конкретных данных.
        /// </summary>
        /// <param name="customReportID">Код отчета для формирования.</param>
        /// <param name="repairOrderID">Код заказа.</param>
        public void ShowReport(Guid? customReportID, Guid? repairOrderID)
        {
            ArmController.Instance.SetChildWindow(_view);
            _view.WindowStartupLocation = WindowStartupLocation.Center;
            _view.Show();
            
            SaveStartTask(
                source =>
                ClientCore.Instance.HTMLReportService.CreateRepairOrderReport(
                    ClientCore.Instance.AuthService.SecurityToken, customReportID, repairOrderID), content =>
                                                                                                       {
                                                                                                           content =
                                                                                                               ContentCharset +
                                                                                                               content;
                                                                                                       _view.webBrowser.NavigateToString(content);
                                                                                                   }, null);
        }

        private const string ContentCharset = " <head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'> </head>";

        /// <summary>
        /// Вызывается когда пользователь нажимает на кнопку "закрыть".
        /// </summary>
        private void CloseButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _view.Close();
            ArmController.Instance.ClearAllChildWindows();
        }

        /// <summary>
        /// Вызывается когда пользователь нажимает на кнопку распечатать.
        /// </summary>
        private void PrintButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            dynamic domDocument = _view.webBrowser.Document;
            domDocument.execCommand("Print", true, null);
        }
    }
}
