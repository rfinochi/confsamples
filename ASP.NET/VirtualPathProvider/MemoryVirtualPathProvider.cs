using System.IO;
using System.Web.Hosting;

namespace Lagash.ServiceModel
{
    public class ServiceModelVirtualPathProvider : VirtualPathProvider
    {
        public ServiceModelVirtualPathProvider( ) : base( ) { }

        public override bool FileExists( string virtualPath )
        {
            if ( virtualPath.Contains( "Ghost.aspx" ) )
                return true;
            else
                return this.Previous.FileExists( virtualPath );
        }

        public override VirtualFile GetFile( string virtualPath )
        {
            if ( virtualPath.Contains( "Ghost.aspx" ) )
                return new MemoryVirtualFile( virtualPath );
            else
                return Previous.GetFile( virtualPath );
        }
    }
}