using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Controllers
{
    public class TestAjaxComboBoxController:Controller
    {
        public JsonResult GetItems(string id)
        {
            var items = new[]
                        {
                            new SelectListItem{Text = "Выбор1",Value = "1",Selected = "1"==id},
                            new SelectListItem{Text = "Выбор2",Value = "2",Selected = "2"==id},
                            //new SelectListItem{Text = "Выбор3",Value = "3",Selected = true},
                            new SelectListItem{Text = "Выбор3",Value = "3",Selected = "3"==id},
                            new SelectListItem{Text = "Выбор4",Value = "4",Selected = "4"==id},
                        };
            

            return
                Json(
                    new
                    {
                        Result = "Success",
                        Items = items
                    });
        }
    }
}