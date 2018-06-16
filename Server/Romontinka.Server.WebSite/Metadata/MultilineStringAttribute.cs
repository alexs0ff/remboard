using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Metadata
{
    /// <summary>
    /// Атрибут для добавления к модели многострокового кода.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MultilineStringAttribute : Attribute, IMetadataAware
    {
        public MultilineStringAttribute(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        /// <summary>
        /// Содержит ключ от количества строк для текста.
        /// </summary>
        public const string StringRowsCountKey = "StringRowsCountKey";

        /// <summary>
        /// Содержит ключ от количество колонок для текста.
        /// </summary>
        public const string StringColumnsCountKey = "StringColumnsCountKey";

        /// <summary>
        /// Задает или получает количество строк.
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Задает или получает количество колонок.
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// When implemented in a class, provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[StringRowsCountKey] = Rows;
            metadata.AdditionalValues[StringColumnsCountKey] = Columns;
        }
    }
}