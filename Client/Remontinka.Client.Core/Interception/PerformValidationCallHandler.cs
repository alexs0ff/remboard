using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Remontinka.Client.Core.Interception
{
    /// <summary>
    /// The call handler in order to performing validation.
    /// </summary>
    public class PerformValidationCallHandler : ICallHandler
    {
        /// <summary>
        ///   The name of value parameter that passed into property set method.
        /// </summary>
        private const string ValueParameter = "value";

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
            if (input.MethodBase.Name.StartsWith("set_"))
            {
                string propertyName = input.MethodBase.Name.Substring(4);
                //Validator.ValidateProperty( input.Arguments[ValueParameter],
                //                          new ValidationContext( input.Target ){MemberName = propertyName} );
            }

            return getNext()(input, getNext);
        }

        /// <summary>
        ///   Order in which the handler will be executed
        ///   Executes before <see cref = "T:SqLauncher.Web.Model.Interception.NotifyPropertyChangedHandler" />
        /// </summary>
        public int Order
        {
            get { return 1; }
            set { throw new NotImplementedException(); }
        }
    }
}
