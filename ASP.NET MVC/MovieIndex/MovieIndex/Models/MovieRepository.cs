using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace MovieIndex.Models
{
    public class MovieRepository : IMovieRepository
    {
        public List<Movie> GetAll( )
        {
            using ( MovieIndexContext context = new MovieIndexContext( ) )
            {
                return ( from m in context.Movies.Include( m => m.Genre ) 
                         select m ).ToList( );
            }
        }

        public List<Movie> GetByName( string name )
        {
            using ( MovieIndexContext context = new MovieIndexContext( ) )
            {
                return ( from m in context.Movies.Include( m => m.Genre )
                         where m.Name.StartsWith( name )
                         select m ).ToList( );
            }
        }
        
        public Movie GetById( int id )
        {
            using ( MovieIndexContext context = new MovieIndexContext( ) )
            {
                return ( from m in context.Movies
                         where m.Id == id
                         select m ).SingleOrDefault( );
            }
        }

        public void Create( Movie movie )
        {
            using ( MovieIndexContext context = new MovieIndexContext( ) )
            {
                context.Movies.Add( movie );

                context.SaveChanges( );
            }
        }
        
        public void Update( Movie movie )
        {
            using ( MovieIndexContext context = new MovieIndexContext( ) )
            {
                context.Entry( movie ).State = EntityState.Modified;

                context.SaveChanges( );
            }
        }
    
        public void Delete( int id )
        {
            using ( MovieIndexContext context = new MovieIndexContext( ) )
            {
                Movie movie = context.Movies.Find( id );
                context.Movies.Remove( movie );

                context.SaveChanges( );
            }
        }
    }
}