<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataCache.aspx.cs" Inherits="DataCache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Caching Demo - Data Cache</title>
</head>
  <body>
    <form id="Form1" method="GET" runat="server">

      <h3><font face="Verdana">Caching Data</font></h3>

      <ASP:DataGrid id="myDataGrid" runat="server"
        Width="700"
        BackColor="#ccccff"
        BorderColor="black"
        ShowFooter="false"
        CellPadding=3
        CellSpacing="0"
        Font-Name="Verdana"
        Font-Size="8pt"
        HeaderStyle-BackColor="#aaaad" />

      <p>

      <i><asp:label id="cacheMsg" runat="server"/></i>

    </form>
  </body>
</html>