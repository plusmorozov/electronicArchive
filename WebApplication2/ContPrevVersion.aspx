<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContPrevVersion.aspx.cs" Inherits="WebApplication2.ContPrevVersion" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/MainMenu.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="ContentView" Src="~/Ascx/Content.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="AppFutter" Src="~/Ascx/Futter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Просмотр предыдущих версий контента</title>
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
    <link href="~/css/font-awesome.css" rel="stylesheet" />
</head>

<body>
<form id="frmDocView" runat="server">

        <div class="blockMain">
        <div class="blockTitle" id="pnlTitle" runat="server" >
            <div class="blockSubSectionLeft">
                 <MyControl:MyMenu ID="MainMenu" runat="server"></MyControl:MyMenu>
                <asp:Label CssClass="elmTitle" ID="lblTitle1"   runat="server" Text="Контент " ></asp:Label>
                <asp:Label CssClass="elmTitle" ID="lblDocNum"   runat="server" Text="[ Доступные версии ]"   ></asp:Label>
            </div>

            <div class="divClear"></div>
        </div>

        <div class="blockError" id="pnlError" runat="server" visible="false">
            <asp:BulletedList ID="lstError" runat="server">
            </asp:BulletedList>
        </div >

        <div class="blockSection" ID="pnlDoc" runat="server">
                                                                 
            <div class="blockSectionGray" id="pnlAttachedFiles" runat="server">
                <MyControl:ContentView ID="Content" runat="server"></MyControl:ContentView>
            </div>

            <div class="divClear"></div>    
            <div class="blockSubSectionCenter">
                <asp:Button CssClass="elmBtnR_MR10" ID="btnCancel" runat="server" Text="Назад"    TabIndex="101" UseSubmitBehavior="False" onclick="btnCancel_Click" />
            </div>
            <div class="divClear"></div>        

       </div>

        <mycontrol:AppFutter ID="Futter" runat="server" />
        </div>

<asp:SqlDataSource ID="qryControl" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ArchiveConnString %>" 
    ProviderName="<%$ ConnectionStrings:ArchiveConnString.ProviderName %>">
</asp:SqlDataSource>

    </form>
</body>



</html>
