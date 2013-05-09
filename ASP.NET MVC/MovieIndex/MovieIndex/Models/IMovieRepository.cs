using System.Collections.Generic;

namespace MovieIndex.Models
{
    public interface IMovieRepository
    {
        List<Movie> GetAll( );

        Movie GetById( int id );

        void Create( Movie movie );

        void Update( Movie movie );

        void Delete( int id );
    }
}