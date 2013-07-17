<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OutputCache2.aspx.cs" Inherits="OutputCache2" %>
<%@ OutputCache Duration="60" VaryByParam="id" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Caching Demo - Output Cache 2</title>
</head>
  <body>
    <h3><font face="Verdana">Usando el output cache</font></h3>

    <p>

    <i>Generado a las:</i> <asp:label id="timeMsg" runat="server"/>

    <p>

    <b>Tiendas por region:</b>

    <table cellspacing="0" cellpadding="3" rules="all" style="background-color:#AAAADD;border-color:black;border-color:black;width:700px;border-collapse:collapse;">
        <tr>
          <td><a href="outputcache2.aspx?id=277">277</a></td>
          <td><a href="outputcache2.aspx?id=275">275</a></td>
          <td><a href="outputcache2.aspx?id=281">281</a></td>
          <td><a href="outputcache2.aspx?id=279">279</a></td>
          <td><a href="outputcache2.aspx?id=282">282</a></td>
      </tr>
    </table>

    <p>

    <ASP:DataGrid id="myDataGrid" runat="server"
      Width="700"
      BackColor="#ccccff"
      BorderColor="black"
      ShowFooter="false"
      CellPadding=3
      CellSpacing="0"
      Font-Name="Verdana"
      Font-Size="8pt"
      HeaderStyle-BackColor="#aaaadd"
    />

  </body>
</html>
