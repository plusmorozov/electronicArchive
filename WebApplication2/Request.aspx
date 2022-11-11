<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Request.aspx.cs" Inherits="WebApplication2.Request" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/AdminMenu.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="AppFutter" Src="~/Ascx/Futter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Система электронного архива</title>
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
    <link href="~/css/font-awesome.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
    <div class="blockMain">
        <div id="Div1" class="blockTitle" runat="server">
            <div class="blockSubSectionLeft">
                <MyControl:MyMenu ID="AdminMenu" runat="server"></MyControl:MyMenu>
                <asp:Label CssClass="elmTitle" ID="lblTitle1"   runat="server" Text="Запросы на доступ" ></asp:Label>
            </div>
        </div>
        <div class="blockSectionGray" id="Div2" runat="server">
            <asp:GridView ID="dbgRequest" runat="server" CellPadding="4" ForeColor="#333333" 
             AutoGenerateColumns="False" 
             DataSourceID="qryControl" 
             onrowcommand="dbgRequest_RowCommand"
             >
                <RowStyle BackColor="#F0EDE6" />
                <AlternatingRowStyle BackColor="#E6E3D2" />
                <HeaderStyle BackColor="Silver" Font-Bold="False" ForeColor="#666666" Font-Italic="False" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <EmptyDataRowStyle BackColor="LightBlue"/>
                <Columns>
                    <asp:TemplateField HeaderText="Дата запроса" ItemStyle-Width="150px">
                        <ItemTemplate>
                                <asp:Label ID="lblTiemRequest" runat="server"><%# Eval("TimeRequest")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Документ" ItemStyle-Width="450px">
                        <ItemTemplate>
                                <asp:Label ID="lblDoc" runat="server"><%# Eval("strDocName")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Контент" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblContent" runat="server"><%# Eval("strContentName")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Гриф" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblGrif" runat="server"><%# Eval("strGrifName")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Архив" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblArchive" runat="server"><%# Eval("strArchiveName")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Запрашивает" ItemStyle-Width="580px">
                        <ItemTemplate>
                            <asp:Label ID="lblUserInfo" runat="server"><%# Eval("strUserInfo")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Дней доступа" ItemStyle-Width="100px">
                        <ItemTemplate>
                        <asp:DropDownList CssClass="elmBtnL_M10" ID="ddlDay" runat="server" AutoPostBack="True" >
                            <asp:ListItem>1</asp:ListItem>  <asp:ListItem>2</asp:ListItem>  <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>  <asp:ListItem>5</asp:ListItem>  <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>  <asp:ListItem>8</asp:ListItem>  <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                        </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="100px">
                        <ItemTemplate>
                                <asp:LinkButton runat="server" ID="BtnAllowAccess" CommandName="AllowAccess" CommandArgument='<%# Eval("ID") %>' ><i class="fa fa-check-circle-o fa-3x"></i></asp:LinkButton>    
                                <asp:LinkButton runat="server" ID="BtnDenyAccess" CommandName="DenyAccess" CommandArgument='<%# Eval("ID") %>' ><i class="faRed fa-times-circle-o fa-3x"></i></asp:LinkButton>                                    
                        </ItemTemplate>  
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <center>
                    <asp:Label ID="Label5"  runat="server" Text="Запросов на доступ к информации нет" Width="1030px"></asp:Label>
                    </center>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
     
        <div ID="pnlAdminInfo" runat="server" 
            style="padding: 10px; background-color: #00FFFF; font-weight: normal" >
            <asp:Label ID="lblInfo" runat="server" Font-Bold="True" Font-Size="Small" 
                ForeColor="#666666" ></asp:Label>
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
