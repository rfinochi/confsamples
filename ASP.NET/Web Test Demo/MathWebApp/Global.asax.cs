using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace MathWebApp
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start( object sender, EventArgs e )
        {

        }

        protected void Session_Start( object sender, EventArgs e )
        {

        }

        protected void Application_BeginRequest( object sender, EventArgs e )
        {
            //HttpApplication app = (HttpApplication)sender;

            //string encodings = app.Request.Headers.Get( "Accept-Encoding" );
            //if ( encodings != null )
            //{
            //    encodings = encodings.ToLower( );
            //    if ( encodings.Contains( "gzip" ) )
            //    {
            //        app.Response.Filter = new GZipStream( app.Response.Filter, CompressionMode.Compress );
            //        app.Response.AppendHeader( "Content-Encoding", "gzip" );
            //    }
            //    else if ( encodings.Contains( "deflate" ) )
            //    {
            //        app.Response.Filter = new System.IO.Compression.DeflateStream( app.Response.Filter, CompressionMode.Compress );
            //        app.Response.AppendHeader( "Content-Encoding", "deflate" );
            //    }
            //}
        }

        protected void Application_AuthenticateRequest( object sender, EventArgs e )
        {

        }

        protected void Application_Error( object sender, EventArgs e )
        {

        }

        protected void Session_End( object sender, EventArgs e )
        {

        }

        protected void Application_End( object sender, EventArgs e )
        {

        }
    }
}