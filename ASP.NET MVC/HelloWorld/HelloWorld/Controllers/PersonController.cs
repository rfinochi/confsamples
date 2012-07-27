using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    public class PersonController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Details( string name, string surname, int day, int month )
        {
            return View( new PersonViewModel( ) 
                            {
                                Name = name,
                                Surname = surname, 
                                Day = day, 
                                Month = month } );
        }

        [AcceptVerbs( HttpVerbs.Post )]
        public ActionResult Create( PersonViewModel person )
        {
            if ( person.Name == "XXXX" )
                ModelState.AddModelError( "Name", "El nombre no puede ser XXX" );

            if(ModelState.IsValid)
            {

                return RedirectToAction( "Details",
                                            new
                                            {
                                                name = person.Name,
                                                surname = person.Surname,
                                                day = person.Day,
                                                month = person.Month
                                            } );
            }
            else
            {
                return View();
            }
        }

        //[AcceptVerbs( HttpVerbs.Post )]
        //public ActionResult Create( FormCollection formValues )
        //{
        //    return RedirectToAction( "Details",
        //                                new
        //                                {
        //                                    name = formValues[ "Name" ],
        //                                    surname = formValues[ "Surname" ],
        //                                    day = formValues[ "Day" ],
        //                                    month = formValues[ "Month" ],
        //                                } );
        //}

        //[AcceptVerbs( HttpVerbs.Post )]
        //public ActionResult Create( string name, string surname, int day, int month )
        //{
        //    return RedirectToAction( "Details",
        //                                new
        //                                {
        //                                    name = name,
        //                                    surname = surname,
        //                                    day = day,
        //                                    month = month
        //                                } );
        //}
    }
}
