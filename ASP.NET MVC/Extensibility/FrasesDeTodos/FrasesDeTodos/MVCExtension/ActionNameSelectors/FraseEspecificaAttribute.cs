using System;
using System.Reflection;
using System.Web.Mvc;

namespace FrasesDeTodos.MVCExtension.ActionNameSelectors
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class FraseEspecificaAttribute : ActionNameSelectorAttribute
    {
        private const string PREFIX = "frase-";

        public override bool IsValidName(ControllerContext controllerContext,
                                         string actionName,
                                         MethodInfo methodInfo)
        {
            if (actionName == "ObtenerEspecifica")
                return true;

            if (!actionName.StartsWith(PREFIX, StringComparison.InvariantCultureIgnoreCase))
                return false;

            var info = actionName.Substring(PREFIX.Length).Split('-');

            controllerContext.RequestContext.RouteData.Values.Add("deQuien", info[0]);
            controllerContext.RequestContext.RouteData.Values.Add("indice", info[1]);

            return true;
        }
    }
}