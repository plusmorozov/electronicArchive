<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Journal.aspx.cs" Inherits="WebApplication2.Jouranal" %>
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
                <asp:Label CssClass="elmTitle" ID="lblTitle1"   runat="server" Text="Журнал выдачи документов" ></asp:Label>
            </div>
        </div>

       <div class="blockSectionGray" id="pnlDate" runat="server">
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblDate"    runat="server" Text="Показать журнал в период:" Width="800px" ></asp:Label>
            <div class="divClear"></div>                                            
                <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblStart"    runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;С"  width="55px"></asp:Label>
                <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"    ID="ddlStart_DD" runat="server" TabIndex="301" Width="45px" ></asp:DropDownList>
                <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B" ID="ddlStart_MM" runat="server" TabIndex="302" Width="95px" AutoPostBack="True" onselectedindexchanged="DataBind_Start_Days">
                    <asp:ListItem Value= "1">январь</asp:ListItem>     <asp:ListItem Value= "2">февраль</asp:ListItem>    <asp:ListItem Value= "3">март</asp:ListItem>
                    <asp:ListItem Value= "4">апрель</asp:ListItem>     <asp:ListItem Value= "5">май</asp:ListItem>        <asp:ListItem Value= "6">июнь</asp:ListItem>
                    <asp:ListItem Value= "7">июль</asp:ListItem>       <asp:ListItem Value= "8">август</asp:ListItem>    <asp:ListItem Value= "9">сентябрь</asp:ListItem>
                    <asp:ListItem Value="10">октябрь</asp:ListItem>    <asp:ListItem Value="11">ноябрь</asp:ListItem>     <asp:ListItem Value="12">декабрь</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B" ID="ddlStart_YY" runat="server" TabIndex="303" Width="65px" AutoPostBack="True" onselectedindexchanged="DataBind_Start_Days">
                    <asp:ListItem Value="2019">2019</asp:ListItem>     <asp:ListItem Value="2020">2020</asp:ListItem>     <asp:ListItem Value="2021">2021</asp:ListItem>
                    <asp:ListItem Value="2022">2022</asp:ListItem>     <asp:ListItem Value="2023">2023</asp:ListItem>     <asp:ListItem Value="2024">2024</asp:ListItem>
                    <asp:ListItem Value="2025">2025</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label1"        CssClass="elmLeft_MB8R10_NB_AL_W200" runat="server" Text=" "  width="15px"></asp:Label>

               
                <div class="divClear"></div>                                            

                <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblFinish"    runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;По"  width="55px"></asp:Label>
                <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"    ID="ddlFinish_DD" runat="server" TabIndex="306" Width="45px" ></asp:DropDownList>
                <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"    ID="ddlFinish_MM" 
                runat="server" TabIndex="307" Width="95px" AutoPostBack="True" 
                onselectedindexchanged="DataBind_Finish_Days">
                    <asp:ListItem Value= "1">январь</asp:ListItem>     <asp:ListItem Value= "2">февраль</asp:ListItem>    <asp:ListItem Value= "3">март</asp:ListItem>
                    <asp:ListItem Value= "4">апрель</asp:ListItem>     <asp:ListItem Value= "5">май</asp:ListItem>        <asp:ListItem Value= "6">июнь</asp:ListItem>
                    <asp:ListItem Value= "7">июль</asp:ListItem>       <asp:ListItem Value= "8">август</asp:ListItem>    <asp:ListItem Value= "9">сентябрь</asp:ListItem>
                    <asp:ListItem Value="10">октябрь</asp:ListItem>    <asp:ListItem Value="11">ноябрь</asp:ListItem>     <asp:ListItem Value="12">декабрь</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList CssClass="elmLeft_DateTime_MB8_B"    ID="ddlFinish_YY" 
                runat="server" TabIndex="308" Width="65px" AutoPostBack="True" 
                onselectedindexchanged="DataBind_Finish_Days">
                    <asp:ListItem Value="2019">2019</asp:ListItem>     <asp:ListItem Value="2020">2020</asp:ListItem>     <asp:ListItem Value="2021">2021</asp:ListItem>
                    <asp:ListItem Value="2022">2022</asp:ListItem>     <asp:ListItem Value="2023">2023</asp:ListItem>     <asp:ListItem Value="2024">2024</asp:ListItem>
                    <asp:ListItem Value="2025">2025</asp:ListItem>
                </asp:DropDownList>
                <div class="divClear"></div>                                            
                <asp:Label ID="Label2"        CssClass="elmLeft_MB8R10_NB_AL_W200" runat="server" Text=" "  width="15px"></asp:Label>
                <asp:Button ID="Button1" runat="server" CssClass="elmLeft_MB8R10_NB_AL_W200" onclick="btn_ShowJournal_click" Text="Показать" />
                <div class="divClear"></div>                                            
                
        </div>

         <div class="blockSectionGray" id="Div2" runat="server">
            <asp:GridView ID="dbgJournal" runat="server" CellPadding="4" ForeColor="#333333" 
             AutoGenerateColumns="False" 
             DataSourceID="qryControl" 
             >
                <RowStyle BackColor="#F0EDE6" />
                <AlternatingRowStyle BackColor="#E6E3D2" />
                <HeaderStyle BackColor="Silver" Font-Bold="False" ForeColor="#666666" Font-Italic="False" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <EmptyDataRowStyle BackColor="LightBlue"/>
                <Columns>
                    <asp:TemplateField HeaderText="Дата выдачи" ItemStyle-Width="150px">
                        <ItemTemplate>
                                <asp:Label ID="lblDateSolution" runat="server"><%# Eval("strDateSolution")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Документ" ItemStyle-Width="250px">
                        <ItemTemplate>
                                <asp:Label ID="lblDocName" runat="server"><%# Eval("strDocName")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Контент" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblContName" runat="server"><%# Eval("strContName")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Гриф" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblGrif" runat="server"><%# Eval("strGrif")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Пользователь" ItemStyle-Width="580px">
                        <ItemTemplate>
                            <asp:Label ID="lblReader" runat="server"><%# Eval("strUser")%></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Доступ" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblAccessAllow" runat="server" Visible='<%# Eval("strStatus").ToString() == "1" %>'>Разрешен</asp:Label>
                            <asp:Label ID="lblAccessDisallow" runat="server" Visible='<%# Eval("strStatus").ToString() == "2" %>'>Запрещен</asp:Label>
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
