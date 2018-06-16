using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Категория товара.
    /// </summary>
    public class ItemCategory:EntityBase<Guid>
    {
        private string _itemCategoryID;

        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        public string ItemCategoryID
        {
            get { return _itemCategoryID; }
            set
            {
                FormatUtils.ExchangeFields(ref _itemCategoryIDGuid, ref _itemCategoryID, value);
            }
        }

        private Guid? _itemCategoryIDGuid;

        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        public Guid? ItemCategoryIDGuid
        {
            get { return _itemCategoryIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _itemCategoryIDGuid, ref _itemCategoryID,value);
            }
        }

        /// <summary>
        /// Задает или название категории.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (ItemCategory) entityBase;
            entity.ItemCategoryID = ItemCategoryID;
            entity.Title = Title;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return ItemCategoryIDGuid;
        }
    }
}
