using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MovieIndex
{
    [HubName( "movieIndex" )]
    public class MovieIndexHub : Hub { }
}