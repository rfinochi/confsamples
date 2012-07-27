<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HelloWorld.Models.PersonViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        
        <div class="display-label">Name</div>
        <div class="display-field"><%= Html.Encode(Model.Name) %></div>
        
        <div class="display-label">Surname</div>
        <div class="display-field"><%= Html.Encode(Model.Surname) %></div>
        
        <div class="display-label">Day</div>
        <div class="display-field"><%= Html.Encode(Model.Day) %></div>
        
        <div class="display-label">Month</div>
        <div class="display-field"><%= Html.Encode(Model.Month) %></div>
        
    </fieldset>
</asp:Content>