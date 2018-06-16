using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Remontinka.Client.Core;
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Client.Wpf.Model;
using Remontinka.Client.Wpf.View;

namespace Remontinka.Client.Wpf.Controllers
{
    /// <summary>
    /// Контроллер авторизации.
    /// </summary>
    public class AuthController : BaseController
    {
        /// <summary>
        /// Содержит текущее представление.
        /// </summary>
        private AuthView _view;

        /// <summary>
        /// Содержит модель авторизации.
        /// </summary>
        private AuthControlModel _authModel;

        /// <summary>
        /// Получает View для отображения на форме.
        /// </summary>
        /// <returns>View.</returns>
        public override UserControl GetView()
        {
            return _view;
        }

        /// <summary>
        /// Инициализация данных контроллера.
        /// </summary>
        public override void Initialize()
        {
            _authModel = ClientCore.Instance.CreateInstance<AuthControlModel>();
            _view = new AuthView { DataContext = _authModel };
            _authModel.IsEnabaled = true;
            _authModel.RegisterAreaVisible = false;
            _view.cancelButton.Click += CancelButtonClick;
            _view.showRegisterButton.Click += ShowRegisterButtonClick;
            _view.registerButton.Click += RegisterButtonClick;
            _view.authButton.Click += AuthButtonClick;
            _view.passwordTextBox.KeyDown += PasswordTextBoxKeyDown;

            ReloadUsers(null);
            ClientCore.Instance.AuthService.InfoStatusChanged += AuthServiceInfoStatusChanged;
            ClientCore.Instance.AuthService.AuthError += AuthServiceAuthError;
            ClientCore.Instance.AuthService.UserRegistred += AuthServiceOnUserRegistred;
            ClientCore.Instance.AuthService.AuthComplited+=AuthServiceOnAuthComplited;
        }

        /// <summary>
        /// Вызывается когда пользователь нажимает клавиши в поле пароляю
        /// </summary>
        void PasswordTextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StartAuthProcess();
            } //if
        }

        /// <summary>
        /// Вызывается когда регистрация пользователя успешно проходит.
        /// </summary>
        private void AuthServiceOnAuthComplited(object sender, AuthComplitedEventArgs authComplitedEventArgs)
        {
            //TODO:Сделать показ списка вопросов или регистрации
            if (!ClientCore.Instance.DataStore.SyncOperationExists())
            {
                ArmController.Instance.ShowSyncForm();
            } //if
            else
            {
                ArmController.Instance.Model.ShowMainMenu = true;
                ArmController.Instance.ShowRepairOrderList();
            }
        }

        /// <summary>
        /// Вызывается когда пользователь нажимает на кнопку "войти".
        /// </summary>
        void AuthButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            StartAuthProcess();
        }

        /// <summary>
        /// Стартует процесс авторизации.
        /// </summary>
        private void StartAuthProcess()
        {
            var selectedLogin = _view.loginsBox.SelectedItem as ComboBoxItemModel;
            if (selectedLogin == null)
            {
                _authModel.ErrorText = "Логин должен быть заполнен";
                return;
            } //if

            if (string.IsNullOrWhiteSpace(_view.passwordTextBox.Password))
            {
                _authModel.ErrorText = "Пароль не может быть пустым";
                return;
            } //if

            ClientCore.Instance.AuthService.StartAuth((string)selectedLogin.Value, _view.passwordTextBox.Password);
            _authModel.IsEnabaled = false;
        }

        private const int PasswordMinLenght = 6;

        /// <summary>
        /// Вызывается, когда пользователь нажимает на кнопку "зарегистрировать".
        /// </summary>
        void RegisterButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_view.registerLoginName.Text))
            {
                _authModel.ErrorText = "Логин должен быть заполнен";
                _view.registerLoginName.Focus();
                return;
            } //if

            if (string.IsNullOrWhiteSpace(_view.passwordRegisterTextBox.Password) || _view.passwordRegisterTextBox.Password.Length < PasswordMinLenght)
            {
                _authModel.ErrorText = string.Format("Пароль должен содержать не менее {0} символов", PasswordMinLenght);
                _view.passwordRegisterTextBox.Focus();
                return;
            } //if

            if (!StringComparer.Ordinal.Equals(_view.passwordRegisterTextBox.Password, _view.passwordRetryRegisterTextBox.Password))
            {
                _authModel.ErrorText = "Пароли не совпадают";
                _view.passwordRegisterTextBox.Focus();
                return;
            } //if
            _authModel.IsEnabaled = false;
            ClientCore.Instance.AuthService.StartUserRegistration(_view.registerLoginName.Text,
                                                                  _view.passwordRegisterTextBox.Password,
                                                                  _view.commentBox.Text);
            
        }

        /// <summary>
        /// Вызывается при регистрации нового пользователя.
        /// </summary>
        private void AuthServiceOnUserRegistred(object sender, UserRegistredEventArgs userRegistredEventArgs)
        {
            StartSaveInvoke(() =>
                            {
                                ReloadUsers(null);
                                _view.loginsBox.SelectedValue = userRegistredEventArgs.LoginName;
                                _authModel.RegisterAreaVisible = false;
                                _authModel.IsEnabaled = true;
                            });
            
        }

        /// <summary>
        /// Вызывается во время прихода информации об ошибке с сервиса авторизации.
        /// </summary>
        void AuthServiceAuthError(object sender, ErrorEventArgs e)
        {
            SaveInvoke(() => { 
                _authModel.ErrorText = e.Message;
                _authModel.IsEnabaled = true;
            });
        }

        /// <summary>
        /// Вызывается во время прихода информации с сервиса авторизации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AuthServiceInfoStatusChanged(object sender, InfoEventArgs e)
        {
            SaveInvoke(() => _authModel.ErrorText = e.InfoText);
        }

        /// <summary>
        /// Вызывается когда пользователь нажал на кнопку регистрации.
        /// </summary>
        void ShowRegisterButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _authModel.RegisterAreaVisible = true;
            _view.registerLoginName.Focus();
        }

        /// <summary>
        /// Вызывается когда необходимо отменить действие.
        /// </summary>
        void CancelButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ClientCore.Instance.RiseApplicationNeedExit();
        }

        /// <summary>
        /// Перезагрузка списка пользователей.
        /// </summary>
        void ReloadUsers(Action actionOnComplited)
        {
            SaveStartTask(ct => ClientCore.Instance.DataStore.GetUsers().ToList(), users =>
                                                                              {
                                                                                  _authModel.Users.Clear();
                                                                                      foreach (var user in users)
                                                                                      {
                                                                                          var item = new ComboBoxItemModel
                                                                                          {
                                                                                              Title = string.Format("{0} ({1})", user.LoginName, user.FirstName),
                                                                                              Value = user.LoginName
                                                                                          };
                                                                                          _authModel.Users.Add(item);
                                                                                      }

                                                                                  if (actionOnComplited!=null)
                                                                                  {
                                                                                      actionOnComplited();
                                                                                  } //if
                                                                              },
                          (exception, isCanceled, isFaulted) => _authModel.ErrorText = "Ошибка получения пользователей");
        }

        /// <summary>
        /// Завершает действие контроллера, освобождая его ресурсы.
        /// </summary>
        public override void Terminate()
        {
            ClientCore.Instance.AuthService.InfoStatusChanged -= AuthServiceInfoStatusChanged;
            ClientCore.Instance.AuthService.AuthError -= AuthServiceAuthError;
            ClientCore.Instance.AuthService.UserRegistred -= AuthServiceOnUserRegistred;
            ClientCore.Instance.AuthService.AuthComplited -= AuthServiceOnAuthComplited;
        }
    }
}
