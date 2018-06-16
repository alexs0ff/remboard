using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Romontinka.Server.Core.Security;
using Romontinka.Server.Core.UnitOfWorks;

namespace Romontinka.Server.Core
{
    /// <summary>
    /// Интерфейс системных сервисов.
    /// </summary>
    public interface ISystemService
    {
        /// <summary>
        /// Экспортирует сущности по определенным параметрам.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="exportParams">Параметры экспорта.</param>
        /// <returns>Результат.</returns>
        byte[] Export(SecurityToken token,ExportParams exportParams);
    }
}
