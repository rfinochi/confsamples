<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    ASP.NET MVC - Demo
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <ul>
            <li>
                <%=Html.ActionLink("Asistentes", "Index", "Attenders")%>
            </li>
            <li>
                <%=Html.ActionLink("Charlas", "Index", "Talks")%>
            </li>
        </ul>
       
    </div>
</asp:Content>
