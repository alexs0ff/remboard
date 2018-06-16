using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.UnitOfWorks
{
    /// <summary>
    /// Юнит регистрации.
    /// </summary>
    public class RegistrationUnit : UnitBase
    {
        /// <summary>
        /// Задает или получает email регистрации.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Задает или получает юр название фирмы.
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку фирмы.
        /// </summary>
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает адрес фирмы.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает логин регистрации.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Задает или получает пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Задает или получает имя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Задает или получает фамилию.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает идентификатор клиента.
        /// </summary>
        public string ClientIdentifier { get; set; }
    }
}
