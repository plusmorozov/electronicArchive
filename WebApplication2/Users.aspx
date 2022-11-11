<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="WebApplication2.Users" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/AdminMenu.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="AppFutter" Src="~/Ascx/Futter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Управление пользователями</title>
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmDocCreate" runat="server">
    <div class="blockMain">
        <div class="blockTitle" id="pnlTitle" runat="server" >
            <div class="blockSubSectionLeft">
                 <MyControl:MyMenu ID="AdminMenu" runat="server"></MyControl:MyMenu>
                <asp:Label CssClass="elmTitle" ID="lblTitle1"   runat="server" Text="Пользователи " ></asp:Label>
                <asp:Label CssClass="elmTitle" ID="lblDocNum"   runat="server" Text="[ Создание / редактирование ]"   ></asp:Label>
            </div>
        <div class="divClear"></div>
            <div class="divClear"></div>
        </div>

        <div class="blockError" id="pnlError" runat="server" visible="false">
            <asp:BulletedList ID="lstError" runat="server">
            </asp:BulletedList>
        </div >

        <div class="blockSection" ID="pnlDoc" runat="server">
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblUserLogin"    runat="server" Text="Пользователь" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlUserLogin"      runat="server" TabIndex="204" Width="480px" AutoPostBack="true" onselectedindexchanged="ddlUserLogin_SelectedIndexChanged" ></asp:DropDownList>
            <asp:Button       CssClass="elmBtnL_ML10"              ID="btnUserCreate" runat="server" TabIndex="202" Text="Создать"   UseSubmitBehavior="False" onclick="btnUserCreate_Click" Visible="False" />
            <asp:Button       CssClass="elmBtnL_ML10"              ID="btnUserUpdate" runat="server" TabIndex="203" Text="Сохранить" UseSubmitBehavior="False" onclick="btnUserUpdate_Click" Visible="False" />
            <asp:Button       CssClass="elmBtnL_ML10"              ID="btnUserCancel" runat="server" TabIndex="204" Text="Отменить"  UseSubmitBehavior="False" onclick="btnUserCancel_Click" Visible="False" />
            <div class="divClear"></div>

            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblLogin"      runat="server" Text="Логин" ></asp:Label>
            <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtLogin"      runat="server" TabIndex="220" Width="800px"></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblFam"        runat="server" Text="Фамилия" ></asp:Label>
            <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtFam"        runat="server" TabIndex="221" Width="800px"></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblName"        runat="server" Text="Имя" ></asp:Label>
            <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtName"        runat="server" TabIndex="221" Width="800px"></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblOtch"        runat="server" Text="Отчество" ></asp:Label>
            <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtOtch"        runat="server" TabIndex="221" Width="800px"></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblPodr"       runat="server" Text="Подразделение" ></asp:Label>
            <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtPodr"       runat="server" TabIndex="222" Width="800px"></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblDepartment"       runat="server" Text="Отдел" ></asp:Label>
            <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtDepartment"       runat="server" TabIndex="222" Width="800px"></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblPost"       runat="server" Text="Должность" ></asp:Label>
            <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtPost"       runat="server" TabIndex="222" Width="800px"></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblPhone"   runat="server" Text="Телефон" ></asp:Label>
            <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtPhone"   runat="server" TabIndex="223" Width="800px"></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblEmail"   runat="server" Text="Почтовый ящик" ></asp:Label>
            <asp:TextBox CssClass="elmLeft_MB8R10_NB_AL_W200" ID="edtEmail"   runat="server" TabIndex="223" Width="800px"></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblRole"       runat="server" Text="Роль" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_MB8R10_NB_AL_W200" ID="ddlRole"  runat="server" TabIndex="225" Width="800px">
                <asp:ListItem Value="0" Text=" "></asp:ListItem>
                <asp:ListItem Value="2">Администратор</asp:ListItem>
                <asp:ListItem Value="3">Архивариус</asp:ListItem>
                <asp:ListItem Value="4">Пользователь</asp:ListItem>
            </asp:DropDownList>
            <div class="divClear"></div>

            <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblUserEnable"   runat="server" Text="Включить пользователя" ></asp:Label>
            <asp:CheckBox                                     ID="cbUserEnable" runat="server" TabIndex="226" Text="Вкл/Откл"/>
            <div class="divClear"></div>

            <div class="blockSectionGray" id="pnlUserAccessData" runat="server">
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblAdmin"   runat="server" Text="Доступ к странице администрирования" ></asp:Label>
                <asp:CheckBox                                     ID="cbAdmin" runat="server" TabIndex="226"/>
                <div class="divClear"></div>
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblRequest"   runat="server" Text="Просмотр запросов на доступ" ></asp:Label>
                <asp:CheckBox                                     ID="cbRequest" runat="server" TabIndex="226"/>
                <div class="divClear"></div>
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblViewDocIssue"   runat="server" Text="Доступ к журналу выдачи" ></asp:Label>
                <asp:CheckBox                                     ID="cbViewDocIssue" runat="server" TabIndex="226"/>
                <div class="divClear"></div>
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblUserAdmin"   runat="server" Text="Администрирование пользователей" ></asp:Label>
                <asp:CheckBox                                     ID="cbUserAdmin" runat="server" TabIndex="226"/>
                <div class="divClear"></div>
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblSpr"   runat="server" Text="Доступ к справочникам" ></asp:Label>
                <asp:CheckBox                                     ID="cbSpr" runat="server" TabIndex="226"/>
                <div class="divClear"></div>
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblDebug"   runat="server" Text="Вывод отладочной информации" ></asp:Label>
                <asp:CheckBox                                     ID="cbDebug" runat="server" TabIndex="226"/>
                <div class="divClear"></div>
                <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblGrif"      runat="server" Text="Доступные грифы" ></asp:Label>
                <asp:CheckBoxList CssClass="elmLeft_DateTime_MB8_B2" ID="cblGrif" runat="server" TabIndex="215" Width="350px"></asp:CheckBoxList>
                <%--<div class="divClear"></div>
                <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblDocType"      runat="server" Text="Доступные типы документов" ></asp:Label>
                <asp:CheckBoxList CssClass="elmLeft_DateTime_MB8_B2" ID="cblDocType" runat="server" TabIndex="215" Width="350px"></asp:CheckBoxList>--%>
                <div class="divClear"></div>
                <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblArchiveRead"      runat="server" Text="Доступные архивы для скачивания" ></asp:Label>
                <asp:CheckBoxList CssClass="elmLeft_DateTime_MB8_B2" ID="cblArchiveRead" runat="server" TabIndex="215" Width="350px"></asp:CheckBoxList>
                <div class="divClear"></div>
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblDocCreate"   runat="server" Text="Создание документов" ></asp:Label>
                <asp:CheckBox                                     ID="cbDocCreate" runat="server" TabIndex="226"/>
                <div class="divClear"></div>
                <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblArchiveDocCreate"      runat="server" Text="Доступные архивы для создания документов" ></asp:Label>
                <asp:CheckBoxList CssClass="elmLeft_DateTime_MB8_B2" ID="cblArchiveDocCreate" runat="server" TabIndex="215" Width="350px"></asp:CheckBoxList>
                <div class="divClear"></div>
                <asp:Label   CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblContCreate"   runat="server" Text="Создание контентов" ></asp:Label>
                <asp:CheckBox                                     ID="cbContCreate" runat="server" TabIndex="226"/>
                <div class="divClear"></div>
                <%--<asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblContCreate1"      runat="server" Text="Архивы для создания контентов" ></asp:Label>
                <asp:CheckBoxList CssClass="elmLeft_DateTime_MB8_B2" ID="cblContCreate" runat="server" TabIndex="215" Width="350px"></asp:CheckBoxList>
                <div class="divClear"></div>
                --%>
                
                
                
                <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblArchiveDocIssue"      runat="server" Text="Архивы для выдачи документов" ></asp:Label>
                <asp:CheckBoxList CssClass="elmLeft_DateTime_MB8_B2" ID="cblArchiveDocIssue" runat="server" TabIndex="215" Width="350px"></asp:CheckBoxList>
                <div class="divClear"></div>
                
            </div>
       </div>

        <div class="blockDebug" id="pnlDebug" runat="server" visible="false">
            <asp:Label ID="lblDebug1" runat="server" Text="Вывод значения 1"></asp:Label>
            <br />
            <asp:Label ID="lblDebug2" runat="server" Text="Вывод значения 2"></asp:Label>
        </div>
        <mycontrol:AppFutter ID="Futter" runat="server" />
    </div>
    </form>
</body>
</html>
