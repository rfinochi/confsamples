using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

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
            builder.RegisterType<DirectorRepository>( ).As<IDirectorRepository>( );

            DependencyResolver.SetResolver( new AutofacDependencyResolver( builder.Build( ) ) );

            var builderApi = new ContainerBuilder( );

            builderApi.RegisterApiControllers( Assembly.GetExecutingAssembly( ) );

            builderApi.RegisterType<DirectorRepository>( ).As<IDirectorRepository>( );
            builderApi.RegisterType<MovieRepository>( ).As<IMovieRepository>( );

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver( builderApi.Build( ) );
        }
    }
}