using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrasesDeTodos.Repositories;

namespace FrasesDeTodos.Controllers
{
    public class FrasesController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("VerFrasesPersona", new { id = "Zaiden" });
        }

		public ActionResult VerFrasesPersona(string id)
		{
			ViewData["autor"] = id;
			ViewData.Model = FrasesRepository.ObtenerFrases(id);
			return View();
		}

		[HttpPost]
		public ActionResult ObtenerTodas(string deQuien)
		{
			return Json(FrasesRepository.ObtenerFrases(deQuien));
		}

		[HttpPost]
		public ActionResult CargarUnaFrase(string frase, string deQuien)
		{
			var f = FrasesRepository.NuevaFrase(frase, deQuien);
			return Json(f);
		}

		[HttpPost]
		public void MeGusta(int idFrase)
		{
			FrasesRepository.Votar(idFrase);
		}

		[HttpPost]
		public void Comentar(int idFrase, string comentario)
		{
			FrasesRepository.Comentar(idFrase, comentario);
		}
    }
}
