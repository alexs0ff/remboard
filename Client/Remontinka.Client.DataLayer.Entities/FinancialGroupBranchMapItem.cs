using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Пункт соответвия финансовой группы и филиала.
    /// </summary>
    public class FinancialGroupBranchMapItem : EntityBase<Guid>
    {
        private string _financialGroupBranchMapID;

        /// <summary>
        /// Задает или получает код пункта соответствия финансовой группы и филиала.
        /// </summary>
        public string FinancialGroupBranchMapID
        {
            get { return _financialGroupBranchMapID; }
            set
            {
                FormatUtils.ExchangeFields(ref _financialGroupBranchMapIDGuid, ref _financialGroupBranchMapID, value);
                _financialGroupBranchMapID = value;
            }
        }

        private Guid? _financialGroupBranchMapIDGuid;

        /// <summary>
        /// Задает или получает код пункта соответствия финансовой группы и филиала.
        /// </summary>
        public Guid? FinancialGroupBranchMapIDGuid
        {
            get { return _financialGroupBranchMapIDGuid; }
            set { FormatUtils.ExchangeFields(ref _financialGroupBranchMapIDGuid, ref _financialGroupBranchMapID, value); }
        }

        private string _branchID;

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public string BranchID
        {
            get { return _branchID; }
            set { _branchID = value; }
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

        private string _financialGroupID;

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public string FinancialGroupID
        {
            get { return _financialGroupID; }
            set { _financialGroupID = value; }
        }

        private Guid? _financialGroupIDGuid;

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public Guid? FinancialGroupIDGuid
        {
            get { return _financialGroupIDGuid; }
            set { FormatUtils.ExchangeFields(ref _financialGroupIDGuid, ref _financialGroupID, value); }
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (FinancialGroupBranchMapItem) entityBase;
            entity.FinancialGroupBranchMapID = FinancialGroupBranchMapID;
            entity.BranchID = BranchID;
            entity.FinancialGroupID = FinancialGroupID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return FinancialGroupBranchMapIDGuid;
        }
    }
}
