// Автор: 
// Проект: Romontinka.Server.WebSite
// Файл: EditorHtmlClassAttribute.cs
// Описание: 
// Дата создания: 01.04.2015 14:19
// 
// Дата модификации: 01.04.2015 14:25
// 

using System;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Metadata
{
    /// <summary>
    /// Атрибут для добавления класса к определенным свойствам редактора модели.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EditorHtmlClassAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Имя класса атрибутов.
        /// </summary>
        public const string ClassNamesKey = "EditorClassNames";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Attribute"/> class.
        /// </summary>
        public EditorHtmlClassAttribute(string classNames)
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