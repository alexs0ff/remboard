using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Контейнер типов и сервисов.
    /// </summary>
    public class ClientCore
    {
        #region Singleton

        /// <summary>
        ///   Объект синхронизации.
        /// </summary>
        private static readonly object _syncRoot = new object();

        /// <summary>
        ///   Инстанс сервера.
        /// </summary>
        private static volatile ClientCore _rubleExpressServer;

        /// <summary>
        ///   Ioc контейнер.
        /// </summary>
        private static UnityContainer _container;

        /// <summary>
        ///   Ioc контейнер.
        /// </summary>
        public UnityContainer Container
        {
            get { return _container; }
        }

        /// <summary>
        ///   Установка контейнера типов IoC.
        /// </summary>
        /// <param name="configuration"> Конфигурация. </param>
        public static void SetConfiguration(IModelConfiguration configuration)
        {
            _container = new UnityContainer();
            configuration.Initialize(_container);
            ModelConfiguration = configuration;
        }

        /// <summary>
        /// Возвращает конфигурацию контейнера.
        /// </summary>
        public static IModelConfiguration ModelConfiguration { get; private set; }

        /// <summary>
        ///   Инициализирует новый экземпляр класса <see cref="T:Remontinka.Client.Core.ClientCore" /> .
        /// </summary>
        private ClientCore()
        {
            AuthService = SaveResolve<IAuthService>();
            UserNotifier = SaveResolve<IUserNotifier>();
            WebClient = SaveResolve<IWebClient>();
            DataStore = SaveResolve<IDataStore>();
            CryptoService = SaveResolve<ICryptoService>();
            SyncService = SaveResolve<ISyncService>();
            OrderTimelineManager = SaveResolve<IOrderTimelineManager>();
            HTMLReportService = SaveResolve<IHTMLReportService>();
           
        }

        /// <summary>
        /// Обеспечивает проверку на возможность решения типа.
        /// </summary>
        /// <typeparam name="T">Тип для резолвинга.</typeparam>
        /// <returns>Инициализированный объект типа или null.</returns>
        private T SaveResolve<T>() where T : class
        {
            T result = null;

            if (_container.IsRegistered<T>())
            {
                result = _container.Resolve<T>();
            } //if

            return result;
        }

        /// <summary>
        /// Получает инстанс сервера.
        /// </summary>
        /// <returns></returns>
        public static ClientCore Instance
        {
            get
            {
                if (_rubleExpressServer == null)
                {
                    lock (_syncRoot)
                    {
                        if (_rubleExpressServer == null)
                        {

                            if (_container == null)
                            {
                                throw new InvalidOperationException("Доступ к сервисам не возможен, контейнер типов в текущем контексте не инициализирован.");
                            }

                            _rubleExpressServer = new ClientCore();
                        }
                    }
                }

                return _rubleExpressServer;
            }
        }

        /// <summary>
        /// Creates an instance for intercepting.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <returns>The created instance</returns>
        public T CreateInstance<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// Creates an instance for intercepting.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <returns>The created instance</returns>
        public T CreateInstance<T>(Type typeOfObject)
            where T: class
        {
            var instance = _container.Resolve(typeOfObject);
            if (instance!=null)
            {
                var obj = instance as T;

                return obj;
            }

            return null ;
        }

        #endregion

        #region  External events

        /// <summary>
        /// Указывает на событие, которое должно указать на закрытие приложения.
        /// </summary>
        public event EventHandler<EventArgs> ApplicationNeedExit;

        /// <summary>
        /// Вызывает событие указывающее основному приложению на необходимость завершения работы.
        /// </summary>
        public void RiseApplicationNeedExit()
        {
            EventHandler<EventArgs> handler = ApplicationNeedExit;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        #endregion  External events

        #region  Services

        /// <summary>
        /// Проверяет на то что сервис проинициализирован.
        /// </summary>
        /// <typeparam name="T">Проверяемый тип.</typeparam>
        private void CheckService<T>(object serviceInstance)
        {
            if (serviceInstance == null)
            {
                throw new InvalidCastException(string.Format("Сервис '{0}' не инициализирован, проверьте настройки контейнера", typeof(T).Name));
            } //if
        }


        private IHTMLReportService _htmlReportService;

        /// <summary>
        /// Получает сервис создания отчетами пользователей.
        /// </summary>
        public IHTMLReportService HTMLReportService
        {
            get
            {
                CheckService<IHTMLReportService>(_htmlReportService);
                return _htmlReportService;
            }
            private set
            {
                _htmlReportService = value;
            }
        }

        private IOrderTimelineManager _orderTimelineManager;

        /// <summary>
        /// Получает сервис управления изменениями заказа.
        /// </summary>
        public IOrderTimelineManager OrderTimelineManager
        {
            get
            {
                CheckService<IOrderTimelineManager>(_orderTimelineManager);
                return _orderTimelineManager;
            }
            private set
            {
                _orderTimelineManager = value;
            }
        }


        private IAuthService _authService;

        /// <summary>
        /// Получает сервис аутентификации и афторизации.
        /// </summary>
        public IAuthService AuthService
        {
            get
            {
                CheckService<IAuthService>(_authService);
                return _authService;
            }
            private set
            {
                _authService = value;
            }
        }


        private IUserNotifier _userNotifier;

        /// <summary>
        /// Получает исообщений пользователю.
        /// </summary>
        public IUserNotifier UserNotifier
        {
            get
            {
                CheckService<IUserNotifier>(_userNotifier);
                return _userNotifier;
            }
            private set
            {
                _userNotifier = value;
            }
        }

        private IWebClient _webClient;

        /// <summary>
        /// Получает клиент запросов к серверу.
        /// </summary>
        public IWebClient WebClient
        {
            get
            {
                CheckService<IWebClient>(_webClient);
                return _webClient;
            }
            private set
            {
                _webClient = value;
            }
        }

        private IDataStore _dataStore;

        /// <summary>
        /// Получает сервис данных.
        /// </summary>
        public IDataStore DataStore
        {
            get
            {
                CheckService<IDataStore>(_dataStore);
                return _dataStore;
            }
            private set
            {
                _dataStore = value;
            }
        }
        

        private ICryptoService _cryptoService;

        /// <summary>
        /// Получает криптографический сервис.
        /// </summary>
        public ICryptoService CryptoService
        {
            get
            {
                CheckService<ICryptoService>(_cryptoService);
                return _cryptoService;
            }
            private set
            {
                _cryptoService = value;
            }
        }
        
        private ISyncService _syncService;

        /// <summary>
        /// Получает криптографический сервис.
        /// </summary>
        public ISyncService SyncService
        {
            get
            {
                CheckService<ISyncService>(_syncService);
                return _syncService;
            }
            private set
            {
                _syncService = value;
            }
        }



        private IRemontinkaService _remontinkaService;

        /// <summary>
        /// Получает сервис доступа к функциям инфраструктуры.
        /// </summary>
        public IRemontinkaService RemontinkaService
        {
            get
            {
                CheckService<IRemontinkaService>(_remontinkaService);
                return _remontinkaService;
            }
            private set
            {
                _remontinkaService = value;
            }
        }
       #endregion
    }
}
