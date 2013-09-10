using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoRazor.Models;

namespace DemoRazor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Manzana = "Una manzana roja";
            ViewBag.Message = "Razor \"Rulea\"!";

            return View();
        }

        #region Support code

        public ActionResult ListPeople()
        {
            List<Persona> personas = new List<Persona>()
                        {
                            new Persona {
                                Nombre = "Rodolfo",
                                Apellido = "Finochietti",
                                Pais = "Argentina"
                            },
                            new Persona {
                                Nombre = "Paolo",
                                Apellido = "Rocca",
                                Pais = "Italia"
                            },
                        };

            //Le mando la lista de personas a la vista
            ViewBag.Personas = personas;

            return View();
        }

        #endregion
    }
}