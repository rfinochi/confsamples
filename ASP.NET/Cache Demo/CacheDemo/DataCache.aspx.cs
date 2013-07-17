using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

public partial class DataCache : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        DataView Source;

        Source = (DataView)Cache[ "MyDataSet" ];

        if ( Source == null )
        {
            DbConnection myConnection = null;
            DbCommand myCommand = null;
            DbDataAdapter myAdapter = null;

            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory( ConfigurationManager.ConnectionStrings[ "AdventureWorks" ].ProviderName );

                myConnection = factory.CreateConnection( );
                myConnection.ConnectionString = ConfigurationManager.ConnectionStrings[ "AdventureWorks" ].ConnectionString;

                myCommand = factory.CreateCommand( );
                myCommand.CommandText = "SELECT TOP 10 * FROM Sales.Store";
                myCommand.Connection = myConnection;

                myAdapter = factory.CreateDataAdapter( );
                myAdapter.SelectCommand = myCommand;

                DataSet ds = new DataSet( );
                myAdapter.Fill( ds, "Store" );

                Source = new DataView( ds.Tables[ 0 ] );
                Cache[ "MyDataSet" ] = Source;

                cacheMsg.Text = "Dataset creado explicitamente";
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
        }
        else
        {
            cacheMsg.Text = "Dataset obtenido desde el cache";
        }

        myDataGrid.DataSource = Source;
        myDataGrid.DataBind( );
    }
}