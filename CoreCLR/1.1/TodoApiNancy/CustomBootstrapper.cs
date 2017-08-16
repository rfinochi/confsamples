using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.TinyIoc;

using TodoApi.Models;

namespace TodoApi
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
       public override void Configure(INancyEnvironment environment)
       {
            environment.Tracing(enabled: false, displayErrorTraces: true);
       }
		
       protected override void ConfigureApplicationContainer(TinyIoCContainer container)
       {
            base.ConfigureApplicationContainer(container);

            container.Register<ITodoRepository, MemoryTodoRepository>().AsSingleton();
       }
    }
}
