<%@ Import Namespace="CodeCamp2009Demos.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Attender>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Detalle Asistente
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Detalle Asistente</h2>
    <fieldset>
        <p>
            <label for="Nombre">
                Nombre:
                <%= Html.Encode(Model.Name) %>
            </label>
        </p>
        <p>
            <label for="Edad">
                Edad:
                <%= Html.Encode(Model.Age) %>
            </label>
        </p>
        <p>
            <label for="Profesión">
                Profesión:
                <%= Html.Encode(Model.Profesion) %>
            </label>
        </p>
        <p>
            <label for="VolveraElAnioQueViene">
                Volverá el año que viene:
                <% if (Model.ReturnNextYear)
                   {%>
                Si
                <% }
                   else
                   {  %>
                No
                <%} %>
            </label>
        </p>
        <p>
            <label for="Comentarios">
                Comentarios:
                <%= Html.Encode(Model.Comments) %>
            </label>
        </p>
    </fieldset>
    <p>
        <%=Html.ActionLink("Editar", "Edit", new { id = Model.Id } )%>
        |
        <%=Html.ActionLink("Volver", "Index")%>
    </p>
</asp:Content>
