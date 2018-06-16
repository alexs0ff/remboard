using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Remontinka.Client.Core;
using Remontinka.Client.Core.Models;
using Remontinka.Client.Core.Services;
using Remontinka.Client.DataLayer.EntityFramework;
using Remontinka.Client.Wpf.Model;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf
{
    /// <summary>
    /// КОнфигурация клиента.
    /// </summary>
    public class ClientConfiguration : IModelConfiguration
    {
        /// <summary>
        /// Initializes the sqLite types interception.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public void Initialize(IUnityContainer container)
        {
            container.AddNewExtension<Interception>();

            container.Configure<Interception>().
               
                SetInterceptorFor<LongOperation>(new VirtualMethodInterceptor()).
                SetInterceptorFor<AuthControlModel>(new VirtualMethodInterceptor()).
               
                SetInterceptorFor<ArmAppModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<SyncProcessModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<SyncItem>(new VirtualMethodInterceptor()).
                SetInterceptorFor<RepairOrderViewModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<TextBoxControlModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<TextBlockControlModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<ComboBoxControlModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<DateControlModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<MaskedTextBoxControlModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<MoneyBoxControlModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<CheckBoxControlModel>(new VirtualMethodInterceptor()).
                SetInterceptorFor<EditModelMasterModel>(new VirtualMethodInterceptor()).
               
                AddPolicy("NotifyChanged");

            container.RegisterType<IAuthService, AuthService>();
            container.RegisterType<IUserNotifier, MessageBoxUserNotifier>();
            container.RegisterType<IWebClient, WebClientService>();
            container.RegisterType<IDataStore, RemontinkaStore>();
            container.RegisterType<ICryptoService, CryptoService>();
            container.RegisterType<ISyncService, SyncService>();
            container.RegisterType<IOrderTimelineManager, OrderTimelineManager>();
            container.RegisterType<IHTMLReportService, HTMLReportService>();
            
            InitBarCodes();
        }

        /// <summary>
        /// Инициализация алгоритмов штрих кодов.
        /// </summary>
        private void InitBarCodes()
        {
            
        }
    }
}
