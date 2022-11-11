<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocCreate.aspx.cs" Inherits="WebApplication2.WebForm2" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/AdminMenu.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="AppFutter" Src="~/Ascx/Futter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Создание документа</title>
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmDocCreate" runat="server">
    <div class="blockMain">
        <div class="blockTitle" id="pnlTitle" runat="server" >
            <div class="blockSubSectionLeft">
                 <MyControl:MyMenu ID="AdminMenu" runat="server"></MyControl:MyMenu>
                <asp:Label CssClass="elmTitle" ID="lblTitle1"   runat="server" Text="Документ " ></asp:Label>
                <asp:Label CssClass="elmTitle" ID="lblDocNum"   runat="server" Text="[ Создание ]"   ></asp:Label>
            </div>
            <div class="blockSubSectionRight">
            <asp:Button CssClass="elmBtnR_M10" ID="btnTest" runat="server" 
                    Text="Тестовые данные" onclick="btnTest_Click"  />  
            </div>  
        <div class="divClear"></div>
            <div class="divClear"></div>
        </div>

        <div class="blockError" id="pnlError" runat="server" visible="false">
            <asp:BulletedList ID="lstError" runat="server">
            </asp:BulletedList>
        </div >

        <div class="blockSection" ID="pnlDoc" runat="server">
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblDocName"    runat="server" Text="Название документа" ></asp:Label>
            <asp:TextBox      CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="edtDocName"    runat="server" TabIndex="201" Width="804px" ></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblShifr"      runat="server" Text="Шифр" ></asp:Label>
            <asp:TextBox      CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="edtShifr"      runat="server" TabIndex="202" Width="804px" ></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblYear"      runat="server" Text="Год" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlYear"      runat="server" TabIndex="203" Width="65px">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="2014">2014</asp:ListItem>     
                    <asp:ListItem Value="2015">2015</asp:ListItem>     
                    <asp:ListItem Value="2016">2016</asp:ListItem>     
                    <asp:ListItem Value="2017">2017</asp:ListItem>
                    <asp:ListItem Value="2018">2018</asp:ListItem>
                    <asp:ListItem Value="2019">2019</asp:ListItem>
                    <asp:ListItem Value="2020">2020</asp:ListItem>
                    <asp:ListItem Value="2021">2021</asp:ListItem>
                    <asp:ListItem Value="2022">2022</asp:ListItem>
                    <asp:ListItem Value="2023">2023</asp:ListItem>
            </asp:DropDownList>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblDocType"      runat="server" Text="Тип документа" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlDocType"      runat="server" TabIndex="204" Width="350px" >
            </asp:DropDownList>
            <div class="divClear"></div>       
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblArchivedHuman"    runat="server" Text="Документ сдал" ></asp:Label>
            <asp:TextBox      CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="edtArchivedHuman"    runat="server" TabIndex="205" Width="804px" ></asp:TextBox>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblSection"      runat="server" Text="Архив подразделения" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlSection"      runat="server" TabIndex="206" Width="350px" >
            </asp:DropDownList>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblObjectType"      runat="server" Text="Тип объекта" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlObjectType"      runat="server" TabIndex="207" Width="350px" AutoPostBack="True" onselectedindexchanged="ddlObjectType_SelectedIndexChanged" >
            </asp:DropDownList>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblObject"      runat="server" Text="Объект" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlObject"      runat="server" TabIndex="208" Width="350px" >
            </asp:DropDownList>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblCustomer"      runat="server" Text="Заказчик" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlCustomer"      runat="server" TabIndex="209" Width="350px" >
            </asp:DropDownList>
            <div class="divClear"></div>     
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblExecuter"      runat="server" Text="Исполнитель" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlExecuter"      runat="server" TabIndex="210" Width="350px" >
            </asp:DropDownList>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblNameWork"    runat="server" Text="Наименование работ" ></asp:Label>
            <asp:TextBox      CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="edtNameWork"    runat="server" TabIndex="211" Width="804px" ></asp:TextBox>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblStorageBuild"      runat="server" Text="Место хранения. Здание" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlStorageBuild"      runat="server" TabIndex="212" Width="350px" AutoPostBack="True" onselectedindexchanged="ddlStorageBuild_SelectedIndexChanged" >
            </asp:DropDownList>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblStoragePlace"      runat="server" Text="Место хранения. Кабинет" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlStoragePlace"      runat="server" TabIndex="213" Width="350px" >
            </asp:DropDownList>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblStatus"      runat="server" Text="Статус оцифровки" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlStatus"      runat="server" TabIndex="214" Width="350px" >
            </asp:DropDownList>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblDepartment"      runat="server" Text="Отделы"  visible="false"></asp:Label>
            <div class="divScroll" runat = "server" visible="false">
            <asp:CheckBoxList CssClass="elmLeft_DateTime_MB8_B2"      
                    ID="chbklstDepartment"      runat="server" TabIndex="215" Width="350px" 
                    onselectedindexchanged="chbklstDepartment_SelectedIndexChanged" >
            </asp:CheckBoxList>
            </div>
            <div class="divClear" runat="server"  visible="false"></div>                                            
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200_descr"    ID="lblDescription"      runat="server" Text="Описание документа" ></asp:Label>
            <asp:TextBox      CssClass="elmLeft_MB8R10_NB_AL_W200_descr"    ID="edtDescription"    runat="server" TabIndex="216" Width="804px" ></asp:TextBox>
            <div class="divClear"></div>                                            
            <div class="divClear"></div>    
            <div class="blockSubSectionCenter" >
                <asp:Button CssClass="elmBtnR_MR10" ID="btnCancel" runat="server" Text="Отмена"    TabIndex="101" UseSubmitBehavior="False" onclick="btnCancel_Click" />
                <asp:Button CssClass="elmBtnR_MR10" ID="btnNext" runat="server" Text="Далее" TabIndex="102" UseSubmitBehavior="False" onclick="btnNext_Click" />
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
    </form>
</body>
</html>
