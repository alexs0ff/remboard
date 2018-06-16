using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Remontinka.Client.Core.Interception
{
    /// <summary>
    /// The notify property change attribute. Used for change property insterception.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotifyPropertyChangedAttribute : HandlerAttribute
    {
        /// <summary>
        /// Will rise the value changed.
        /// </summary>
        private bool _riseValueChanged = true;

        /// <summary>
        /// Will rise the value changed.
        /// </summary>
        public bool RiseValueChanged
        {
            get { return _riseValueChanged = true; }
            set { _riseValueChanged = value; }
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            if (_riseValueChanged)
            {
                var wiring = ModelResolver.GetInstanceByContainer(container);
                return new NotifyPropertyChangedHandler(wiring);
            }
            else
            {
                return new NotifyPropertyChangedHandler(null);
            } //else
        }
    }
}
