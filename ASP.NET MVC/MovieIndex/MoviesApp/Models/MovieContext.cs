using System.Data.Entity;

namespace MoviesApp.Models
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies
        {
            get;
            set;
        }

        public DbSet<Genre> Genres
        {
            get;
            set;
        }
    }
}