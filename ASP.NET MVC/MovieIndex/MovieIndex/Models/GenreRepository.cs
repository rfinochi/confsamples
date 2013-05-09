using System.Collections.Generic;
using System.Linq;

namespace MovieIndex.Models
{
    public class GenreRepository : IGenreRepository
    {
        public List<Genre> GetAll( )
        {
            using ( MovieIndexContext context = new MovieIndexContext( ) )
            {
                return ( from g in context.Genres
                         select g ).ToList( );
            }
        }
    }
}