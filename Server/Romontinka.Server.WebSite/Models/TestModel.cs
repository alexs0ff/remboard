using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.User;

namespace Romontinka.Server.WebSite.Models
{
    /// <summary>
    /// Тестовая модель.
    /// </summary>
    public class TestModel
    {
        [UIHint("SingleLookup")]
        [Required]
        [SingleLookup("UserSingleLookup",typeof(JLookupUserSearchModel), null,true,true)]
        [DisplayName("Пользователь")]
        [EditorHtmlClass("mytestclass")]
        [LabelHtmlClass("mytestclass2")]
        public Guid? UserID { get; set; }

        [UIHint("Decimal")]
        [Required]
        [DisplayName("Число")]
        [EditorHtmlClass("mytestclass")]
        [LabelHtmlClass("mytestclass2")]
        public int? Number { get; set; }

        [UIHint("MultilineString")]
        [Required]
        [DisplayName("Лицевой счет")]
        [EditorHtmlClass("mytestclass")]
        [LabelHtmlClass("mytestclass2")]
        [MultilineString(5,3)]
        public string Text { get; set; }

        [Required]
        [DisplayName("Лицевой счет")]
        [EditorHtmlClass("mytestclass")]
        [LabelHtmlClass("mytestclass2")]
        public string Account { get; set; }

        [Required]
        [DisplayName("Дата")]
        [EditorHtmlClass("mytestclass")]
        [LabelHtmlClass("mytestclass2")]
        public DateTime EventDate { get; set; }

        [Required]
        [DisplayName("ПОдтвердить")]
        [EditorHtmlClass("mytestclass")]
        [LabelHtmlClass("mytestclass2")]
        public bool IsLoaded { get; set; }

        [UIHint("SimpleLookup")]
        [DisplayName("Поиск")]
        [SimpleLookupAttribute(null,0)]
        [EditorHtmlClass("mytestclass")]
        [LabelHtmlClass("mytestclass2")]
        [Required]
        public string TestLookup { get; set; }

        [UIHint("AjaxComboBox")]
        [DisplayName("Выбор")]
        [AjaxComboBox("TestAjaxComboBox","GetItems",true)]
        [EditorHtmlClass("mytestclass")]
        [LabelHtmlClass("mytestclass2")]
        [Required]
        public string AjaxComboBoxId { get; set; }

        [UIHint("AjaxCheckBoxList")]
        [DisplayName("Список")]
        [AjaxCheckBoxList("TestCheckBoxList", "GetItems", null)]
        [EditorHtmlClass("mytestclass")]
        [LabelHtmlClass("mytestclass2")]
        [Required]
        public string[] AjaxCheckBoxListIds { get; set; }
        
    }
}