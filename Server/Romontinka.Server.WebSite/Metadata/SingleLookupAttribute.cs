using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Models.Controls;

namespace Romontinka.Server.WebSite.Metadata
{
    /// <summary>
    /// Атрибут для создание метаданных для поля с поиском с открытием дочернего окна для выбора одного эдемента.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SingleLookupAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.WebSite.Metadata.SimpleLookupAttribute"/> class.
        /// </summary>
        public SingleLookupAttribute(string controller, Type searchModelType,string parentID,bool clearButton,bool fullScreen )
        {
            _model = new SingleLookupModel();
            _model.Controller = controller;
            _model.ParentID = parentID;
            _model.ClearButton = clearButton;
            _model.SearchModel = (JLookupSearchBaseModel)Activator.CreateInstance(searchModelType);
            _model.FullScreen = fullScreen;
        }

        /// <summary>
        /// Имя ключа для объекта.
        /// </summary>
        public const string SingleLookupKey = "SingleLookup";

        /// <summary>
        /// Содержит модель для поля просмотра.
        /// </summary>
        private readonly SingleLookupModel _model;

        /// <summary>
        /// When implemented in a class, provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[SingleLookupKey] = _model;
            metadata.AdditionalValues[MetadataConstants.ValidationMessagePropertyID] =
                LookupHelpers.CreateLookupDisplayProperty(_model.Property);
        }
    }
}