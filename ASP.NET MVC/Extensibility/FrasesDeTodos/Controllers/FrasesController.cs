using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrasesDeTodos.Services;
using FrasesDeTodos.Models;
using FrasesDeTodos.MVCExtension.ActionNameSelectors;
using System.Text;
using FrasesDeTodos.MVCExtension.CustomActionResult;
using FrasesDeTodos.MVCExtension.ActionMethodSelectors;

namespace FrasesDeTodos.Controllers
{
    public class FrasesController : Controller
    {
        public ActionResult Index(string id)
        {
            ViewData["autor"] = id;

            return View("VerFrasesPersona");
        }

        public ActionResult ObtenerTodas(string deQuien)
        {
            var frases = FrasesService.ObtenerFrases(deQuien);

            var result = frases.Select(x => new
            {
                Id = x.FraseId,
                Texto = x.Texto,
                CantidadVotos = x.CantidadVotos,
                Comentarios = x.Comentarios.Select(y => new
                {
                    Id = y.ComentarioId,
                    Texto = y.Texto
                })
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarUnaFrase(string texto, string autor)
        {
            var frase = FrasesService.NuevaFrase(texto, autor);

            FrasesService.NuevaFrase(frase);

            return Json(new
            {
                Id = frase.FraseId,
                Texto = frase.Texto,
                CantidadVotos = 0,
            });
        }

        [HttpPost]
        public int MeGusta(int fraseId)
        {
            return FrasesService.Votar(fraseId, true);
        }

        [HttpPost]
        public int NoMeGusta(int fraseId)
        {
            return FrasesService.Votar(fraseId, false);
        }

        [HttpPost]
        public ActionResult Comentar(int fraseId, string texto)
        {
            var comentario = FrasesService.Comentar(fraseId, texto);

            return Json(new { Id = comentario.ComentarioId, Texto = comentario.Texto });
        }

        [HttpPost]
        public void EliminarComentario(int fraseId, int comentarioId)
        {
            FrasesService.EliminarComentario(fraseId, comentarioId);
        }

        [HttpPost]
        public ActionResult EditarComentario(int fraseId, int comentarioId, string texto)
        {
            var comentario = FrasesService.EditarComentario(fraseId, comentarioId, texto);

            return Json(new { Id = comentario.ComentarioId, Texto = comentario.Texto });
        }
        
        //public ActionResult List()
        //{
        //    return View(FrasesService.ObtenerFrases());
        //}

        //[HttpPost]
        //public ActionResult CargarUnaFrase(Frase frase)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        MostrarError();
        //    }
        //    FrasesService.NuevaFrase(frase);

        //    return Json(new
        //    {
        //        Id = frase.FraseId,
        //        Texto = frase.Texto,
        //        CantidadVotos = 0,
        //    });
        //}

        //[FraseEspecificaAttribute]
        //public ActionResult ObtenerEspecifica(string deQuien, int indice)
        //{
        //    var frases = FrasesService.ObtenerFrases(deQuien);

        //    var result = frases
        //        .ElementAt(indice);

        //    return new XmlResult(result);
        //}

        //[FraseEspecificaAttribute]
        //public ActionResult ObtenerEspecifica(string deQuien, int indice)
        //{
        //    var frases = FrasesService.ObtenerFrases(deQuien);

        //    var result = frases
        //        .ElementAt(indice);

        //    var frase = new
        //    {
        //        Id = result.FraseId,
        //        Texto = result.Texto,
        //        CantidadVotos = result.CantidadVotos,
        //        Comentarios = result.Comentarios.Select(y => new
        //        {
        //            Id = y.ComentarioId,
        //            Texto = y.Texto
        //        })
        //    };

        //    return Json(frase, JsonRequestBehavior.AllowGet);
        //}

        private void MostrarError()
        {
            var messages = new StringBuilder();

            foreach (var modelStateDictionaryEntry in ModelState)
            {
                var modelState = modelStateDictionaryEntry.Value;

                if (modelState.Errors.Count() > 0)
                {
                    foreach (var e in modelState.Errors)
                    {
                        messages.AppendLine(e.ErrorMessage);
                    }
                }
            }

            throw new ApplicationException(messages.ToString());
        }
    }
}