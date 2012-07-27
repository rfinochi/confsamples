<%@ Import Namespace="CodeCamp2009Demos.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Attender>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Asistentes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        $(document).ready(bindTableSorter);

        function bindTableSorter() {
            $("#attendersListTable").tablesorter({
                headers: {
                    4: {
                        sorter: false
                    },
                    5: {
                        sorter: false
                    }
                }
            });
        }
                
    </script>

    <h2>
        Lista Asistentes
    </h2>
    <%using (Ajax.BeginForm("SearchAjax", new AjaxOptions { UpdateTargetId = "results", OnSuccess="bindTableSorter" }))
      { %>

<%--  <%using (Html.BeginForm("Search","Attenders"))
      { %>--%>
Filtro:
    <%=Html.TextBox("filter", null)%>
    &nbsp;
    <input type="submit" value="Buscar" style="width: 160px" />
   <%} %>
  
    <br />
    <div id="results">
        <%Html.RenderPartial("AttendersListPartial", Model); %>
    </div>
    <div>
        <%=Html.ActionLink("Nuevo", "Create") %>
    </div>
    
</asp:Content>
