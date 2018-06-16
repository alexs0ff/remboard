using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Пункт соответствия пользователей и филиалов.
    /// </summary>
    public class UserBranchMapItem:EntityBase<Guid>
    {
        private string _userBranchMapID;

        /// <summary>
        /// Задает или получает код соответствия.
        /// </summary>
        public string UserBranchMapID
        {
            get { return _userBranchMapID; }
            set
            {
                FormatUtils.ExchangeFields(ref _userBranchMapIDGuid, ref _userBranchMapID, value);
            }
        }

        private Guid? _userBranchMapIDGuid;

        /// <summary>
        /// Задает или получает код соответствия.
        /// </summary>
        public Guid? UserBranchMapIDGuid
        {
            get { return _userBranchMapIDGuid; }
            set { FormatUtils.ExchangeFields(ref _userBranchMapIDGuid, ref _userBranchMapID, value); }
        }

        private string _branchID;

        /// <summary>
        /// Задает или получает код связанного филиала.
        /// </summary>
        public string BranchID
        {
            get { return _branchID; }
            set { FormatUtils.ExchangeFields(ref _branchIDGuid, ref _branchID, value); }
        }

        private Guid? _branchIDGuid;

        /// <summary>
        /// Задает или получает код связанного филиала.
        /// </summary>
        public Guid? BranchIDGuid
        {
            get { return _branchIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _branchIDGuid, ref _branchID, value);
            }
        }

        private string _eventDate;

        /// <summary>
        /// Задает или получает дату соответствия.
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
        /// Задает или получает дату соответствия.
        /// </summary>
        public DateTime EventDateDateTime
        {
            get { return _eventDateDateTime; }
            set { FormatUtils.ExchangeFields(ref _eventDateDateTime, ref _eventDate, value); }
        }

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
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserIDGuid
        {
            get { return _userIDGuid; }
            set { FormatUtils.ExchangeFields(ref _userIDGuid, ref _userID, value); }
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (UserBranchMapItem) entityBase;
            entity.BranchID = BranchID;
            entity.UserBranchMapID = UserBranchMapID;
            entity.UserID = UserID;
            entity.EventDate = EventDate;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserBranchMapIDGuid;
        }
    }
}
