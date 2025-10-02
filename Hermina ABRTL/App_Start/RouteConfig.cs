using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hermina_ABRTL
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "LandingPage", action = "Login", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "ExePort",
            //    url: "{controller}/{action}/{IDRS}/{Round}",
            //    defaults: new { controller = "Checker", action = "ExePort", IDRS = UrlParameter.Optional, Round = UrlParameter.Optional }
            //);
        }
    }
}
