using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeCamp2009Demos
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           // routes.MapRoute(
           //    "SearchAttenders",                                                  // Route name
           //    "Attenders/Search/{filter}",                                        // URL with parameters
           //    new { controller = "Attenders", action = "Search", title = "" }    // Parameter defaults
           //);

            routes.MapRoute(
                "TalkDetails",                                                  // Route name
                "Talks/Details/{title}",                                        // URL with parameters
                new { controller = "Talks", action = "Details", title = "" }    // Parameter defaults
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );


        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}