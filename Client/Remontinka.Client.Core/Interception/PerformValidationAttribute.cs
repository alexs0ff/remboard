using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Remontinka.Client.Core.Interception
{
    /// <summary>
    ///   Represents the attribute for performs the validation.
    ///   Required the Data annotation engine. http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations%28VS.95%29.aspx
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PerformValidationAttribute : HandlerAttribute
    {
        /// <summary>
        ///   Derived classes implement this method. When called, it
        ///   creates a new call handler as specified in the attribute
        ///   configuration.
        /// </summary>
        /// <param name = "container">The <see cref = "T:Microsoft.Practices.Unity.IUnityContainer" /> to use when creating handlers,
        ///   if necessary.</param>
        /// <returns>
        ///   A new call handler object.
        /// </returns>
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new PerformValidationCallHandler();
        }
    }
}
