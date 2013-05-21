using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

using MovieIndex.Models;

namespace MovieIndex.Controllers
{
    public class MoviesController : ApiController
    {
        private IMovieRepository _movieRepository;

        public MoviesController( IMovieRepository movieRepository )
        {
            _movieRepository = movieRepository;
        }

        public IEnumerable<Movie> GetMovies( )
        {
            return _movieRepository.GetAll( );
        }

        //[Queryable]
        //public IQueryable<Movie> Get( )
        //{
        //    return _movieRepository.GetAll( ).AsQueryable( );
        //}
        
        public Movie GetMovie( int id )
        {
            Movie movie = _movieRepository.GetById( id );
            if ( movie == null )
                throw new HttpResponseException( HttpStatusCode.NotFound );

            return movie;
        }

        //public HttpResponseMessage GetMovie( int id )
        //{
        //    Movie movie = _movieRepository.GetById( id );
        //    if ( movie == null )
        //        throw new HttpResponseException( HttpStatusCode.NotFound );

        //    IContentNegotiator negotiator = this.Configuration.Configuration.Services.GetContentNegotiator( );

        //    ContentNegotiationResult result = negotiator.Negotiate( typeof( Movie ), this.Request, this.Configuration.Formatters );
        //    if ( result == null )
        //    {
        //        var response = new HttpResponseMessage( HttpStatusCode.NotAcceptable );
        //        throw new HttpResponseException( response );
        //    }

        //    return new HttpResponseMessage( )
        //    {
        //        Content = new ObjectContent<Movie>(
        //            movie,
        //            result.Formatter,
        //            result.MediaType.MediaType
        //        )
        //    };
        //}
    }
}