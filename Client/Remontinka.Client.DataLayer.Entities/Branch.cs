using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Представляет филиал.
    /// </summary>
    public class Branch:EntityBase<Guid>
    {
        private string _branchID;

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public string BranchID
        {
            get { return _branchID; }
            set { FormatUtils.ExchangeFields(ref _branchIDGuid, ref _branchID, value); }
        }

        private Guid? _branchIDGuid;

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public Guid? BranchIDGuid
        {
            get { return _branchIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _branchIDGuid, ref _branchID, value);
            }
        }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает адрес филиала.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает юр название филиала.
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (Branch)entityBase;
            entity.Address = Address;
            entity.BranchID = BranchID;
            entity.Title = Title;
            entity.LegalName = LegalName;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return BranchIDGuid;
        }
    }
}
