using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.Controllers;
using Remontinka.Client.Wpf.Model;
using Xceed.Wpf.Toolkit;

namespace Remontinka.Client.Wpf
{
    /// <summary>
    /// Контроллер приложения АРМ.
    /// </summary>
    public class ArmController
    {
        #region Singleton

        /// <summary>
        ///   Объект синхронизации.
        /// </summary>
        private static readonly object _syncRoot = new object();

        /// <summary>
        ///   Инстанс контроллера.
        /// </summary>
        private static volatile ArmController _controller;

        /// <summary>
        ///   Инициализирует новый экземпляр класса <see cref="T:Remontinka.Client.Wpf.ArmController" /> .
        /// </summary>
        private ArmController()
        {
        }

        /// <summary>
        /// Получает инстанс менеджера.
        /// </summary>
        /// <returns></returns>
        public static ArmController Instance
        {
            get
            {
                if (_controller == null)
                {
                    lock (_syncRoot)
                    {
                        if (_controller == null)
                        {
                            _controller = new ArmController();
                        }
                    }
                }

                return _controller;
            }
        }

        #endregion

        /// <summary>
        /// Содержит ссылку на главное окно программы.
        /// </summary>
        private MainWindow _mainWindow;

        /// <summary>
        /// Получает ссылку на главное окно программы.
        /// </summary>
        public MainWindow MainWindow
        {
            get { return _mainWindow; }
        }

        /// <summary>
        /// Помещает ссылку на главное окно программы. 
        /// </summary>
        /// <param name="window">Инстанс главного окна.</param>
        public void SetMainWindow(MainWindow window)
        {
            Model = ClientCore.Instance.CreateInstance<ArmAppModel>();
            Model.CurrentOperation.Release();
            _mainWindow = window;
            _mainWindow.DataContext = Model;
        }

        /// <summary>
        /// Устанавливает UserControl в главное окно приложения.
        /// </summary>
        /// <param name="control">Контрол.</param>
        private void SetWindow(UserControl control)
        {
            if (_mainWindow.Dispatcher.CheckAccess())
            {
                _mainWindow.SetForegroundControl(control);
            } //if
            else
            {
                _mainWindow.Dispatcher.Invoke(DispatcherPriority.Normal,
                                              new Action<UserControl>(_mainWindow.SetForegroundControl), control);
            } //else
        }

        /// <summary>
        /// Устанавливает плавающее окно для показа.
        /// </summary>
        /// <param name="window">Окно.</param>
        public void SetChildWindow(ChildWindow window)
        {
            ClearAllChildWindows();
            _mainWindow.childWindowContainer.Children.Add(window);
        }

        /// <summary>
        /// Удаление с экрана приложения всех плавающих окон
        /// </summary>
        public void ClearAllChildWindows()
        {
            _mainWindow.childWindowContainer.Children.Clear();
        }


        /// <summary>
        /// Содержит текущий контроллер.
        /// </summary>
        private BaseController _currentController;

        /// <summary>
        /// Завершает действие текущего контроллера.
        /// </summary>
        private void DisposeCurrentController()
        {
            if (_mainWindow.Dispatcher.CheckAccess())
            {
                if (_currentController != null)
                {
                    _currentController.Terminate();
                } //if
                _currentController = null;
            }
            else
            {
                _mainWindow.Dispatcher.Invoke(DispatcherPriority.Normal,
                                              new Action(DisposeCurrentController));
            } //else
        }

        /// <summary>
        /// Устанавливает текущий контроллер.
        /// </summary>
        /// <param name="baseController">Контроллер который следует установить.</param>
        private void SetCurrentController(BaseController baseController)
        {
            if (_currentController != null && !_currentController.CanTerminate())
            {
                ClientCore.Instance.UserNotifier.ShowMessage("Внимание",
                                                             "Текущая операция не завершена, необходимо дождаться окончания действий системы");
                return;
            } //if

            DisposeCurrentController();

            if (_mainWindow.Dispatcher.CheckAccess())
            {
                _currentController = baseController;
                _currentController.Initialize();
                SetWindow(_currentController.GetView());
            }
            else
            {
                _mainWindow.Dispatcher.Invoke(DispatcherPriority.Normal,
                                              new Action<BaseController>(SetCurrentController), baseController);
            } //else
        }

        /// <summary>
        /// Получает модель программы.
        /// </summary>
        public ArmAppModel Model { get; private set; }

        #region  Auth

        /// <summary>
        /// Стартует АРМ с процесса авторизации.
        /// </summary>
        public void ShowAuthForm()
        {
            SetCurrentController(new AuthController());
        }

        #endregion  Auth

        #region Sync 

        /// <summary>
        /// Стартует АРМ с процесса авторизации.
        /// </summary>
        public void ShowSyncForm()
        {
            SetCurrentController(new SyncController());
        }

        #endregion Sync

        #region Order list 

        /// <summary>
        /// Стартует АРМ с процесса авторизации.
        /// </summary>
        public void ShowRepairOrderList()
        {
            SetCurrentController(new RepairOrderViewController());
        }

        #endregion Order list
    }
}
