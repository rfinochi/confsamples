using System.Data.Entity;

namespace MovieIndex.Models
{
    public class MovieIndexContext : DbContext
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

        public DbSet<Director> Directors
        {
            get;
            set;
        }
    }
}