using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mime;

namespace FrasesDeTodos.MVCExtension.ExceptionFilters
{
    public class PlainTextExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                var response = filterContext.RequestContext.HttpContext.Response;
                response.Write(filterContext.Exception.Message);
                response.ContentType = MediaTypeNames.Text.Plain;
                response.End();
            }
            else
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult() { ViewName = "Error" };
            }
        }
    }
}