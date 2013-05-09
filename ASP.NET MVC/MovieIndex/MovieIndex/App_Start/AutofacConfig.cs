using System.Reflection;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

using MovieIndex.Models;

namespace MovieIndex
{
    public class AutofacConfig
    {
        public static void RegisterDependencies( )
        {
            var builder = new ContainerBuilder( );
            builder.RegisterControllers( Assembly.GetExecutingAssembly( ) );

            builder.RegisterType<MovieRepository>( ).As<IMovieRepository>( );
            builder.RegisterType<GenreRepository>( ).As<IGenreRepository>( );

            IContainer container = builder.Build( );
            DependencyResolver.SetResolver( new AutofacDependencyResolver( container ) );
        }
    }
}