using System.Web.Mvc;

using MoviesApp.Models;

namespace MoviesApp.Controllers
{
    public class HomeController : Controller
    {
        private IMovieRepository _repository;

        public HomeController( ) : this( new MovieRepository( ) ) { }

        public ActionResult Index( )
        {
            return RedirectToAction( "List" );
        }

        public HomeController( IMovieRepository repository )
        {
            _repository = repository;
        }
        
        public ActionResult List( )
        {
            return View( _repository.GetAll( ) );
        }

        public ActionResult Create( )
        {
            return View( );
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create( Movie pelicula )
        {
            _repository.Create( pelicula );

            return RedirectToAction( "List" );
        }                       
    }
}