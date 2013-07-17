using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

public partial class OutputCache2 : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        string selectCmd;
        string id = Request[ "id" ];

        if ( id == null )
            selectCmd = "SELECT * FROM Sales.Store";
        else
            selectCmd = "SELECT * FROM Sales.Store WHERE SalesPersonID = " + id; //Error de seguridad

        DbConnection myConnection = null;
        DbCommand myCommand = null;
        DbDataAdapter myAdapter = null;

        try
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory( ConfigurationManager.ConnectionStrings[ "AdventureWorks" ].ProviderName );

            myConnection = factory.CreateConnection( );
            myConnection.ConnectionString = ConfigurationManager.ConnectionStrings[ "AdventureWorks" ].ConnectionString;

            myCommand = factory.CreateCommand( );
            myCommand.CommandText = selectCmd;
            myCommand.Connection = myConnection;

            myAdapter = factory.CreateDataAdapter( );
            myAdapter.SelectCommand = myCommand;

            DataSet ds = new DataSet( );
            myAdapter.Fill( ds, "Store" );

            myDataGrid.DataSource = new DataView( ds.Tables[ 0 ] );
            myDataGrid.DataBind( );
        }
        catch
        {
            if ( myAdapter != null )
            {
                myAdapter.Dispose( );
                myAdapter = null;
            }

            if ( myCommand != null )
            {
                myCommand.Dispose( );
                myCommand = null;
            }

            if ( myConnection != null )
            {
                if ( myConnection.State == ConnectionState.Open )
                    myConnection.Close( );
                myConnection.Dispose( );
                myConnection = null;
            }

            throw;
        }

        timeMsg.Text = DateTime.Now.ToString( "G" );
    }
}