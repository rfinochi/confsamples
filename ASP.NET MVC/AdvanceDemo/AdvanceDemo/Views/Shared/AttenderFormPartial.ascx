<%@ Import Namespace="CodeCamp2009Demos.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Attender>" %>

<script type='text/javascript'>
       
        $(document).ready(function() {
            startAutoComplete();
            $('#showTalks').click(function() {$('#talks').show('slow'); $(this).hide(); $('#hideTalks').show();});
            $('#hideTalks').click(function() {$('#talks').hide('slow'); $(this).hide(); $('#showTalks').show();});
        });  
              
        function startAutoComplete(){
            $('#Profesion').autocomplete('<%=Url.Action("AutoCompleteProfesion", "Attenders") %>', {      
                    dataType: 'json',      
                    parse: function(data) {          
                        var rows = new Array();         
                        for(var i=0; i<data.length; i++){             
                            rows[i] = { data:data[i], result:data[i] };       
                        }          
                        return rows;      
                    },     
                    formatItem: function(row, i, max) {
			            return i + "/" + max + ": " + row;
		            },
                    width: 300,     
                    mustMatch: false,  
                    });
       }
    
</script>

<fieldset>
    <legend>Datos Asistente</legend>
    <p>
        <label for="Edad">
            Nombre:</label>
        <%= Html.TextBox("Name")%>
        <%= Html.ValidationMessage("Name", "*")%>
    </p>
    <p>
        <label for="Nombre">
            Edad:</label>
        <%= Html.TextBox("Age")%>
        <%= Html.ValidationMessage("Age", "*")%>
    </p>
    <p>
        <label for="Profesion">
            Profesión:</label>
        <%= Html.TextBox("Profesion") %>
        <%= Html.ValidationMessage("Profesion", "*")%>
    </p>
    <p>
        <label for="VolveraElAnioQueViene">
            <%= Html.CheckBox("ReturnNextYear")%>
            Volveré el año que viene.</label>
    </p>
    <p>
        <label for="Comentarios">
            Comentarios:</label>
        <%= Html.TextArea("Comments", new { cols="50", rows="5" }) %>
    </p>
    <p>
        <a id="showTalks">Ver Charlas</a> <a id="hideTalks" style="display: none;">Ocultar Charlas</a>
    </p>
    <div id="talks" style="display: none;">
        <p>
            <%= Html.ListBox("TalksIds", ViewData["TalksList"] as MultiSelectList, new { style = "width:250px;" })%>
        </p>
    </div>
    
    <p>
        <input type="submit" value="Guardar" />
    </p>
</fieldset>