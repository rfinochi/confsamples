<%@ Import Namespace="CodeCamp2009Demos.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Attender>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Crear Asistente
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Crear Asistente</h2>
    <%= Html.ValidationSummary("Error al crear el asistente!") %>
    
    <% using (Html.BeginForm())
       {%>
         <%Html.RenderPartial("AttenderFormPartial", Model); %>
    <% } %>
    <div>
        <%=Html.ActionLink("Volver", "Index") %>
    </div>
</asp:Content>
