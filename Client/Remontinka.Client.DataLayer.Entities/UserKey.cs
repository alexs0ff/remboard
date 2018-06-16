using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Представляет пользовательский ключ.
    /// </summary>
    public class UserKey : EntityBase<Guid>
    {
        private string _userKeyID;

        /// <summary>
        /// Задает или получает код пользовательского ключа.
        /// </summary>
        public string UserKeyID
        {
            get { return _userKeyID; }
            set
            {
                FormatUtils.ExchangeFields(ref _userKeyGuidID, ref _userKeyID, value);
            }
        }

        private Guid? _userKeyGuidID;

        /// <summary>
        /// Задает или получает код ключа в Guid представлении.
        /// </summary>
        public Guid? UserKeyIDGuid
        {
            get { return _userKeyGuidID; }
            set
            {
                FormatUtils.ExchangeFields(ref _userKeyGuidID, ref _userKeyID,value);
            }
        }

        private string _eventDate;

        /// <summary>
        /// Задает или получает дату добавления.
        /// </summary>
        public string EventDate
        {
            get { return _eventDate; }
            set
            {
                FormatUtils.ExchangeFields(ref _eventDateDateTime, ref _eventDate, value);
            }
        }

        private DateTime _eventDateDateTime;

        /// <summary>
        /// Задает или получает дату обновление в формате даты.
        /// </summary>
        public DateTime EventDateDateTime
        {
            get { return _eventDateDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _eventDateDateTime, ref _eventDate,value);
            }
        }

        /// <summary>
        /// Задает или получает содержимое публичного ключа.
        /// </summary>
        public string PublicKeyData { get; set; }

        /// <summary>
        /// Задает или получает содрежимое секретного ключа.
        /// </summary>
        public string PrivateKeyData { get; set; }

        /// <summary>
        /// Задает или получает номер ключа.
        /// </summary>
        public string Number { get; set; }

        private string _userID;

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public string UserID
        {
            get { return _userID; }
            set
            {
                FormatUtils.ExchangeFields(ref _userIDGuid, ref _userID, value);
            }
        }

        private Guid? _userIDGuid;

        /// <summary>
        /// Задает или получает код пользователя в Guid представлении.
        /// </summary>
        public Guid? UserIDGuid
        {
            get { return _userIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _userIDGuid, ref _userID,value);
            }
        }

        private long _isActivated;

        /// <summary>
        /// Задает или получает признак активности ключа.
        /// </summary>
        public long IsActivated
        {
            get { return _isActivated; }
            set
            {
                FormatUtils.ExchangeFields(ref _isActivatedBool, ref _isActivated, value);
            }
        }

        private bool _isActivatedBool;

        /// <summary>
        /// Задает или получает признак активности ключа.
        /// </summary>
        public bool IsActivatedBool
        {
            get { return _isActivatedBool; }
            set
            {
                FormatUtils.ExchangeFields(ref _isActivatedBool, ref _isActivated, value);
            }
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (UserKey) entityBase;
            entity.EventDate = EventDate;
            entity.Number = Number;
            entity.PrivateKeyData = PrivateKeyData;
            entity.PublicKeyData = PublicKeyData;
            entity.UserID = UserID;
            entity.UserKeyID = UserKeyID;
            entity.IsActivated = IsActivated;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserKeyIDGuid;
        }
    }
}
