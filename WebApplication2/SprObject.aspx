<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SprObject.aspx.cs" Inherits="WebApplication2.SprObject" %>
<%@ Register TagPrefix="MyControl" TagName="MyMenu" Src="~/Ascx/AdminMenu.ascx" %>
<%@ Register TagPrefix="MyControl" TagName="AppFutter" Src="~/Ascx/Futter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Справочник объектов</title>
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

            <div class="blockSection" ID="pnlObject" runat="server">
            <asp:Label        CssClass="elmLeft_MB8R10_NB_AL_W200" ID="lblObjects"      runat="server" Width="150px" Text="Объекты" ></asp:Label>
            <asp:DropDownList CssClass="elmLeft_MB8R10_NB_AL_W200" ID="ddlObjectType"   runat="server" Width="400px" TabIndex="201" AutoPostBack="True" onselectedindexchanged="ddlObjectType_SelectedIndexChanged" ></asp:DropDownList>
            <asp:Label        CssClass="elmBtnR_MR10"              ID="lblS1"           runat="server" Width="4px">&nbsp;</asp:Label>
            <asp:LinkButton   CssClass="elmBtnR_MR10_2"              ID="btnTable_Create" runat="server" TabIndex="202" onclick="btnTable_Create_Click" ForeColor="#4B6C9E" ><i class="fa fa-plus-square-o fa-3x"></i></asp:LinkButton>
            <div class="divClear"></div>
            
                <div id="pnlTableInsert" runat="server" visible="False">
                    <table width="99%">
                        <tr>
                            <td>
                                <div class="divClear10"></div>
                                <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblName"    runat="server" Width="200px">Наименование</asp:Label>
                                <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtName"    runat="server" Width="750px" Text=""></asp:TextBox>
                                <div class="divClear"></div>
                                <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblType"    runat="server" Width="200px">Тип объекта</asp:Label>
                                <asp:DropDownList CssClass="elmLeft_MB8R10_B_AL_W200"  ID="ddlType"    runat="server" Width="750px" DataSourceID="qryObjectType" DataTextField="strName" DataValueField="ID"></asp:DropDownList>
                                <div class="divClear"></div>
                                <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblInvNum"  runat="server" Width="200px">Инвентарный номер</asp:Label>
                                <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtInvNum"  runat="server" Width="750px" Text=""></asp:TextBox>
                                <div class="divClear"></div>
                            </td>
                            <td>
                                <center>
                                <asp:LinkButton ID="btnTable_Create_Submit" runat="server" ForeColor="#4B6C9E" onclick="btnTable_Create_Submit_Click"><i class="fa fa-check-square-o fa-3x"></i></asp:LinkButton><br /><br />
                                <asp:LinkButton ID="btnTable_Create_Cancel" runat="server" ForeColor="#E27474" onclick="btnTable_Create_Cancel_Click"><i class="faRed fa-times-circle-o fa-3x"></i></asp:LinkButton>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>

                    <asp:GridView CssClass="elmTable_SPR" ID="dbgTable" runat="server" DataSourceID="qryObject" AutoGenerateColumns="False" DataKeyNames="ID"
                    AllowPaging="True" CellPadding="3" GridLines="Vertical" PageSize="19" Width="1160px" ShowHeader="False"  
                    onrowcommand="dbgTable_RowCommand" onrowcancelingedit="dbgTable_RowCancelingEdit" onrowupdating="dbgTable_RowUpdating" 
                    EmptyDataText="Нет объектов для данных условий" >

                    <EditRowStyle CssClass="elmTable_SPR" />
                    <RowStyle BackColor="#EEEEEE"/>
                    <AlternatingRowStyle BackColor="#DCDCDC"/>
                    <PagerStyle BackColor="#C0C0C0" HorizontalAlign="Center" />

                    <Columns>
                        <asp:TemplateField  >
                            <EditItemTemplate >
                                <div class="divClear10"></div>
                                <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblName"    runat="server" Width="200px">Наименование</asp:Label>
                                <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtName"    runat="server" Width="750px" Text='<%# Bind("strName") %>'></asp:TextBox>
                                <div class="divClear"></div>
                                <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblType"    runat="server" Width="200px">Тип объекта</asp:Label>
                                <asp:DropDownList CssClass="elmLeft_MB8R10_B_AL_W200"  ID="ddlType"    runat="server" Width="750px" DataSourceID="qryObjectType" DataTextField="strName" DataValueField="ID" SelectedValue='<%# Bind("ID_OT") %>'></asp:DropDownList>
                                <div class="divClear"></div>
                                <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblInvNum"  runat="server" Width="200px">Инвентарный номер</asp:Label>
                                <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtInvNum"  runat="server" Width="750px" Text='<%# Bind("strInvNum") %>'></asp:TextBox>
                                <div class="divClear"></div>
                            </EditItemTemplate>  
                            <ItemTemplate>
                                <asp:Label CssClass="elmLeft_MR20"  ID="lblNameOT"   runat="server" Width="100px"  Text='<%# Bind("strNameShort") %>'></asp:Label>
                                <asp:Label CssClass="elmLeft_MR20"  ID="lblName"     runat="server" Width="600px"  Text='<%# Bind("strName") %>'></asp:Label>
                                <asp:Label CssClass="elmRight_ML20" ID="lblInvNum"   runat="server" Width="100px"  Text='<%# Bind("strInvNum") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <center>
                                <asp:LinkButton ID="btnTable_Update_Submit" runat="server" CommandName="Update" ForeColor="#4B6C9E"><i class="fa fa-check-square-o fa-3x"></i></asp:LinkButton><br /><br />
                                <asp:LinkButton ID="btnTable_Update_Cancel" runat="server" CommandName="Cancel" ForeColor="#E27474"><i class="faRed fa-times-circle-o fa-3x"></i></asp:LinkButton>
                                </center>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <center>
                                <asp:LinkButton ID="btnTable_Edit"          runat="server" CommandName="Edit"   ForeColor="#4B6C9E"><i class="fa fa-edit fa-3x"></i></asp:LinkButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:SqlDataSource ID="qryObjectType" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ArchiveConnString %>" 
                    ProviderName="<%$ ConnectionStrings:ArchiveConnString.ProviderName %>" 
                    SelectCommand="SELECT 0 as ID, ' ' as strName UNION SELECT ID, strName FROM a_Object_Type ORDER BY strName">            
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="qryObject" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ArchiveConnString %>" 
                    ProviderName="<%$ ConnectionStrings:ArchiveConnString.ProviderName %>" 
                    UpdateCommand="SELECT 1" >  
                </asp:SqlDataSource>
            </div>

            <mycontrol:AppFutter ID="Futter" runat="server" />
        </div>
    </form>
</body>
</html>
