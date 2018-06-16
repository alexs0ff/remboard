using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Операция синхронизации.
    /// </summary>
    public class SyncOperation : EntityBase<Guid>
    {
        private string _syncOperationID;

        /// <summary>
        /// Задает или получает код операции синхронизации.
        /// </summary>
        public string SyncOperationID
        {
            get { return _syncOperationID; }
            set
            {
                FormatUtils.ExchangeFields(ref _syncOperationIDGuid, ref _syncOperationID, value);
            }
        }

        private Guid? _syncOperationIDGuid;

        /// <summary>
        /// Задает или получает код операции синхронизации.
        /// </summary>
        public Guid? SyncOperationIDGuid
        {
            get { return _syncOperationIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _syncOperationIDGuid, ref _syncOperationID, value);
            }
        }

        private string _operationBeginTime;

        /// <summary>
        /// Задает или получает дату начала операции.
        /// </summary>
        public string OperationBeginTime
        {
            get { return _operationBeginTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _operationBeginTimeDateTime, ref _operationBeginTime, value);
            }
        }

        private DateTime _operationBeginTimeDateTime;

        /// <summary>
        /// Задает или получает дату начала операции.
        /// </summary>
        public DateTime OperationBeginTimeDateTime
        {
            get { return _operationBeginTimeDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _operationBeginTimeDateTime, ref _operationBeginTime, value);
            }
        }

        private string _operationEndTime;

        /// <summary>
        /// Задает или получает дату начала операции.
        /// </summary>
        public string OperationEndTime
        {
            get { return _operationEndTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _operationEndTimeDateTime, ref _operationEndTime, value);
            }
        }

        private DateTime? _operationEndTimeDateTime;

        /// <summary>
        /// Задает или получает дату начала операции.
        /// </summary>
        public DateTime? OperationEndTimeDateTime
        {
            get { return _operationEndTimeDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _operationEndTimeDateTime, ref _operationEndTime, value);
            }
        }

        private long _isSuccess;

        /// <summary>
        /// Задает или получает признак успеха.
        /// </summary>
        public long IsSuccess
        {
            get { return _isSuccess; }
            set { FormatUtils.ExchangeFields(ref _isSuccessBoolean, ref _isSuccess, value); }
        }

        private bool _isSuccessBoolean;

        /// <summary>
        /// Задает или получает признак успеха.
        /// </summary>
        public bool IsSuccessBoolean
        {
            get { return _isSuccessBoolean; }
            set
            {
                FormatUtils.ExchangeFields(ref _isSuccessBoolean, ref _isSuccess, value);
            }
        }

        /// <summary>
        /// Задает или получает комментарий.
        /// </summary>
        public string Comment { get; set; }

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
            set
            {
                FormatUtils.ExchangeFields(ref _userIDGuid, ref _userID, value);
            }
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (SyncOperation) entityBase;
            entity.Comment = Comment;
            entity.IsSuccess = IsSuccess;
            entity.OperationBeginTime = OperationBeginTime;
            entity.OperationEndTime = OperationEndTime;
            entity.SyncOperationID = SyncOperationID;
            entity.UserID = UserID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return SyncOperationIDGuid;
        }
    }
}
