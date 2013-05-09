using System.Web.Mvc;

using MovieIndex.Models;

namespace MovieIndex.Controllers
{
    public class HomeController : Controller
    {
        private IMovieRepository _movieRepository;
        private IGenreRepository _genreRepository;

        public HomeController( IMovieRepository movieRepository, IGenreRepository genreRepository )
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }

        public ActionResult Index( )
        {
            return View( _movieRepository.GetAll( ) );
        }

        public ActionResult Details( int id = 0 )
        {
            Movie movie = _movieRepository.GetById( id );
            
            if ( movie == null )
                return HttpNotFound( );

            return View( movie );
        }

        public ActionResult Create( )
        {
            ViewBag.GenreId = new SelectList( _genreRepository.GetAll( ), "Id", "Description" );
            return View( );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Movie movie )
        {
            if ( ModelState.IsValid )
            {
                _movieRepository.Create( movie );

                return RedirectToAction( "Index" );
            }

            ViewBag.GenreId = new SelectList( _genreRepository.GetAll( ), "Id", "Description" );
            return View( movie );
        }

        public ActionResult Edit( int id )
        {
            Movie movie = _movieRepository.GetById( id );
            if ( movie == null )
                return HttpNotFound( );

            ViewBag.GenreId = new SelectList( _genreRepository.GetAll( ), "Id", "Description" );
            return View( movie );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Movie movie )
        {
            if ( ModelState.IsValid )
            {
                _movieRepository.Update( movie );
                return RedirectToAction( "Index" );
            }

            ViewBag.GenreId = new SelectList( _genreRepository.GetAll( ), "Id", "Description" );
            return View( movie );
        }

        public ActionResult Delete( int id )
        {
            Movie movie = _movieRepository.GetById( id );
            if ( movie == null )
                return HttpNotFound( );

            return View( movie );
        }

        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed( int id )
        {
            _movieRepository.Delete( id );
            return RedirectToAction( "Index" );
        }
    }
}