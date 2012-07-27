using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CodeCamp2009Demos.Models;

namespace CodeCamp2009Demos.Controllers
{
    public class TalksController : Controller
    {
        //
        // GET: /Talks/
        public ActionResult Index()
        {
            this.ViewData["talks"] = (from t in TalksRepository.GetTalks()
                                     orderby t.Id descending
                                     select t).ToArray();

            return this.View();
        }

        //
        // GET: /Talks/Details/5
        public ActionResult Details(string title)
        {
            return View(TalksRepository.GetTalkByTitle(title));
        }

        // GET: /Talks/Create
        public ActionResult Create()
        {
            this.ViewData["categories"] = TalksRepository.GetCategories();
            return View();
        } 

        // POST: /Talks/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string title, string speaker, int categoryId, string description)
        {
            try
            {
                var talk = new Talk 
                { 
                    Description = description,
                    Speaker = speaker,
                    Title = title,
                    Category = TalksRepository.GetCategoryById(categoryId),
                };

                TalksRepository.Add(talk);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetTalks()
        {
            var talks = TalksRepository.GetTalks();

            return Json(talks);
        }
    }
}
