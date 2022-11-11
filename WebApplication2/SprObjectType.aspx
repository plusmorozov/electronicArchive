<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SprObjectType.aspx.cs" Inherits="WebApplication2.SprObjectType" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/AdminMenu.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="AppFutter" Src="~/Ascx/Futter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Справочник типов объектов</title>
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
    <link href="~/css/font-awesome.css" rel="stylesheet" />
</head>
<body>
    <form id="frmDocView" runat="server">
        <div class="blockMain">
            <div class="blockTitle" id="pnlTitle" runat="server" >
                <div class="blockSubSectionLeft">
                    <MyControl:MyMenu ID="AdminMenu" runat="server"></MyControl:MyMenu>
                </div>
                <div class="divClear"></div>
            </div>

            <div class="blockError" id="pnlError" runat="server" visible="false">
                <asp:BulletedList ID="lstError" runat="server">
                </asp:BulletedList>
            </div >

            <div class="blockSection" ID="pnlObjectType" runat="server">
            <asp:LinkButton   CssClass="elmBtnR_MR10_2"              ID="btnTable_Create" runat="server" TabIndex="202" onclick="btnTable_Create_Click" ForeColor="#4B6C9E" ><i class="fa fa-plus-square-o fa-3x"></i></asp:LinkButton>
            <div class="divClear"></div>
            
                <div id="pnlTableInsert" runat="server" visible="False">
                </div>
                
                <div class="divClear"></div>   
            </div>

            <mycontrol:AppFutter ID="Futter" runat="server" />
        </div>

        <asp:SqlDataSource ID="QueryCompany" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ArchiveConnString %>" 
            ProviderName="<%$ ConnectionStrings:ArchiveConnString.ProviderName %>" 
            SelectCommand="SELECT C.ID,  C.strName, C.strNameShort, C.strNameFull, C.strAddress FROM a_company C"
            UpdateCommand="SELECT 1" > 
        </asp:SqlDataSource>
    </form>
</body>
</html>