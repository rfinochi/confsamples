<%@ Import Namespace="CodeCamp2009Demos.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Attender>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Editar Asistente
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Editar Asistente</h2>
    <%= Html.ValidationSummary() %>
    <% using (Html.BeginForm())
       {%>
              <%Html.RenderPartial("AttenderFormPartial", Model); %>
    <% } %>
    
    <div>
        <%=Html.ActionLink("Volver", "Index") %>
    </div>
</asp:Content>
