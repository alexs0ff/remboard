using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Remontinka.Client.Core.Interception
{
    /// <summary>
    /// Represents type interseption and dpendency injection  engine.
    /// </summary>
    public class ModelResolver
    {
        #region Fabric

        /// <summary>
        /// The created instances.
        /// </summary>
        private static readonly ICollection<ModelResolver> _instances = new Collection<ModelResolver>();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        private ModelResolver()
        {
            _container = new UnityContainer();
        }

        /// <summary>
        /// Creates and setups a new instance for model obejct wiring.
        /// </summary>
        /// <param name="modelInterception">The database model interception settings.</param>
        public static ModelResolver SetUp(IModelConfiguration modelInterception)
        {
            var wiring = new ModelResolver();

            modelInterception.Initialize(wiring._container);
            _instances.Add(wiring);
            return wiring;
        }

        /// <summary>
        /// Gets the Container Wiring instance by owned IUnityContater instance.
        /// </summary>
        /// <param name="container">The owned unity container.</param>
        /// <returns>The finded instance or null.</returns>
        internal static ModelResolver GetInstanceByContainer(IUnityContainer container)
        {
            return _instances.FirstOrDefault(instance => ReferenceEquals(instance.Container, container));
        }

        #endregion Fabric

        #region interception

        private IUnityContainer _container;

        #endregion

        #region Public Methods/Properties

        public IUnityContainer Container
        {
            get { return _container; }
        }

        public void TearDown()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
            _instances.Remove(this);
            _container = null;
        }

        #endregion

        #region Value change event

        /// <summary>
        /// Suspending counter of value changing event.
        /// </summary>
        private int _valueChangedSuspendCounter;

        /// <summary>
        /// Starts suspendings of rising valuse change events.
        /// </summary>
        public void BeginSuspendValueChangeEvent()
        {
            _valueChangedSuspendCounter++;
        }

        /// <summary>
        /// End suspendings of rising valuse change events.
        /// </summary>
        public void EndSuspendValueChangeEvent()
        {
            if (_valueChangedSuspendCounter > 0)
            {
                _valueChangedSuspendCounter--;
            }
        }

        /// <summary>
        /// Occurs when a value of interseption objects has been changed.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Rises a value changed event.
        /// </summary>
        /// <param name="e"></param>
        public void RiseValueChanged(ValueChangedEventArgs e)
        {
            if (_valueChangedSuspendCounter == 0)
            {
                EventHandler<ValueChangedEventArgs> handler = ValueChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        #endregion Value change event

        #region Creating instance

        /// <summary>
        /// Creates an instance for intercepting.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <returns>The created instance</returns>
        public T CreateInstance<T>()
        {
            BeginSuspendValueChangeEvent();
            var instance = _container.Resolve<T>();

            var bindableModel = instance as BindableModelObject;

            EndSuspendValueChangeEvent();
            return instance;
        }

        #endregion Creating instance
    }
}
