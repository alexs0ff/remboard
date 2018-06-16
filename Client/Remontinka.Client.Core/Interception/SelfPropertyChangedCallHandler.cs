using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Remontinka.Client.Core.Interception
{
    /// <summary>
    /// The call handler of notifing self property changes.
    /// </summary>
    public class SelfPropertyChangedCallHandler : ICallHandler
    {
        /// <summary>
        ///   The name of value parameter that passed into property set method.
        /// </summary>
        private const string ValueParameter = "value";

        /// <summary>
        /// The self property name.
        /// </summary>
        private const string SelfPropertyName = "Self";

        /// <summary>
        ///   Implement this method to execute your handler processing.
        /// </summary>
        /// <param name = "input">Inputs to the current call to the target.</param>
        /// <param name = "getNext">Delegate to execute to get the next delegate in the handler
        ///   chain.</param>
        /// <returns>
        ///   Return value from the target.
        /// </returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            object newValue = null;
            object oldValue = null;

            BindableModelObject bindableObject = null;

            if (input.MethodBase.Name.StartsWith("set_"))
            {
                string propertyName = input.MethodBase.Name.Substring(4);

                newValue = input.Arguments[ValueParameter];
                var propertyInfo = input.Target.GetType().GetProperty(propertyName);
                oldValue = propertyInfo.GetValue(input.Target, null);

                bindableObject = input.Target as BindableModelObject;
            }

            var nextDelegate = getNext()(input, getNext);

            if (bindableObject != null)
            {
                ProcessChanges(bindableObject, newValue, oldValue);
            } //if

            return nextDelegate;
        }

        /// <summary>
        /// Processes the changes and invokes the NotifyPropertyChanges event on Self property.
        /// </summary>
        /// <param name="bindableObject">The bindable object.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        private void ProcessChanges(BindableModelObject bindableObject, object newValue, object oldValue)
        {
            bindableObject.RiseSelfPropertyChanged();

            if (_cashedBindableObject == null)
            {
                _cashedBindableObject = bindableObject;
            } //if

            UnregisterValue(oldValue);

            RegisterValue(newValue);
        }

        /// <summary>
        /// Registers the new value for new changes watching
        /// </summary>
        /// <param name="newValue">The new value.</param>
        private void RegisterValue(object newValue)
        {
            if (newValue != null)
            {
                var propertyChanged = newValue as INotifyPropertyChanged;

                if (propertyChanged != null)
                {
                    propertyChanged.PropertyChanged += WatchedValuePropertyChanged;
                } //if

                var collectionChanged = newValue as INotifyCollectionChanged;

                if (collectionChanged != null)
                {
                    collectionChanged.CollectionChanged += WatchedCollectionChanged;
                    var collection = newValue as ICollection;

                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            var itemChanged = item as INotifyPropertyChanged;

                            if (itemChanged != null)
                            {
                                itemChanged.PropertyChanged += WatchedValuePropertyChanged;
                            } //if
                        } //foreach
                    } //if
                } //if
            } //if
        }

        /// <summary>
        /// Unregisters old value for value changes watching.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        private void UnregisterValue(object oldValue)
        {
            if (oldValue != null)
            {
                var propertyChanged = oldValue as INotifyPropertyChanged;

                if (propertyChanged != null)
                {
                    propertyChanged.PropertyChanged -= WatchedValuePropertyChanged;
                } //if

                var collectionChanged = oldValue as INotifyCollectionChanged;

                if (collectionChanged != null)
                {
                    collectionChanged.CollectionChanged -= WatchedCollectionChanged;

                    var collection = oldValue as ICollection;

                    if (collection != null)
                    {
                        foreach (var itemChanged in collection.OfType<INotifyPropertyChanged>())
                        {
                            itemChanged.PropertyChanged -= WatchedValuePropertyChanged;
                        }
                    } //if

                } //if
            } //if
        }

        /// <summary>
        /// Occurs when watched collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void WatchedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _cashedBindableObject.RiseSelfPropertyChanged();

            if (e.OldItems != null)
            {
                foreach (var propertyChanged in e.OldItems.OfType<INotifyPropertyChanged>())
                {
                    propertyChanged.PropertyChanged -= WatchedValuePropertyChanged;
                }
            } //if

            if (e.NewItems != null)
            {
                foreach (var propertyChanged in e.NewItems.OfType<INotifyPropertyChanged>())
                {
                    propertyChanged.PropertyChanged += WatchedValuePropertyChanged;
                }
            }
        }

        /// <summary>
        /// Occurs when any of watched values has been changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void WatchedValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _cashedBindableObject.RiseSelfPropertyChanged();
        }

        private static int _currentDepth;

        /// <summary>
        /// The max recursion depth.
        /// </summary>
        private const int RecursionDepth = 10;

        private BindableModelObject _cashedBindableObject;

        /// <summary>
        ///   Order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get { return 1; }
            set { throw new NotImplementedException(); }
        }
    }
}
