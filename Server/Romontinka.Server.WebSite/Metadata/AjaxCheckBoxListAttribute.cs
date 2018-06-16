using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Models.Controls;

namespace Romontinka.Server.WebSite.Metadata
{
    /// <summary>
    /// Атрибут для метаданных списка с check боксами.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AjaxCheckBoxListAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Имя ключа для объекта.
        /// </summary>
        public const string AjaxCheckBoxListKey = "AjaxCheckBoxList";

        /// <summary>
        /// Содержит модель для поля выбора.
        /// </summary>
        private readonly AjaxCheckBoxListModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.WebSite.Metadata.SimpleLookupAttribute"/> class.
        /// </summary>
        public AjaxCheckBoxListAttribute(string controller):this(controller,string.Empty,string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.WebSite.Metadata.SimpleLookupAttribute"/> class.
        /// </summary>
        public AjaxCheckBoxListAttribute(string controller, string getItemsAction, string parentId)
        {
            _model = new AjaxCheckBoxListModel();
            _model.Controller = controller;
            _model.GetItemsAction = getItemsAction;
            _model.ParentId = parentId;
        }

        /// <summary>
        /// When implemented in a class, provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[AjaxCheckBoxListKey] = _model;
        }
    }
}