using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace Lagash.ServiceModel
{
    internal class MemoryVirtualFile : VirtualFile
    {
        public MemoryVirtualFile( string virtualPath ) : base( virtualPath ) { }

        public override Stream Open( )
        {
            string file = @"<%@ Page Language='C#' AutoEventWireup='true' CodeBehind='Default.aspx.cs' Inherits='Lagash.ServiceModel.Default' %>
                            <!DOCTYPE html>
                            <html xmlns='http://www.w3.org/1999/xhtml'>
                            <head runat='server'>
                                <title></title>
                            </head>
                            <body>
                                <form id='form1' runat='server'>
                                <div>
    
                                </div>
                                    <asp:TextBox ID='TextBox1' runat='server'>Hello World Ghost</asp:TextBox>
                                </form>
                            </body>
                            </html>
                            ";

            return new MemoryStream( ASCIIEncoding.Default.GetBytes( file ) );
        }
    }
}