using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Описатель грида с данными.
    /// </summary>
    public class DataGridDescriptor
    {
        /// <summary>
        /// Содержит имя по умолчанию для колонки с идентификатором сущности.
        /// </summary>
        private const string KeyPropertyNameDefault = "Id";

        public const string TableStripedClass = "table-striped";

        public const string TableBorderedClass = "table-bordered";

        public DataGridDescriptor()
        {
            SearchInputs = new List<SearchInputBase>();
            Columns = new List<GridColumnBase>();
            KeyGridColumn = new KeyGridColumn { Id = KeyPropertyNameDefault };
        }

        /// <summary>
        /// Задает или получает флаг указывающий, что строки в таблице будут выделены через одну.
        /// </summary>
        public bool HasTableStripedClass { get; set; }

        /// <summary>
        /// Задает или получет флаг указывающий, что ячейки в таблице будут выделены границами.
        /// </summary>
        public bool HasTableBorderedClass { get; set; }

        /// <summary>
        /// Задает или получает имя грида, должен быть уникальным в пределах страницы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получает настраиваемые формы поиска.
        /// </summary>
        public List<SearchInputBase> SearchInputs { get;set; }

        /// <summary>
        /// Получает списко колонок в гриде.
        /// </summary>
        public List<GridColumnBase> Columns { get; set; }

        /// <summary>
        /// Задает или получает описатель ключевой колонки.
        /// </summary>
        public KeyGridColumn KeyGridColumn { get; set; }

        /// <summary>
        /// Задает или получает описатель колонки удаления данных.
        /// </summary>
        public DeleteButtonGridColumn DeleteButtonGridColumn { get; set; }

        /// <summary>
        /// Задает или получает опистель кнопки создания данных.
        /// </summary>
        public CreateButtonGrid CreateButtonGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель колонки редактирования данных.
        /// </summary>
        public EditButtonGridColumn EditButtonGridColumn { get; set; }

        /// <summary>
        /// Задает или получает описатель колонки с кнопкой детализации.
        /// </summary>
        public ShowDetailsButtonColumn ShowDetailsButtonColumn { get; set; }

        /// <summary>
        /// Задает или получает высоту грида в пикселях.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Задает или получает максимальный размер вариантов пагинатора.
        /// </summary>
        public int PaginatorMaxSize { get; set; }

        /// <summary>
        /// Задает или получает название возвращаемых колонок, где указываются классы для тегов tr в таблице.
        /// </summary>
        public string RowClassId { get; set; }

        /// <summary>
        /// Задает или получает флаг указывающий, что грид при показе необходимо самостоятельно загрузить.
        /// </summary>
        public bool AutoLoad { get; set; }

        /// <summary>
        /// Задает или получает название контроллера для данных.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Задает или получает действие по выборке данных.
        /// </summary>
        public string GetItemsAction { get; set; }

        /// <summary>
        /// Задает или получает действие по удалению данных.
        /// </summary>
        public string DeleteItemAction { get; set; }

        /// <summary>
        /// Задает или получает действия для сохранения элемента, который был изменен.
        /// </summary>
        public string SaveEditedItemAction { get; set; }

        /// <summary>
        /// Задает или получает действие по сохранению элемента, который был создан.
        /// </summary>
        public string SaveCreatedItemAction { get; set; }

        /// <summary>
        /// Задает или получает действие по получению формы редактирования определенного элемента.
        /// </summary>
        public string EditItemAction { get; set; }

        /// <summary>
        /// Задает или получает действие по получению формы создания определенного элемента.
        /// </summary>
        public string CreateItemAction { get; set; }

        /// <summary>
        /// Задает или получает имя js функции которая будет вызвана перед обновлением данных.
        /// Если значение не задано, вызов фунции опускается.
        /// </summary>
        public string BeforeGridUpdateJsFunctionName { get; set; }
        
    }
}