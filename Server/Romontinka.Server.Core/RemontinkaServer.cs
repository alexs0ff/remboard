using System;
using Microsoft.Practices.Unity;

namespace Romontinka.Server.Core
{
    /// <summary>
    /// Сервер ремонтинки.
    /// </summary>
    public class RemontinkaServer
    {
         #region Singleton

        /// <summary>
        ///   Объект синхронизации.
        /// </summary>
        private static readonly object _syncRoot = new object();

        /// <summary>
        ///   Инстанс сервера.
        /// </summary>
        private static volatile RemontinkaServer _orionServer;

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
        public static void SetConfiguration(ContainerConfiguration configuration)
        {
            _container = configuration.GetConfiguration();
        }

        /// <summary>
        ///   Инициализирует новый экземпляр класса <see cref="T:Romontinka.Server.Core.RemontinkaServer" /> .
        /// </summary>
        private RemontinkaServer()
        {
            DataStore = SaveResolve<IDataStore>();
            SecurityService = SaveResolve<ISecurityService>();
            EntitiesFacade = SaveResolve<IEntitiesFacade>();
            OrderTimelineManager = SaveResolve<IOrderTimelineManager>();
            HTMLReportService = SaveResolve<IHTMLReportService>();
            MailingService = SaveResolve<IMailingService>();
            SystemService = SaveResolve<ISystemService>();
            CryptoService = SaveResolve<ICryptoService>();
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
        public static RemontinkaServer Instance
        {
            get
            {
                if (_orionServer == null)
                {
                    lock (_syncRoot)
                    {
                        if (_orionServer == null)
                        {
                            if (_container == null)
                            {
                                throw new InvalidOperationException(
                                    "Доступ к сервисам не возможен, контейнер типов в текущем контексте не инициализирован.");
                            }

                            _orionServer = new RemontinkaServer();
                        }
                    }
                }

                return _orionServer;
            }
        }

        #endregion

        /// <summary>
        /// Проверяет на то что сервис проинициализирован.
        /// </summary>
        /// <typeparam name="T">Проверяемый тип.</typeparam>
        private void CheckService<T>(object serviceInstance)
        {
            if (serviceInstance == null)
            {
                throw new ServiceIsNotAvailableException(typeof(T).Name,
                                                         string.Format(
                                                             "Сервис '{0}' не инициализирован, проверьте настройки контейнера",
                                                             typeof(T).Name));
            } //if
        }

        private IDataStore _dataStore;

        /// <summary>
        ///   Получает интерфейс хранилища данных.
        /// </summary>
        public IDataStore DataStore
        {
            get
            {
                CheckService<IDataStore>(_dataStore);
                return _dataStore;
            }
            private set { _dataStore = value; }
        }

        private ISecurityService _securityService;

        /// <summary>
        /// Получает сервис безопасности.
        /// </summary>
        public ISecurityService SecurityService
        {
            get
            {
                CheckService<ISecurityService>(_securityService);
                return _securityService;
            }
            private set
            {
                _securityService = value;
            }
        }
        
        private IEntitiesFacade _entitiesFacade;

        /// <summary>
        /// Получает сервис доступа к сущностям.
        /// </summary>
        public IEntitiesFacade EntitiesFacade
        {
            get
            {
                CheckService<IEntitiesFacade>(_entitiesFacade);
                return _entitiesFacade;
            }
            private set
            {
                _entitiesFacade = value;
            }
        }

        private ISystemService _systemService;

        /// <summary>
        /// Получает сервис доступа к сервисным функциям.
        /// </summary>
        public ISystemService SystemService
        {
            get
            {
                CheckService<ISystemService>(_systemService);
                return _systemService;
            }
            private set
            {
                _systemService = value;
            }
        }
        
        private IMailingService _mailingService;

        /// <summary>
        /// Получает сервис доступа к рассылке писем.
        /// </summary>
        public IMailingService MailingService
        {
            get
            {
                CheckService<IMailingService>(_mailingService);
                return _mailingService;
            }
            private set
            {
                _mailingService = value;
            }
        }

        private ICryptoService _cryptoService;

        /// <summary>
        /// Получает сервис криптографии.
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

        private IOrderTimelineManager _orderTimelineManager;

        /// <summary>
        /// Получает сервис доступа к трекеру изменений.
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

        private IHTMLReportService _htmlReportService;

        /// <summary>
        /// Получает сервис доступа к сервису отчетов.
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

        /// <summary>
        /// Получает сервис по его типу.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <returns>Сервис.</returns>
        public T GetService<T>()
             where T : class
        {
            return SaveResolve<T>();
        }
    }
}
