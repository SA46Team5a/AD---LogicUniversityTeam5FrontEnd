using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LogicUniversityTeam5
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Requisition",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "RequisitionController", action = "ManageRequisition", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Admin",
                url: "admin/{controller}/{action}/{id}",
                defaults: new { controller = "HomeController", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
