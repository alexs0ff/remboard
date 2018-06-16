using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Controllers
{
    public class TestLookupController:Controller
    {
        public JsonResult GetItems(string query, string parent)
        {
            return
                Json(
                    new
                        {
                            Result = "Success",
                            Items = new[] {new {Id = "11", Value = "Значение1 "}, new {Id = "2", Value = "Значение2"}}
                        });
        }

        public JsonResult GetItem(string id)
        {
            return Json(new { Result = "Success", Item = new { Id = "1", Value = "Значение7777" } });
        }
    }
}