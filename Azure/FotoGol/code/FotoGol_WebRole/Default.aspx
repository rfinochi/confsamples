<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FotoGol_WebRole._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Windows Azure FotoGol</title>
    <link href="main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="general">
        <div class="title">
            <h1>
                <img src="logolagash.png"  alt="Lagash" /> FotoGol
            </h1>
        </div>
        <div class="inputSection">
            <dl>
                <dt>
                    <label for="NameLabel">Nombre:</label></dt>
                <dd>
                    <asp:TextBox 
                       ID="NameTextBox" 
                       runat="server" 
                       CssClass="field"/>
                    <asp:RequiredFieldValidator 
                      ID="NameRequiredValidator" 
                      runat="server" 
                      ControlToValidate="NameTextBox"
                      Text="*" />
                </dd>
                <dt>
                    <label for="MessageLabel">Mensaje:</label>
                </dt>
                <dd>
                    <asp:TextBox 
                       ID="MessageTextBox" 
                       runat="server" 
                       TextMode="MultiLine" 
                       CssClass="field" />
                    <asp:RequiredFieldValidator 
                       ID="MessageRequiredValidator" 
                       runat="server" 
                       ControlToValidate="MessageTextBox"
                       Text="*" />
                </dd>
                <dt>
                    <label for="FileUpload1">Foto:</label></dt>
                <dd>
                    <asp:FileUpload 
                        ID="FileUpload1" 
                        runat="server" 
                        size="16" />
                    <asp:RequiredFieldValidator 
                        ID="PhotoRequiredValidator" 
                        runat="server" 
                        ControlToValidate="FileUpload1"
                        Text="*" />
                    <asp:RegularExpressionValidator 
                        ID="PhotoRegularExpressionValidator"
                        runat="server"
                        ControlToValidate="FileUpload1" 
                        ErrorMessage="Only .jpg or .png files are allowed"
                        ValidationExpression="([a-zA-Z\\].*(.jpg|.JPG|.png|.PNG)$)" />
                </dd>
            </dl>
            <div class="inputSignSection">
                <asp:ImageButton 
                       ID="SignButton" 
                       runat="server" 
                       AlternateText="Sign GuestBook"
                       onclick="SignButton_Click" 
                       ImageUrl="~/boton_jabulani.jpg" 
                       ImageAlign="Bottom"  />
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:DataList 
                   ID="DataList1" 
                   runat="server" 
                   DataSourceID="ObjectDataSource1">
                    <ItemTemplate>
                    
                        
                       <div class="signature">
                        <div>
                            <div class="signatureImage">
                                <a href="<%# Eval("PhotoUrl") %>" target="_blank">
                                    <img src="<%# Eval("ThumbnailUrl") %>" alt="<%# Eval("GuestName") %>" />
                                </a>
                            </div>
                            <div class="signatureDescription">
                              
                                <div class="signatureName">
                                    <%# Eval("GuestName") %>
                                </div>
                                <div class="signatureSays">
                                    says
                                </div>
                                <div class="signatureDate">
                                    <%# ((DateTime)Eval("Timestamp")).ToShortDateString() %>
                                </div>
                                <div class="signatureMessage">
                                    "<%# Eval("Message") %>"
                                </div>
                            </div>
                            <br style="clear: both" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <asp:Timer 
                    ID="Timer1" 
                    runat="server"
                    Interval="15000"
                    OnTick="Timer1_Tick">
                </asp:Timer>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ObjectDataSource 
           ID="ObjectDataSource1"
           runat="server" 
           DataObjectTypeName="FotoGol_Data.FotoGolEntry"
           InsertMethod="AddGuestBookEntry"
           SelectMethod="Select" 
           TypeName="FotoGol_Data.FotoGolEntryDataSource">
        </asp:ObjectDataSource>
    </div>
    
    </form>
    
</body>
</html>
