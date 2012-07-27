<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HelloWorld.Models.PersonViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Name) %>
                <%= Html.ValidationMessageFor(model => model.Name) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Surname) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Surname) %>
                <%= Html.ValidationMessageFor(model => model.Surname) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Day) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Day) %>
                <%= Html.ValidationMessageFor(model => model.Day) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Month) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Month) %>
                <%= Html.ValidationMessageFor(model => model.Month) %>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%= Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

