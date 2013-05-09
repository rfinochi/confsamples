using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesApp.Models
{
    public class MovieRepository : IMovieRepository
    {
        public List<Movie> GetAll( )
        {
            using ( MovieContext context = new MovieContext( ) )
            {
                return ( from movie in context.Movies
                         select movie ).ToList( );
            }
        }

        public void Create( Movie newMovie )
        {
            using ( MovieContext context = new MovieContext( ) )
            {
                context.Movies.Add( newMovie );

                context.SaveChanges( );
            }
        }
    }
}