using System;
using System.Web.UI;

namespace MathWebApp
{
    public partial class _Default : Page
    {
        protected void Page_Load( object sender, EventArgs e ) { }

        protected void buttonSum_Click( object sender, EventArgs e )
        {
            textBoxResult.Text = Math.Sum( Convert.ToInt32( textBoxX.Text ), Convert.ToInt32( textBoxY.Text ) ).ToString( );
        }
    }
}