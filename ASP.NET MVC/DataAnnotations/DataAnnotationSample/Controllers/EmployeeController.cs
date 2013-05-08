using System.Web.Mvc;

using DataAnnotationSample.Models;

namespace DataAnnotationSample.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Index( )
        {
            return View( );
        }

        [HttpPost]
        public ActionResult Index( EmployeeModel objEmployeeModel )
        {
            return View( );
        }
    }
}