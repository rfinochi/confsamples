<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	TaskList
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Charlas</h2>
    
    <dl id="talks">
        <% foreach (var talk in (CodeCamp2009Demos.Models.Talk[])this.ViewData["talks"]) { %>
            <dt>
                <%= Html.Encode(talk.Title) %>
            </dt>
            
            <dd>
                <p><%= Html.Encode(talk.Description) %></p>
                <%= Html.ActionLink("Mas Info...", "Details", new { title = talk.Title }) %>
            </dd>
        <% } %>
    </dl>

    <div>
        <%=Html.ActionLink("Nueva", "Create") %>
    </div>
</asp:Content>
