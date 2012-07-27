<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CodeCamp2009Demos.Models.Attender>>" %>
<link href="../../Content/Site.css" rel="stylesheet" type="text/css" />

    <table>
        <tr>
            <th></th>
            <th>
                Nombre
            </th>
            <th>
                Edad
            </th>
             <th>
                Volvera
            </th>
             <th>
                Comentarios
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Editar", "Edit", new { id=item.Id  }) %> |
                <%= Html.ActionLink("Detalles", "Details", new { id = item.Id })%>
            </td>
            <td>
                <%= Html.Encode(item.Name)%>
            </td>
            <td>
                <%= Html.Encode(item.Age) %>
            </td>
            <td>
                <%= Html.Encode(item.Return) %>
            </td>
            <td>
                <%= Html.Encode(item.Comments) %>
            </td>
        </tr>
    
    <% } %>

    </table>
