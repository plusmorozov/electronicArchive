<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminMenu.ascx.cs" Inherits="WebApplication2.WebUserControl2" %>
<asp:Menu ID="AdminMenu" runat="server" BackColor="#B5C7DE" 
        DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em" 
        ForeColor="#284E98" Orientation="Horizontal" StaticSubMenuIndent="10px" 
        onmenuitemclick="AdminMenu_MenuItemClick">
        <DynamicHoverStyle BackColor="#284E98" ForeColor="White" />
        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
        <DynamicMenuStyle BackColor="#B5C7DE" />
        <DynamicSelectedStyle BackColor="#507CD1" />
        <Items>
            <asp:MenuItem Text="Документы" Value="Docs"></asp:MenuItem>
            <asp:MenuItem Text="Администрирование" Value="Admin"></asp:MenuItem>
            <asp:MenuItem Text="Запросы на доступ" Value="Request"></asp:MenuItem>
            <asp:MenuItem Text="Журнал выдачи" Value="Journal"></asp:MenuItem>
            <asp:MenuItem Text="Пользователи" Value="Users"></asp:MenuItem>
            <asp:MenuItem Text="Справочники" Value="Spr">
                <asp:MenuItem Text="Компании" Value="Company"></asp:MenuItem>
                <asp:MenuItem Text="Объекты" Value="Object"></asp:MenuItem>
            </asp:MenuItem>
        </Items>
        <StaticHoverStyle BackColor="#284E98" ForeColor="White" />
        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
        <StaticSelectedStyle BackColor="#507CD1" />
    </asp:Menu>