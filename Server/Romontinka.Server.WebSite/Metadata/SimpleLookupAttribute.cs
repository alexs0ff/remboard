using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Metadata
{
    /// <summary>
    /// Атрибут для создание метаданных для простого поля с поиском.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SimpleLookupAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.WebSite.Metadata.SimpleLookupAttribute"/> class.
        /// </summary>
        public SimpleLookupAttribute(string controller, string getItemsAction, string getItemAction, string parentID, int minLenght)
        {
            _model = new LookupModel();
            _model.Controller = controller;
            _model.GetItemsAction = getItemsAction;
            _model.GetItemAction = getItemAction;
            _model.ParentID = parentID;
            _model.MinLenght = minLenght;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.WebSite.Metadata.SimpleLookupAttribute"/> class.
        /// </summary>
        public SimpleLookupAttribute(string parentID, int minLenght):this(null,null,null,parentID,minLenght)
        {
            
        }

        /// <summary>
        /// Имя ключа для объекта.
        /// </summary>
        public const string SimpleLookupKey = "SimpleLookup";

        /// <summary>
        /// Содержит модель для поля просмотра.
        /// </summary>
        private readonly LookupModel _model;

        /// <summary>
        /// When implemented in a class, provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[SimpleLookupKey] = _model;
            metadata.AdditionalValues[MetadataConstants.ValidationMessagePropertyID] =
                LookupHelpers.CreateLookupDisplayProperty(_model.Property);
        }
    }
}