<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AscxMain.aspx.cs" Inherits="WebApplication2.WebForm1" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/MainMenu.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="AppFutter" Src="~/Ascx/Futter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"> 
    <title>Система электронного архива</title>
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
    <link href="~/css/font-awesome.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="blockMain">
        <div class="blockTitle" runat="server">
        <div class="blockSubSectionLeft">
            <MyControl:MyMenu ID="MainMenu" runat="server"></MyControl:MyMenu>
        </div>
            <div class="blockSubSectionRight">
                <asp:Button CssClass="elmBtnR_M10" ID="btnTestUser" runat="server" Text="Тест" 
                        onclick="btnTestUser_Click" />    
                <asp:Button CssClass="elmBtnR_M10" ID="btnStopTest" runat="server" Text="Стоп тест" 
                        onclick="btnStopTest_Click" />    
                <div class="divClear"></div>
            </div>
        </div>
        <div id="Filter" class="blockSection" runat="server">
            <asp:TextBox CssClass="elmBtnL_M10" ID="edtSearch" runat="server"></asp:TextBox>
            <asp:Button CssClass="elmBtnL_M10" ID="btnSearch" runat="server" onclick="Filter_Changed" Text="Поиск" />
            <asp:DropDownList CssClass="elmBtnL_M10" ID="ddlDocType" runat="server" AutoPostBack="True" onselectedindexchanged="Filter_Changed"></asp:DropDownList>
            <asp:DropDownList CssClass="elmBtnL_M10" ID="ddlYear" runat="server" AutoPostBack="True" onselectedindexchanged="Filter_Changed">
            <asp:ListItem Value="0">Все</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>  <asp:ListItem>2017</asp:ListItem>  <asp:ListItem>2018</asp:ListItem>
                    <asp:ListItem>2019</asp:ListItem>  <asp:ListItem>2020</asp:ListItem>  <asp:ListItem>2021</asp:ListItem>
                    <asp:ListItem>2022</asp:ListItem>  <asp:ListItem>2023</asp:ListItem>  <asp:ListItem>2024</asp:ListItem>
                    <asp:ListItem>2025</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList CssClass="elmBtnL_M10" ID="ddlSection" runat="server" AutoPostBack="True" onselectedindexchanged="Filter_Changed"></asp:DropDownList>
            <asp:DropDownList CssClass="elmBtnL_M10" ID="ddlStatus" runat="server" AutoPostBack="True" onselectedindexchanged="Filter_Changed"></asp:DropDownList>
            &nbsp;<div class="divClear"></div>
        </div>

        <div class="blockSectionGray" id="Div1" runat="server">
            <asp:GridView ID="dbgMain" runat="server" CellPadding="4" ForeColor="#333333" 
             AutoGenerateColumns="False" 
             DataSourceID="qryControl" 
             onselectedindexchanged="dbgMain_SelectedIndexChanged"
             onrowcommand="dbgMain_RowCommand"
             >
                <RowStyle BackColor="#F0EDE6" />
                <AlternatingRowStyle BackColor="#E6E3D2" />
                <HeaderStyle BackColor="Silver" Font-Bold="False" ForeColor="#666666" Font-Italic="False" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <EmptyDataRowStyle BackColor="LightBlue"/>
                <Columns>
                    <asp:TemplateField HeaderText="Документ" ItemStyle-Width="450px">
                        <ItemTemplate>
                                <asp:LinkButton ID="linkDoc" runat="server"
                                                oncommand="linkDocClick"
                                                CommandArgument='<%# Eval("ID") %>'
                                                Text='<%# Eval("strNameDoc")%>' >
                                </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Количество контентов" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblCountContent" runat="server"><%# Eval("intCountContent")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Год" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblYear" runat="server"><%# Eval("intYear")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Тип документа" ItemStyle-Width="580px">
                        <ItemTemplate>
                            <asp:Label ID="lblTypeDoc" runat="server"><%# Eval("strNameTypeDoc")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Подразделение" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblSection" runat="server"><%# Eval("strNameSec")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Статус" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatusDoc" runat="server"><%# Eval("strNameStatusDoc")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <center>
                    <asp:Label ID="Label5"  runat="server" Text="Документов соответствующих фильтрам нет" Width="1030px"></asp:Label>
                    </center>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
     
        <%--<div ID="pnlAdminInfo" runat="server" 
            style="padding: 10px; background-color: #00FFFF; font-weight: normal" >
            <asp:Label ID="lblInfo" runat="server" Font-Bold="True" Font-Size="Small" 
                ForeColor="#666666" ></asp:Label>
        </div>--%>

     <mycontrol:AppFutter ID="Futter" runat="server" />
     </div>

     <asp:SqlDataSource ID="qryControl" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ArchiveConnString %>" 
        ProviderName="<%$ ConnectionStrings:ArchiveConnString.ProviderName %>">
                
     </asp:SqlDataSource>
     </form>
</body>
</html>
