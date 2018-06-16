using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Модель конфигурации.
    /// </summary>
    public interface IModelConfiguration
    {
        /// <summary>
        /// Initializes the sqLite types interception.
        /// </summary>
        /// <param name="container">The unity container.</param>
        void Initialize(IUnityContainer container);
    }
}
