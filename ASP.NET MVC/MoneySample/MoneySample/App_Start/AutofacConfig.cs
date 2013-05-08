using System.Reflection;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

using MoneySample.Models;

namespace MoneySample
{
    public class AutofacConfig
    {
        #region Public Static Methods

        public static void RegisterDependencies( )
        {
            var builder = new ContainerBuilder( );
            builder.RegisterControllers( Assembly.GetExecutingAssembly( ) );

            builder.RegisterType<DollarRepository>( ).As<IDollarRepository>( );

            IContainer container = builder.Build( );
            DependencyResolver.SetResolver( new AutofacDependencyResolver( container ) );
        }

        #endregion
    }
}