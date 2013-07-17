<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="OutputCache1.aspx.cs" Inherits="_Default" %>
<%@ OutputCache Duration="10" VaryByParam="none" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Caching Demo - Output Cache 1</title>
</head>
<body>
    <form id="form1" runat="server">
        <h3><font face="Verdana">Output Cache</font></h3>

        <p><i>Generado a las:</i> <asp:label id="timeMsg" runat="server"/>
    </form>
</body>
</html>
