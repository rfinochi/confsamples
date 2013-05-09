using System.Collections.Generic;

namespace MoviesApp.Models
{
    public interface IMovieRepository
    {
        List<Movie> GetAll( );

        void Create( Movie newMovie );
    }
}