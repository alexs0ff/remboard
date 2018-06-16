using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Финансовая группа филиалов.
    /// </summary>
    public class FinancialGroupItem: EntityBase<Guid>
    {
        private string _financialGroupID;

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public string FinancialGroupID
        {
            get { return _financialGroupID; }
            set { FormatUtils.ExchangeFields(ref _financialGroupIDGuid, ref _financialGroupID, value); }
        }

        private Guid? _financialGroupIDGuid;

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public Guid? FinancialGroupIDGuid
        {
            get { return _financialGroupIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _financialGroupIDGuid,ref _financialGroupID,value );
            }
        }

        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает юр название фирмы.
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку фирмы.
        /// </summary>
        public string Trademark { get; set; }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return FinancialGroupIDGuid;
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (FinancialGroupItem) entityBase;
            entity.FinancialGroupID = FinancialGroupID;
            entity.LegalName = LegalName;
            entity.Title = Title;
            entity.Trademark = Trademark;
        }
    }
}
