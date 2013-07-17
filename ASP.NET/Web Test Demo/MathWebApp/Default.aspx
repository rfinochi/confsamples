<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MathWebApp._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="textBoxX" runat="server" Width="46px"></asp:TextBox><br />
        <asp:TextBox ID="textBoxY" runat="server" Width="46px"></asp:TextBox><asp:Button ID="buttonSum" runat="server" Text="Sum" onclick="buttonSum_Click"></asp:Button><br />
        <asp:TextBox ID="textBoxResult" runat="server" ReadOnly="True" Width="46px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
