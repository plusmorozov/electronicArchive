<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminContent.ascx.cs" Inherits="Content.AdminContentView" %>
    
    
    
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
                        <asp:LinkButton runat="server" ID="BtnContEdit"     CommandName="ContEdit"     CommandArgument='<%# Eval("ID") %>' Visible=true><i class="fa fa-edit fa-3x"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="BtnContDownload" CommandName="ContDownload" CommandArgument='<%# Eval("ID") %>' Visible=true><i class="fa fa-download fa-3x"></i></asp:LinkButton>
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
        <div class="blockPrevVersionContent" id="pnlPrevContent" runat="server" visible="false">
            
           

            <asp:Button CssClass="elmBtnC" ID="btnClose" runat="server" 
                Text="Закрыть" TabIndex="101" onclick="btnClose_Click"/>
            <br />

            <br />
            
    </div>

