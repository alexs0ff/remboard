using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Models.Controls;

namespace Romontinka.Server.WebSite.Metadata
{
    /// <summary>
    /// Атрибут для метаданных комбобокса.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AjaxComboBoxAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Имя ключа для объекта.
        /// </summary>
        public const string AjaxComboBoxKey = "AjaxComboBox";

        /// <summary>
        /// Содержит модель для поля выбора.
        /// </summary>
        private readonly AjaxComboBoxModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.WebSite.Metadata.SimpleLookupAttribute"/> class.
        /// </summary>
        public AjaxComboBoxAttribute(string controller):this(controller,string.Empty,true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.WebSite.Metadata.SimpleLookupAttribute"/> class.
        /// </summary>
        public AjaxComboBoxAttribute(string controller, string getItemsAction, bool firstIsNull)
        {
            _model = new AjaxComboBoxModel();
            _model.Controller = controller;
            _model.GetItemsAction = getItemsAction;
            _model.FirstIsNull = firstIsNull;
        }

        /// <summary>
        /// When implemented in a class, provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[AjaxComboBoxKey] = _model;
        }
    }
}