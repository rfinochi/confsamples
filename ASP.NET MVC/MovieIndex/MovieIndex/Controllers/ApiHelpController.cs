using System.Web.Http;
using System.Web.Mvc;

namespace MovieIndex.Controllers
{
    public class ApiHelpController : Controller
    {
        public ActionResult Index()
        {
            return View( GlobalConfiguration.Configuration.Services.GetApiExplorer( ) );
        }
    }
}
