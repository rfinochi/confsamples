using System;
using System.Diagnostics;
using System.Web.UI;

public partial class _Default : Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        Trace.Write( "Se ha disparado el evento Page_Load" );
    }

    protected void button1_Click( object sender, EventArgs e )
    {
        Trace.Warn( "Se ha disparado el evento Button1_Click" );
    }

    protected void button2_Click( object sender, EventArgs e )
    {
        Response.Redirect( "SecondPage.aspx" );
    }
}