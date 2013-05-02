using System.Web.Mvc;

using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    public class FooController : Controller
    {
        //public ActionResult Hello( string name )
        //{
        //    //ViewData[ "Name" ] = name;

        //    ViewBag.Name = name;

        //    return View( );
        //}

        //public void Hello( string name )
        //{
        //    Response.Write( "Hello " + name );
        //}

        public ActionResult HelloForViewModel( string name, string surname )
        {
            HelloViewModel result = new HelloViewModel( ) { Name = name, Surname = surname };

            ViewData.Model = result;

            return View( result );
        }
    }
}