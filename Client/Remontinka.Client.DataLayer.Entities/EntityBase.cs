using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    ///   Базовый класс для всех сущностей в системе.
    /// </summary>
    [Serializable]
    public class EntityBase<T>
        where T : struct
    {
        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public virtual void CopyTo(EntityBase<T> entityBase)
        {
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public virtual T? GetId()
        {
            return null;
        }
    }
}
