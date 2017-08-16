using anotheraspnetapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace anotheraspnetapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPictureService _pictureService;

        public HomeController (IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        public IActionResult Index()
        {
           ViewData["Title"] = "Home"; 
	   return View("Index", _pictureService.GetRandomPicture());
        }
    }
}
