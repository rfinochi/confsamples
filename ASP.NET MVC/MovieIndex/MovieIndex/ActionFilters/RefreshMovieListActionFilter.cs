using System.Web.Mvc;

using Microsoft.AspNet.SignalR;

namespace MovieIndex.ActionFilters
{
    public class RefreshMovieListActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuted( ResultExecutedContext filterContext )
        {
            base.OnResultExecuted( filterContext );

            var context = GlobalHost.ConnectionManager.GetHubContext<MovieIndexHub>( );
            context.Clients.All.changeMovieIndex( null );
        }
    }
}