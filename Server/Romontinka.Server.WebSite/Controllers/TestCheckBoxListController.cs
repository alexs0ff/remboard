using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Controllers
{
    public class TestCheckBoxListController:Controller
    {
        [HttpPost]
        public JsonResult GetItems(string[] ids, string parent)
        {
            var items = new[]
                        {
                            new SelectListItem{Text = "Список1",Value = "1",Selected =false},
                            new SelectListItem{Text = "Список2",Value = "2",Selected =false},
                            //new SelectListItem{Text = "Выбор3",Value = "3",Selected = true},
                            new SelectListItem{Text = "Список3",Value = "3",Selected =true},
                            new SelectListItem{Text = "Список4",Value = "4",Selected =false},
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