<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function startListening() {
            var user = $('#sourceUser').val();
            $.post('/comet?user=' + user + '', resultReady);
            //$.post('/comet.ashx', resultReady);
        }
        
        function resultReady(data) {
            $('.result').append('<p>' + data[0] + '</p>');
            //$('.result').append(data );
            startListening();
        }

        function Say(user, message) {
            $.post('/comet/Say?user=' + user + '&message=' + message + '');
        }
    </script>

    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    <br/>
    <p>user: <input type=text value=diego id=sourceUser /> <input type=button value=startListening onclick="startListening()" /></p>
    <p>
        <input type=text value=diego id=targetUser />
        <input type=text value=message id=message />
        <input type=button value=Say onclick="Say($('#targetUser').val(), $('#message').val() )" />
    </p>
    <p class="result">
    </p>
</asp:Content>
