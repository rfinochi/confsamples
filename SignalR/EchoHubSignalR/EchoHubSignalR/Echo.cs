using SignalR.Hubs;

public class Echo : Hub
{
    public void Send( string message )
    {
        Clients.addMessage( message );
    }
}