<%@ Import Namespace="CodeCamp2009Demos.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Attender>>" %>

<table id="attendersListTable" class="tablesorter">
    <thead>
        <tr>
            <th>
                Nombre
            </th>
            <th>
                Edad
            </th>
            <th>
                Volverá
            </th>
            <th>
                Profesión
            </th>
            <th>
            </th>
            <th>
            </th>
        </tr>
    </thead>
    <tbody>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%= Html.Encode(item.Name)%>
            </td>
            <td>
                <%= Html.Encode(item.Age) %>
            </td>
            <td>
                <% if (item.ReturnNextYear)
                   {%>
                    S&iacute;
                <% }
                   else
                   {  %>
                  No   
                <%} %>   
            </td>
            <td>
                <%= Html.Encode(item.Profesion)%>
            </td>
            <td>
                <%= Html.ActionLink("Editar", "Edit", new { id = item.Id  }) %>
            </td>
            <td>
                <%= Html.ActionLink("Detalles", "Details", new { id = item.Id })%>
            </td>
        </tr>
        <% } %>
    </tbody>
</table>
