<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CodeCamp2009Demos.Models.Talk>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Detail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Detalle</h2>

    <fieldset>
        <p>
            Título:
            <%= Html.Encode(Model.Title) %>
        </p>
        <p>
            Descripción:
            <%= Html.Encode(Model.Description) %>
        </p>
        <p>
            Orador:
            <%= Html.Encode(Model.Speaker) %>
        </p>
    </fieldset>
    <p>
        <%=Html.ActionLink("Volver", "Index") %>
    </p>

</asp:Content>

