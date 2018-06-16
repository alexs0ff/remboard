﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Zen.Barcode.Web;

namespace Romontinka.Server.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Ignore("protocol.pcl");//Игнорируем операцию взаимодействия с протоколом

            //Регистрируем процессинг для штрихкодов.
            routes.Add(
                "BarcodeImaging",
                new Route(
                    "Barcode/{id}",
                    new BarcodeImageRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}