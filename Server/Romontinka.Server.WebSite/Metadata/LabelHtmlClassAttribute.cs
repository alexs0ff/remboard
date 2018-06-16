// Автор: 
// Проект: Romontinka.Server.WebSite
// Файл: LabelHtmlClassAttribute.cs
// Описание: 
// Дата создания: 01.04.2015 14:45
// 
// Дата модификации: 01.04.2015 14:46
// 

using System;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Metadata
{
    /// <summary>
    /// Атрибут для добавления класса к определенным свойствам метки модели.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LabelHtmlClassAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Имя класса атрибутов.
        /// </summary>
        public const string ClassNamesKey = "LabelClassNames";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Attribute"/> class.
        /// </summary>
        public LabelHtmlClassAttribute(string classNames)
        {
            ClassNames = classNames;
        }

        /// <summary>
        /// Задает или получает название классов через запятую.
        /// </summary>
        public string ClassNames { get; set; }

        /// <summary>
        /// When implemented in a class, provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[ClassNamesKey] = ClassNames;
        }
    }
}