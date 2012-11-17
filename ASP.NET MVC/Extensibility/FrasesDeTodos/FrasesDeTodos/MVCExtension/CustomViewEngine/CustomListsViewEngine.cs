namespace FrasesDeTodos.MVCExtension.CustomViewEngine
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    public class CustomListsViewEngine : RazorViewEngine
    {
        public CustomListsViewEngine()
        {
            //2 - Area
            //1 - Controller
            //0 - View

            var viewLocationFormats = new List<string>()
            {
                "~/Views/{1}/List/{0}.cshtml",
               
            };

            this.ViewLocationFormats = viewLocationFormats.ToArray();
            
            //this.PartialViewLocationFormats;
            //this.AreaViewLocationFormats
            //this.AreaPartialViewLocationFormats
               
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (viewName == "List")
            {
                viewName = string.Format("ListaDe{0}", controllerContext.Controller.ValueProvider.GetValue("Controller").AttemptedValue);
            }

            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }
    }
}