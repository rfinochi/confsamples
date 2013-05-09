using System;
using System.Data.Entity.Migrations;

using MoviesApp.Models;

namespace MovieApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MovieContext>
    {
        public Configuration( )
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed( MovieContext context )
        {
            Genre genre1 = new Genre { Id = 1, Description = "Thriller" };
            Genre genre2 = new Genre { Id = 2, Description = "Horror" };
            Genre genre3 = new Genre { Id = 3, Description = "Suspense" };
            Genre genre4 = new Genre { Id = 4, Description = "Drama" };

            context.Genres.AddOrUpdate( g => g.Id, genre1, genre2, genre3, genre4 );

            Movie movie1 = new Movie { Id = 1, Name = "Vertigo", Genre = genre3, ReleaseDate = DateTime.Now, Rating = 10 };
            Movie movie2 = new Movie { Id = 2, Name = "Pulp Fiction", Genre = genre4, ReleaseDate = DateTime.Now, Rating = 10 };

            context.Movies.AddOrUpdate( m => m.Id, movie1, movie2 );
        }
    }
}