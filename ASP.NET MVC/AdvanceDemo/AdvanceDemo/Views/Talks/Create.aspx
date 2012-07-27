<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create a New Talk</h2>
    <form id="talksForm" method="post" action="/Talks/Create">
    <fieldset title="Create a New Talk">
        <div class="columns">
            <div>
                <label for="title">Título</label>
                <input type="text" id="title" name="title" />
            </div>
            <div>
                <label for="title">Orador</label>
                <input type="text" id="speaker" name="speaker" />
            </div>
        </div>
        <div class="columns">
            <div>
                <label for="description">Descripción</label>
                <textarea id="description" name="description" rows="10" cols="49"></textarea>
            </div>
        </div>
        <div class="columns">
            <div>
                <label for="category">Categoría</label>
                
                <select id="categoryId" name="categoryId">
                    <% foreach (var category in (CodeCamp2009Demos.Models.Category[])this.ViewData["categories"]) { %>
                    <option value="<%= category.Id %>"> <%= category.Description %></option>
                    <% } %>
                </select>        
            </div>
            <div id="last">
                <input type="submit" value="Guardar" />   
            </div>
        </div>
        
    </fieldset>
    </form>
</asp:Content>
