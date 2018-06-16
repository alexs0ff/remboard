using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Models.Customer;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Context;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контролер взаимодействия с клиентами.
    /// </summary>
    public class CustomerController : BaseController
    {
        /// <summary>
        /// Поиск информации по заказу.
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            return View(new CustomerGetOrderModel());
        }

        /// <summary>
        /// Просмотр информации по заказу.
        /// </summary>
        [HttpPost]
        public ActionResult Index(CustomerGetOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            } //if

            return RedirectToAction("ShowOrderInfo", model);
        }

        public ActionResult ShowOrderInfo(CustomerGetOrderModel model)
        {
            var result = new CustomerGetOrderResultModel();

            RepairOrderGlobalReference reference = null;
            try
            {
                reference = RepairOrderGlobalReferenceHelper.ParseGlobalReference(model.GlobalRepairOrderNumber);
            }
            catch (Exception)
            {


            }

            if (reference != null)
            {
                var repairOrder = RemontinkaServer.Instance.DataStore.GetRepairOrder(reference.RepairOrderNumber,
                    reference.UserDomainNumber);
                if (repairOrder != null)
                {
                    if (string.Equals(repairOrder.AccessPassword, model.GlobalRepairOrderAccessPassword,
                        StringComparison.Ordinal))
                    {
                        result.IsSuccess = true;
                        if (repairOrder.StatusKind == StatusKindSet.Closed.StatusKindID)
                        {
                            result.Description = string.Format("Заказ с номером {0} закрыт", repairOrder.Number);
                        }
                        else
                        {
                            result.Description = string.Format("Статус заказа: \"{0}\", рекомендации: \"{1}\" ",
                                repairOrder.OrderStatusTitle,
                                repairOrder.Recommendation);
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Description = "Ошибочный код заказа";
                    }

                }
                else
                {
                    result.IsSuccess = false;
                    result.Description = "Нет такого заказа";
                }

            }
            else
            {
                result.IsSuccess = false;
                result.Description = "Заказ не найден";
            }

            return View(result);
        }
    }
}