<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestUser.aspx.cs" Inherits="WebApplication2.Admin" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/AdminMenu.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="AppFutter" Src="~/Ascx/Futter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Тестирование пользователей</title>
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
    <link href="~/css/font-awesome.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
        <div class="blockMain">
            <div id="Div1" class="blockTitle" runat="server">
                <div class="blockSubSectionLeft">
                    <MyControl:MyMenu ID="AdminMenu" runat="server"></MyControl:MyMenu>
                </div>
            </div>

            <div class="blockSection" id="pnlHeader_TestUsers" runat="server">
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblTestUser"    runat="server" Text="Тестирование пользователей"></asp:Label>
                <div class="divClear"></div>
            </div >
            <div class="blockSection" id="pnlBody_TestUsers" runat="server" visible="true">
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblTestUID"    runat="server" Text="UID пользователя"></asp:Label>
                <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtTestUID"    runat="server" TabIndex="1000" Width="50px"></asp:TextBox>
                <asp:Button  CssClass="elmBtnL_ML10"              ID="btnSetTestUID" runat="server" Text="Применить" onclick="btnSetTestUID_Click" />
                <div class="divClear10"></div>
            </div>
            <mycontrol:AppFutter ID="Futter" runat="server" />
        </div>
    </form>
</body>
</html>
