using Microsoft.Practices.Unity;

namespace Romontinka.Server.Core
{
    /// <summary>
    /// Конфигурация контейнера.
    /// </summary>
    public abstract class ContainerConfiguration
    {
        /// <summary>
        /// Получает конфигурацию IoC контейрена.
        /// </summary>
        /// <returns>Контейнер Unity.</returns>
        public abstract UnityContainer GetConfiguration();
    }
}
