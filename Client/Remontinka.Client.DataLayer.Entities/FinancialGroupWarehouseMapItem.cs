using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Пункт соответвия финансовой группы и склада.
    /// </summary>
    public class FinancialGroupWarehouseMapItem:EntityBase<Guid>
    {
        private string _financialGroupWarehouseMapID;

        /// <summary>
        /// Код пункта соответствия финансовой группы и склада.
        /// </summary>
        public string FinancialGroupWarehouseMapID
        {
            get { return _financialGroupWarehouseMapID; }
            set { FormatUtils.ExchangeFields(ref _financialGroupWarehouseMapIDGuid, ref _financialGroupWarehouseMapID, value); }
        }

        private Guid? _financialGroupWarehouseMapIDGuid;

        /// <summary>
        /// Код пункта соответствия финансовой группы и склада.
        /// </summary>
        public Guid? FinancialGroupWarehouseMapIDGuid
        {
            get { return _financialGroupWarehouseMapIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _financialGroupWarehouseMapIDGuid, ref _financialGroupWarehouseMapID,
                                           value);
            }
        }

        private string _warehouseID;

        /// <summary>
        /// Задает или получает код связанного склада.
        /// </summary>
        public string WarehouseID
        {
            get { return _warehouseID; }
            set { FormatUtils.ExchangeFields(ref _warehouseIDGuid, ref _warehouseID, value); }
        }

        private Guid? _warehouseIDGuid;

        /// <summary>
        /// Задает или получает код связанного склада.
        /// </summary>
        public Guid? WarehouseIDGuid
        {
            get { return _warehouseIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _warehouseIDGuid, ref _warehouseID, value);
            }
        }

        private string _financialGroupID;

        /// <summary>
        /// Задает или получает код связанной группы.
        /// </summary>
        public string FinancialGroupID
        {
            get { return _financialGroupID; }
            set
            {
                FormatUtils.ExchangeFields(ref _financialGroupIDGuid, ref _financialGroupID, value); ;
            }
        }

        private Guid? _financialGroupIDGuid;

        /// <summary>
        /// Задает или получает код связанной группы.
        /// </summary>
        public Guid? FinancialGroupIDGuid
        {
            get { return _financialGroupIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _financialGroupIDGuid, ref _financialGroupID, value);
            }
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (FinancialGroupWarehouseMapItem)entityBase;
            entity.FinancialGroupID = FinancialGroupID;
            entity.FinancialGroupWarehouseMapID = FinancialGroupWarehouseMapID;
            entity.WarehouseID = WarehouseID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return FinancialGroupWarehouseMapIDGuid;
        }
    }
}
