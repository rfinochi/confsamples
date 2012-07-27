using System.Threading.Tasks;

using SignalR;
using SignalR.Hosting;

public class MyConnection : PersistentConnection
{
    protected override Task OnReceivedAsync( IRequest request, string connectionId, string data )
    {
        return Connection.Broadcast( data );
    }
}