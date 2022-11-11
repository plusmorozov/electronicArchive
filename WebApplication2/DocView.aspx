<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocView.aspx.cs" Inherits="WebApplication2.WebForm4" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/MainMenu.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="ContentView" Src="~/Ascx/Content.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="AppFutter" Src="~/Ascx/Futter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Просмотр документа</title>
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
    <link href="~/css/font-awesome.css" rel="stylesheet" />
</head>
<body>
    <form id="frmDocView" runat="server">

        <div class="blockMain">
        <div class="blockTitle" id="pnlTitle" runat="server" >
            <div class="blockSubSectionLeft">
                 <MyControl:MyMenu ID="MainMenu" runat="server"></MyControl:MyMenu>
                <asp:Label CssClass="elmTitle" ID="lblTitle1"   runat="server" Text="Документ " ></asp:Label>
                <asp:Label CssClass="elmTitle" ID="lblDocNum"   runat="server" Text="[ Просмотр ]"   ></asp:Label>
            </div>

            <div class="divClear"></div>
        </div>

        <div class="blockError" id="pnlError" runat="server" visible="false">
            <asp:BulletedList ID="lstError" runat="server">
            </asp:BulletedList>
        </div >

        <div class="blockSection" ID="pnlDoc" runat="server">
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblDocName"         runat="server" Text="Название документа" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="edtDocName"         runat="server" TabIndex="201" Width="804px" ></asp:Label>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblShifr"           runat="server" Text="Шифр" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="edtShifr"           runat="server" TabIndex="202" Width="804px" ></asp:Label>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblYear"            runat="server" Text="Год" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlYear"            runat="server" TabIndex="203" Width="65px"></asp:Label>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblDocType"         runat="server" Text="Тип документа" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlDocType"         runat="server" TabIndex="204" Width="350px" ></asp:Label>
            <div class="divClear"></div>       
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblArchivedHuman"   runat="server" Text="Документ сдал" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="edtArchivedHuman"   runat="server" TabIndex="205" Width="804px" ></asp:Label>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblSection"         runat="server" Text="Архив подразделения" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlSection"         runat="server" TabIndex="206" Width="350px" ></asp:Label>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblObjectType"      runat="server" Text="Тип объекта" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlObjectType"      runat="server" TabIndex="207" Width="350px"></asp:Label>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblObject"          runat="server" Text="Объект" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlObject"          runat="server" TabIndex="208" Width="350px" ></asp:Label>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblCustomer"        runat="server" Text="Заказчик" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlCustomer"        runat="server" TabIndex="209" Width="350px" ></asp:Label>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblExecuter"        runat="server" Text="Исполнитель" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlExecuter"        runat="server" TabIndex="210" Width="350px" ></asp:Label>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblNameWork"        runat="server" Text="Наименование работ" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="edtNameWork"        runat="server" TabIndex="211" Width="804px" ></asp:Label>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblStorageBuild"    runat="server" Text="Место хранения. Здание" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlStorageBuild"    runat="server" TabIndex="212" Width="350px"></asp:Label>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblStoragePlace"    runat="server" Text="Место хранения. Кабинет" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlStoragePlace"    runat="server" TabIndex="213" Width="350px" ></asp:Label>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblStatus"          runat="server" Text="Статус оцифровки" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="ddlStatus"          runat="server" TabIndex="214" Width="350px" ></asp:Label>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"          ID="lblDepartment"      visible="false" runat="server" Text="Отделы" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="chbklstDepartment" visible="false" runat="server" TabIndex="215" Width="350px" ></asp:Label>
            <div class="divClear"></div>                                            
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200_descr"    ID="lblDescription"     runat="server" Text="Описание документа" ></asp:Label>
            <asp:Label        CssClass="elmLeft_MB8R10_B_AL_W200"           ID="edtDescription"     runat="server" TabIndex="216" Width="804px" ></asp:Label>
            <div class="divClear"></div>   
            
            <div class="divClear"></div>   
            
                                                     
            <div class="blockSectionGray" id="pnlAttachedFiles" runat="server">
                <MyControl:ContentView ID="Content" runat="server"></MyControl:ContentView>
            </div>

            <div class="divClear"></div>    
            <div class="blockSubSectionCenter">
                <asp:Button CssClass="elmBtnR_MR10" ID="btnCancel" runat="server" Text="Назад"    TabIndex="101" UseSubmitBehavior="False" onclick="btnCancel_Click" />
            </div>
            <div class="divClear"></div>        

       </div>

        <div class="blockDebug" id="pnlDebug" runat="server" visible="false">
            <asp:Label ID="lblDebug1" runat="server" Text="Вывод значения 1"></asp:Label>
            <br />
            <asp:Label ID="lblDebug2" runat="server" Text="Вывод значения 2"></asp:Label>
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






