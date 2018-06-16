using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Models.Controls;

namespace Romontinka.Server.WebSite.Metadata
{
    /// <summary>
    /// Атрибут для метаданных автокомплита с локальным источником.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutocompleteLocalSourceAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Attribute"/> class.
        /// </summary>
        public AutocompleteLocalSourceAttribute(string source,bool isMultiple)
        {
            _model = new AutocompleteLocalSourceModel
                     {
                         IsMultiple = isMultiple,
                         SourceName = source
                     };
        }

        /// <summary>
        /// Имя ключа для объекта.
        /// </summary>
        public const string AutocompleteLocalSourceKey = "AutocompleteLocalSource";

        /// <summary>
        /// Содержит модель для автодополнения.
        /// </summary>
        private readonly AutocompleteLocalSourceModel _model;

        /// <summary>
        /// When implemented in a class, provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[AutocompleteLocalSourceKey] = _model;
        }
    }
}