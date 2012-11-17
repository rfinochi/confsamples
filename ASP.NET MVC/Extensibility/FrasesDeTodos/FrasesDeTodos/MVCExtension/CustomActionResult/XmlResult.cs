using System.Web.Mvc;
using System.Xml.Serialization;

namespace FrasesDeTodos.MVCExtension.CustomActionResult
{
    public class XmlResult : ActionResult
    {
        private object data;

        public XmlResult(object data)
        {
            this.data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var serializer = new XmlSerializer(data.GetType());
            var response = context.HttpContext.Response.OutputStream;

            context.HttpContext.Response.ContentType = "text/xml";
            serializer.Serialize(response, data);
        }
    }
}