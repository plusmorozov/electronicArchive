<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Futter.ascx.cs" Inherits="WebApplication2.Ascx.Futter" %>

<link href="/css/Style_Futter.css" rel="stylesheet" type="text/css" />

<div class="blockDebug" id="pnlDebug" runat="server" visible="false">
    <asp:Label ID="lblDebug1" runat="server" Text=""></asp:Label><br />
    <asp:Label ID="lblDebug2" runat="server" Text=""></asp:Label>
</div>

<div class="blockFutter">
    <asp:Label CssClass="elmFutter_Left"  ID="lblUser"    runat="server" Text="Гость"></asp:Label>
    <asp:Label CssClass="elmFutter_Right" ID="lblCreater" runat="server" Text="УЭСП 2018"></asp:Label>
    <asp:Label CssClass="elmFutter_Right" ID="lblApp"     runat="server" Text="Архив"></asp:Label>
    <div class="divClear"></div>
</div>