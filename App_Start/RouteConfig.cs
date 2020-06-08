using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BankApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Currency", "Currency/{action}", new { controller = "Currency", action = "Index" }, new[] { "BankApplication.Controllers" });

            routes.MapRoute("News", "News/{action}/{name}", new { controller = "News", action = "Index", name = UrlParameter.Optional }, new[] { "BankApplication.Controllers" });

            routes.MapRoute("Login", "Login/{action}", new { controller = "Login", action = "Index" }, new[] { "BankApplication.Controllers" });

            routes.MapRoute("Clients", "Clients/{action}/{name}", new { controller = "Clients", action = "Index", name = UrlParameter.Optional }, new[] { "BankApplication.Controllers" });

            routes.MapRoute("Client", "Client/{action}/{name}", new { controller = "Client", action = "Index", name = UrlParameter.Optional }, new[] { "BankApplication.Controllers" });

            routes.MapRoute("Employees", "Employees/{action}", new { controller = "Employees", action = "Index" }, new[] { "BankApplication.Controllers" });

            routes.MapRoute("Employee", "Employee/{action}", new { controller = "Employee", action = "Index" }, new[] { "BankApplication.Controllers" });

            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "BankApplication.Controllers" });

            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "BankApplication.Controllers" });
        }
    }
}
