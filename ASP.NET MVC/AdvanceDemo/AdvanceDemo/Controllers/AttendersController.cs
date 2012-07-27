using System.Web.Mvc;

using CodeCamp2009Demos.Models;

namespace CodeCamp2009Demos.Controllers
{
    public class AttendersController : Controller
    {
        //
        // GET: /Attender/

        public ActionResult Index()
        {
            return View(AttenderRepository.GetAttenders());
        }

        //
        // GET: /Attender/

        public ActionResult SearchAjax(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return PartialView("AttendersListPartial", AttenderRepository.GetAttenders());
            else
                return PartialView("AttendersListPartial", AttenderRepository.GetAttenders(filter));
        }

        public ActionResult Search(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return View("Index", AttenderRepository.GetAttenders());
            else
                return View("Index", AttenderRepository.GetAttenders(filter));
        }

        //
        // GET: /Attender/Details/5

        public ActionResult Details(int id)
        {
            return View(AttenderRepository.GetAttender(id));
        }

        //
        // GET: /Attender/Create

        public ActionResult Create()
        {
            ViewData["TalksList"] = GetTalksList();
 
            return View();
        } 

        //
        // POST: /Attender/Create

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Create([Bind(Exclude = "Id")] Attender attender)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(attender.Name))
        //            ModelState.AddModelError("Name", "El nombre es un campo obligatorio.");

        //        if (attender.Age == 0)
        //            ModelState.AddModelError("Age", "La edad es un campo obligatorio.");

        //        if (!ModelState.IsValid)
        //        {
        //            ViewData["TalksList"] = GetTalksList(attender.TalksIds);
        //            return View(attender);
        //        }

        //        AttenderRepository.AddAttender(attender);

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Create(FormCollection formValues)
        //{
        //    try
        //    {
        //        var attender = new Attender();

        //        UpdateModel(attender);

        //        if (string.IsNullOrEmpty(attender.Name))
        //            ModelState.AddModelError("Name", "El nombre es un campo obligatorio.");
                
        //        if (attender.Age == 0)
        //            ModelState.AddModelError("Age", "La edad es un campo obligatorio.");
                     
        //        if (!ModelState.IsValid)
        //        {
        //            ViewData["TalksList"] = GetTalksList(attender.TalksIds);
        //            return View();
        //        }

        //        AttenderRepository.AddAttender(attender);

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        ViewData["TalksList"] = GetTalksList();

        //        return View();
        //    }
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string Name, int? Age, string Comments, string Profesion, bool ReturnNextYear, int[] TalksIds)
        {
            try
            {
                var attender = new Attender();

                if (string.IsNullOrEmpty(Name))
                    ModelState.AddModelError("Name", "El nombre es un campo obligatorio.");
                else
                    attender.Name = Name;


                if (!Age.HasValue || Age.Value == 0)
                    ModelState.AddModelError("Age", "La edad es un campo obligatorio.");
                else
                    attender.Age = Age.Value;

                attender.Comments = Comments;
                attender.Profesion = Profesion;
                attender.ReturnNextYear = ReturnNextYear;
                attender.TalksIds = TalksIds;

                if (!ModelState.IsValid)
                {
                    ViewData["TalksList"] = GetTalksList(attender.TalksIds);
                    return View();
                }

                AttenderRepository.AddAttender(attender);

                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["TalksList"] = GetTalksList();

                return View();
            }
        }

        //
        // GET: /Attender/Edit/5

        public ActionResult Edit(int id)
        {
            var attender = AttenderRepository.GetAttender(id);

            ViewData["TalksList"] = GetTalksList(attender.TalksIds);

            return View(AttenderRepository.GetAttender(id));
        }

        //
        // POST: /Attender/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Attender attender)
        {
            try
            {
                if (string.IsNullOrEmpty(attender.Name))
                    ModelState.AddModelError("Name", "El nombre es un campo obligatorio.");

                if (attender.Age == 0)
                    ModelState.AddModelError("Age", "La edad es un campo obligatorio.");

                if (!ModelState.IsValid)
                {
                    ViewData["TalksList"] = GetTalksList(attender.TalksIds);

                    return View();
                }

                var currentAttender = AttenderRepository.GetAttender(attender.Id);

                currentAttender.Name = attender.Name;
                currentAttender.Age = attender.Age;
                currentAttender.Comments = attender.Comments;
                currentAttender.ReturnNextYear = attender.ReturnNextYear;
                currentAttender.TalksIds = attender.TalksIds;

                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["TalksList"] = GetTalksList(attender.TalksIds);

                return View();
            }
        }

        /// <summary>
        /// Autocomplete method for Profesions list
        /// </summary>
        /// <param name="q"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult AutoCompleteProfesion(string q, int limit)
        {
            var repository = new ProfesionRepository();

            return Json(repository.GetProfesiones(q));
        }

        /// <summary>
        /// Get the Multiselect list for the tasks ListBox
        /// </summary>
        /// <param name="selectedValues"></param>
        /// <returns></returns>
        private static MultiSelectList GetTalksList()
        {
            return GetTalksList(null);
        }

        /// <summary>
        /// Get the Multiselect list for the tasks ListBox
        /// </summary>
        /// <param name="selectedValues"></param>
        /// <returns></returns>
        private static MultiSelectList GetTalksList(int[] selectedTasks)
        {
            var talksRepository = new TalksRepository();

            var talks = TalksRepository.GetTalks();

            return new MultiSelectList(talks, "Id", "Title", selectedTasks);
        }

    }
}
