using System;
using System.Data.Entity.Migrations;

using MovieIndex.Models;

namespace MovieIndex.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MovieIndexContext>
    {
        public Configuration( )
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed( MovieIndexContext context )
        {
            Genre genre1 = new Genre { Id = 1, Description = "Thriller" };
            Genre genre2 = new Genre { Id = 2, Description = "Horror" };
            Genre genre3 = new Genre { Id = 3, Description = "Suspense" };
            Genre genre4 = new Genre { Id = 4, Description = "Drama" };

            context.Genres.AddOrUpdate( g => g.Id, genre1, genre2, genre3, genre4 );

            Director director1 = new Director { Id = 1, FullName = "Alfred Hitchcock" };
            Director director2 = new Director { Id = 2, FullName = "Quentin Tarantino" };
            Director director3 = new Director { Id = 3, FullName = "Robert Rodriguez" };
            Director director4 = new Director { Id = 4, FullName = "Roberto Sanchez" };

            context.Directors.AddOrUpdate( d => d.Id, director1, director2, director3, director4 );

            Movie movie1 = new Movie { Id = 1, Name = "Vertigo", DirectorName = "Alfred Hitchcock", Genre = genre1, ReleaseDate = DateTime.Now, Rating = 5 };
            Movie movie2 = new Movie { Id = 2, Name = "Pulp Fiction", DirectorName = "Quentin Tarantino", Genre = genre4, ReleaseDate = DateTime.Now, Rating = 3 };

            context.Movies.AddOrUpdate( m => m.Id, movie1, movie2 );
        }
    }
}