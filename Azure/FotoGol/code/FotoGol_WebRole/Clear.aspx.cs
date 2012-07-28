using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FotoGol_WebRole
{
    public partial class Clear : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (TextBox1.Text == "Password11")
                {
                    FotoGol_Data.FotoGolEntryDataSource ds = new FotoGol_Data.FotoGolEntryDataSource();
                    ds.DeleteAll();

                    Response.Redirect("~/");
                }
            }
        }
    }
}
