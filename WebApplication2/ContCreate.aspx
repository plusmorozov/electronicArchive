<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContCreate.aspx.cs" Inherits="WebApplication2.WebForm3" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/AdminMenu.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Создание контента</title>
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmDocCreate" runat="server">
        <div class="blockMain">
        <div class="blockTitle" id="pnlTitle" runat="server" >
            <div class="blockSubSectionLeft">
                <MyControl:MyMenu ID="AdminMenu" runat="server"></MyControl:MyMenu>
                <asp:Label CssClass="elmTitle" ID="lblTitle1"   runat="server" Text="Контент " ></asp:Label>
                <asp:Label CssClass="elmTitle" ID="lblDocNum"   runat="server" Text="[ Создание ]"   ></asp:Label>
            </div>
                        <div class="blockSubSectionRight">
            <asp:Button CssClass="elmBtnR_M10" ID="btnTest" runat="server" 
                    Text="Тестовые данные" onclick="btnTest_Click"  />  
            </div>  
            <div class="divClear"></div>
        </div>

        <div class="blockError" id="pnlError" runat="server" visible="false">
            <asp:BulletedList ID="lstError" runat="server">
            </asp:BulletedList>
        </div >
        
        <div class="blockSection" ID="pnlDoc" runat="server">
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblContName"    runat="server" Text="Название контента" ></asp:Label>
            <asp:TextBox      CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="edtContName"    runat="server" TabIndex="201" Width="804px" ></asp:TextBox>
            <div class="divClear"></div>    
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblContType"    runat="server" Text="Тип контента" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlContType"      runat="server" TabIndex="214" Width="350px" ></asp:DropDownList>
            <div class="divClear"></div>    
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblGrif"    runat="server" Text="Гриф" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlGrif"      runat="server" TabIndex="214" Width="350px" ></asp:DropDownList>
            <div class="divClear"></div>    
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblStorageBuild"      runat="server" Text="Место хранения. Здание" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlStorageBuild"      runat="server" TabIndex="212" Width="350px" AutoPostBack="True" onselectedindexchanged="ddlStorageBuild_SelectedIndexChanged" >
            </asp:DropDownList>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblStoragePlace"      runat="server" Text="Место хранения. Кабинет" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlStoragePlace"      runat="server" TabIndex="213" Width="350px" >
            </asp:DropDownList>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblContCarrier"      runat="server" Text="Физический носитель" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"       ID="ddlContCarrier"      runat="server" TabIndex="213" Width="350px" >
            </asp:DropDownList>
            <div class="divClear"></div>
            <div class="divClear"></div>
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblDescription"    runat="server" Text="Описание файла" ></asp:Label>
            <asp:TextBox      CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="edtDescription"    runat="server" TabIndex="201" Width="804px" ></asp:TextBox>
            <div class="divClear"></div>    
            <div class="divClear"></div>    
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200"    ID="lblAttach"      runat="server" Text="Прикрепить файл" ></asp:Label>
            <asp:FileUpload ID="fuFileUpload" runat="server"/>
            <div class="divClear"></div>    
            <div class="divClear"></div>    
            
            <div class="blockSubSectionCenter" >
                <asp:Button CssClass="elmBtnR_MR10" ID="btnCancel" runat="server" Text="Отмена"    TabIndex="101" UseSubmitBehavior="False" onclick="btnCancel_Click" />
                <asp:Button CssClass="elmBtnR_MR10" ID="btnSave" runat="server" Text="Сохранить" TabIndex="102" UseSubmitBehavior="False" onclick="btnSave_Click" />
            </div>
            <div class="divClear"></div>        
        </div>
        
        <div class="blockDebug" id="pnlDebug" runat="server" visible="false">
            <asp:Label ID="lblDebug1" runat="server" Text="Вывод значения 1"></asp:Label>
            <br />
            <asp:Label ID="lblDebug2" runat="server" Text="Вывод значения 2"></asp:Label>
        </div>

        <div class="blockFooter">
            <asp:Label CssClass="elmLeft_MR20"  ID="lblUser"    runat="server" Text="Гость"></asp:Label>
            <asp:Label CssClass="elmRight_ML20" ID="lblCreater" runat="server" Text="УЭСП 2017"></asp:Label>
            <asp:Label CssClass="elmRight_ML20" ID="lblApp"     runat="server" Text="Архив"></asp:Label>
            <div class="divClear"></div>
        </div>
    </div>

    <div class="blockBreak" id="pnlBreak" runat="server" visible="false" ></div>
    <div class="blockAddContent" id="pnlAddContent" runat="server" visible="false">
        <asp:Label CssClass="elmCenter_MB8R10_NB_AL_W200" ID="lblAddContent" runat="server" Text="<b>Добавить еще 1 контент?</b>"></asp:Label>
        <asp:Button CssClass="elmBtnC" ID="btnAddContent" runat="server" 
                Text="Добавить" TabIndex="100" onclick="btnAddContent_Click"/>
        <asp:Button CssClass="elmBtnC" ID="btnClose" runat="server" 
                Text="Завершить" TabIndex="101" onclick="btnClose_Click"/>
    </div>
    </form>
</body>
</html>
