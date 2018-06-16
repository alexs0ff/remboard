using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core.Interception
{
    public class BindableModelObject : ModelObject, INotifyPropertyChanged
    {
        /// <summary>
        /// The self property name.
        /// </summary>
        private const string SelfPropertyName = "Self";

        /// <summary>
        /// Rises the notify property changed event on Self property.
        /// </summary>
        public void RiseSelfPropertyChanged()
        {
            RisePropertyChanged(SelfPropertyName);
            EventHandler<EventArgs> handler = SelfPropertyChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <summary>
        /// The self object properties state.
        /// This property is notifiend about all object changes.
        /// Use with SelfPropertyChanged attribute.
        /// </summary>
        public object Self
        {
            get { return this; }
        }


        /// <summary>
        /// Occurs when the Self property has been changed.
        /// </summary>
        public event EventHandler<EventArgs> SelfPropertyChanged;

        #region INotifyPropertyChanged Implementation

        /// <summary>
        ///   Occurs when any properties are changed on this object.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///   A helper method that raises the PropertyChanged event for a property.
        /// </summary>
        /// <param name = "propertyNames">The names
        ///   of the properties that changed.</param>
        protected virtual void NotifyChanged(params string[] propertyNames)
        {
            foreach (string name in propertyNames)
            {
                RisePropertyChanged(name);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        public virtual void RisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
