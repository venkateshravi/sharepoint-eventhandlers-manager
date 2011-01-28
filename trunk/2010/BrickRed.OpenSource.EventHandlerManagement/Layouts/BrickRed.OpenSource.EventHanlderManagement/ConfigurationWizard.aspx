<%@ Assembly Name="BrickRed.OpenSource.EventHandlerManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9a972040ed116f24" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigurationWizard.aspx.cs"
    Inherits="BrickRed.OpenSource.EventHandlerManagement.ConfigurationWizard" DynamicMasterPageFile="~masterurl/default.master"
    EnableEventValidation="false" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript">

        //Dialog opening 
        function OpenDialog(url) {
            var options = SP.UI.$create_DialogOptions();
            options.url = url;
            options.width = 600;
            options.height = 280;
            options.dialogReturnValueCallback = Function.createDelegate(null, CloseCallback);
            SP.UI.ModalDialog.showModalDialog(options);
        }

        // Dialog callback 
        function CloseCallback(result, target) {

            if (result === SP.UI.DialogResult.OK) {
                __doPostBack('', '');
            }
            //if (result === SP.UI.DialogResult.cancel) {
            //alert("Cancel was clicked!");
            //}        

        }

        function __doPostBack(eventTarget, eventArgument) {
            if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                theForm.__EVENTTARGET.value = eventTarget;
                theForm.__EVENTARGUMENT.value = eventArgument;
                theForm.submit();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table style="width: 100%">
        <tr>
            <td align="right" style="padding-top: 10px; padding-right: 20px">
                <asp:LinkButton ID="btAddNewEvent" runat="server" Text="Register New Event Handler" />
            </td>
        </tr>
        <tr>
            <td style="padding-left: 10px">
                <b>Registered Event Handler(s)</b>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 10px; padding-right: 10px; padding-top: 5px;">
                <asp:GridView runat="server" ID="objGridView" OnRowDataBound="gridView_RowDataBound"
                    AllowPaging="false" PageSize="5" AllowSorting="false" AutoGenerateColumns="false"
                    Width="100%" EnableModelValidation="True" DataKeyNames="EVENTTYPE" HeaderStyle-CssClass="ms-vh2-nofilter ms-vh2-gridview"
                    BorderWidth="1px" CellPadding="2" EmptyDataText="NO_ITEMS_TO_DISPLAY">
                    <AlternatingRowStyle CssClass="ms-alternating" />
                    <Columns>
                        <asp:BoundField DataField="EVENTTYPE" HeaderText="Event Type" ControlStyle-Width="50%"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ASSEMBLYINFO" HeaderText="Assembly Information" ControlStyle-Width="50%"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="CLASSNAME" HeaderText="Class Name" ControlStyle-Width="50%"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="/_layouts/Images/Edit.GIF"
                                    OnClick="Edit_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/_layouts/Images/Delete.GIF"
                                    OnClick="Delete_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                               <asp:Label ID="lblGUID" runat="Server" Text='<%# Eval("DEFID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                       <%-- <asp:BoundField DataField="DEFID" Visible="false" />--%>
                    </Columns>
                    <HeaderStyle BackColor="#F0F0F0" CssClass="ms-vh2-nofilter ms-vh2-gridview" />
                    <PagerStyle HorizontalAlign="Right" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="left" style = "padding-left:10px;">
                <asp:Label ID="lblNoEventRegisteres" runat="server" Text="No Event Handler is registered."></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Brickred Event Handlers Management
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    Brickred Event Handlers Management
</asp:Content>
