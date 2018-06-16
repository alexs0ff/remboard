using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Models;

namespace Romontinka.Server.WebSite.Controllers
{
    public class TestGridController:Controller
    {
        private const int _perPage = 5;

        private const int _maxItems = 8;

        [HttpPost]
        public JsonResult GetItems(string hiddenId, string lId, string search, int page)
        {
            return Json(new
            {
                Result = "Success",
                TotalCount = 10,
                PerPage = _perPage,
                CurrentPage = page,
                PaginatorMaxItems = _maxItems,
                Items= new []
                       {
                           new {id=1,txt1="Значение 1",txt2="Значение2",ClassID="success"},
                           new {id=2,txt1="Значение 4",txt2="Значение4",ClassID=""},
                           new {id=4,txt1="Текст2 5",txt2="Значение5",ClassID=""},
                           new {id=6,txt1="Таблица 5",txt2="Значение5",ClassID=""},
                           new {id=7,txt1="Значение 5",txt2="Значение5",ClassID="error"},
                           new {id=8,txt1="Текст 5",txt2="Значение5",ClassID=""},
                           new {id=8,txt1="'\"<a href='yandex.ru'>yandex</a>",txt2="Значение5",ClassID=""},
                       } 
            });
        }

        //TODO сделать JsonResult и ActionResult которые в случае удачи возвращали Success
        [HttpPost]
        public JsonResult DeleteItem(string id)
        {
            //return Json(new {Result="False"});
            return Json(new {Result="Success"});
        }

        //TODO сделать форму получения при создании новой сущности с передачей текущих параметров с формы
        public JsonResult GetItemForm(string id)
        {
            return Json(new { Result = "Success", Data = PartialView("GetItemForm", new TestModel { Account = "Тестовый л/с" }).RenderToString() });
        }

        public JsonResult GetNewItemForm(string hiddenId, string search, string id)
        {
            return Json(new { Result = "Success", Data = PartialView("GetNewItemForm", new TestModel { Account = "Новый" }).RenderToString() });
        }
        
        public JsonResult SaveEditedItem(TestModel model)
        {
            if (model.Account=="false")
            {
                ModelState.AddModelError("Account","Подтверждение");
                ModelState.AddModelError("","Какие-то ошибки");
                return Json(new { Result = "Success", NeedReloadModel = true, Data = PartialView("GetItemForm", model).RenderToString() });
            }
            return Json(new { Result = "Success", Item = new { id = 123, txt1 = "Изменено 1", txt2 = "Измененное значение", ClassID = "" } });
        }

        public JsonResult SaveCreatedItem(TestModel model)
        {
            if (model.Account=="false")
            {
                return Json(new { Result = "Success", NeedReloadModel = true, Data = PartialView("GetNewItemForm", model).RenderToString() });
            }
            return Json(new { Result = "Success", Item = new { id = 56, txt1 = "Создано 1", txt2 = "Созданное значение", ClassID = "" } });
        }
    }
}