using Microsoft.AspNet.SignalR;

public class Echo : Hub
{
    public void Send( string message )
    {
        Clients.All.addMessage( message );
    }
}