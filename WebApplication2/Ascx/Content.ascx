<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Content.ascx.cs" Inherits="Content.ContentView" %>
    
    
    
    <asp:GridView ID="dbgAttachedFiles" runat="server" CellPadding="4" 
    ForeColor="#333333" 
    AutoGenerateColumns="False" 
    DataSourceID="qryContent"  
    Caption="" 
    DataKeyNames="ID"
    AllowSorting="True" 
    CaptionAlign="Left"
    onrowdatabound="gwTable_RowDataBound" 
    onrowcommand="dbgAttachedFiles_RowCommand" 
    onsorting="gwTable_Sorting" 
    onselectedindexchanged="dbgAttachedFiles_SelectedIndexChanged">
        <RowStyle BackColor="#F0EDE6" />
        <AlternatingRowStyle BackColor="#E6E3D2" />
        <HeaderStyle BackColor="Silver" Font-Bold="False" ForeColor="#666666" Font-Italic="False" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <EmptyDataRowStyle BackColor="LightBlue"/>
        <Columns>
            <asp:TemplateField HeaderText="Имя файла" ItemStyle-Width="300px">
                <ItemTemplate>
                    <asp:Label ID="lblDoc" runat="server"><%# Eval("strNameCont")%></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Тип" ItemStyle-Width="180px">
                <ItemTemplate>
                    <asp:Label ID="lblContType" runat="server"><%# Eval("strNameType")%></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Версия" ItemStyle-Width="80px">
                <ItemTemplate>
                    <asp:LinkButton ID="linkVerContent" runat="server"
                                    oncommand="linkVerClick"
                                    Text='<%# Eval("intVerContent")%>' 
                                    CommandArgument='<%# Eval("ID") %>'>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Версия" ItemStyle-Width="80px">
                <ItemTemplate>
                    <asp:Label ID="LabelVerContent" runat="server"
                                    Text='<%# Eval("intVerContent")%>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Гриф" ItemStyle-Width="180px">
                <ItemTemplate>
                    <asp:Label ID="lblGrif" runat="server"><%# Eval("strGrif")%></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Место хранения" ItemStyle-Width="380px">
                <ItemTemplate>
                    <asp:Label ID="lblStorageBuild" runat="server"><%# Eval("strStorageBuilding")%></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Создан" ItemStyle-Width="80px">
                <ItemTemplate>
                    <asp:Label ID="lblDateCreate" runat="server"><%# Eval("strDateCreate")%></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Изменен" ItemStyle-Width="80px">
                <ItemTemplate>
                    <asp:Label ID="lblDateModify" runat="server"><%# Eval("strDateModify")%></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="BtnContRequest"  CommandName="ContRequest"  CommandArgument='<%# Eval("ID") %>' Visible='<%# Eval("GR").ToString() == "2" %>'><i class="fa fa-lock fa-3x"></i></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="BtnContDownload" CommandName="ContDownload" CommandArgument='<%# Eval("ID") %>' Visible='<%# Eval("GR").ToString() == "1" %>'><i class="fa fa-download fa-3x"></i></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="BtnWait"                                    CommandArgument='<%# Eval("ID") %>' Visible='<%# Eval("GR").ToString() == "0" %>'><i class="fa fa-cog fa-spin fa-3x"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <center>
                <asp:Label ID="Label5"  runat="server" Text="Документ не содержит прикрепленных файлов" Width="1030px"></asp:Label>
            </center>
        </EmptyDataTemplate>
    </asp:GridView>

<asp:SqlDataSource ID="qryContent" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ArchiveConnString %>" 
    ProviderName="<%$ ConnectionStrings:ArchiveConnString.ProviderName %>" 
    >   
</asp:SqlDataSource>

    <div class="blockBreak" id="pnlBreak" runat="server" visible="false" ></div>
    <div class="blockAddUserInfo" id="pnlAddUser" runat="server" visible="false">
    <div class="blockError" id="pnlError" runat="server" visible="false">
            <asp:BulletedList ID="lstError" runat="server">
            </asp:BulletedList>
        </div >
        <asp:Label CssClass="elmCenter_MB8R10_NB_AL_W200" ID="lblAddContent" runat="server" Text="<b>Введите свои данные</b>"></asp:Label>
        
                <table width="99%">
                    <tr>
                        <td>
                            <div class="divClear10"></div>
                            <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblLogin"      runat="server" Width="200px">Логин</asp:Label>
                            <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtLogin"      runat="server" Width="350px" Text=""></asp:TextBox>
                            <div class="divClear10"></div>
                            <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblFam"      runat="server" Width="200px">Фамилия</asp:Label>
                            <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtFam"      runat="server" Width="350px" Text=""></asp:TextBox>
                            <div class="divClear"></div>
                            <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblName" runat="server" Width="200px">Имя</asp:Label>
                            <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtName" runat="server" Width="350px" Text=""></asp:TextBox>
                            <div class="divClear"></div>
                            <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblOtch" runat="server" Width="200px">Отчество</asp:Label>
                            <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtOtch" runat="server" Width="350px" Text=""></asp:TextBox>
                            <div class="divClear"></div>
                            <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblPodr"  runat="server" Width="200px">Подразделение</asp:Label>
                            <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtPodr"  runat="server" Width="350px" Text=""></asp:TextBox>
                            <div class="divClear"></div>
                            <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblDepartment"  runat="server" Width="200px">Отдел</asp:Label>
                            <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtDepartment"  runat="server" Width="350px" Text=""></asp:TextBox>
                            <div class="divClear"></div>
                            <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblDolzh"  runat="server" Width="200px">Должность</asp:Label>
                            <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtDolzh"  runat="server" Width="350px" Text=""></asp:TextBox>
                            <div class="divClear"></div>
                            <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblPhone"  runat="server" Width="200px">Телефон</asp:Label>
                            <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtPhone"  runat="server" Width="350px" Text=""></asp:TextBox>
                            <div class="divClear"></div>
                            <asp:Label        CssClass="elmLeft_MB8R10_BB_AL_W200" ID="lblEmail"  runat="server" Width="200px">Email</asp:Label>
                            <asp:TextBox      CssClass="elmLeft_MB8R10_B_AL_W200"  ID="edtEmail"  runat="server" Width="350px" Text=""></asp:TextBox>
                            <div class="divClear"></div>
                        </td>
                    </tr>
                </table>

        <asp:Button CssClass="elmBtnC" ID="btnClose" runat="server" 
                Text="Отмена" TabIndex="101" onclick="btnClose_Click"/>
        <asp:Button CssClass="elmBtnC" ID="btnSaveUser" runat="server" 
                Text="Сохранить" TabIndex="100" onclick="btnSave_Click"/>
    </div>



