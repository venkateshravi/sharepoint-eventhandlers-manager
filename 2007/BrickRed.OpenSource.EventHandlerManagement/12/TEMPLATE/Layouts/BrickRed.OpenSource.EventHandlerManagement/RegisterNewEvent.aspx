<%@ Assembly Name="BrickRed.OpenSource.EventHandlerManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=59a50039dd958923" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterNewEvent.aspx.cs"
    Inherits="BrickRed.OpenSource.EventHandlerManagement.RegisterNewEvent" EnableEventValidation="false" %>

<html>
<body>
    <form id="Form1" runat="server">
    <div style="margin: 0px 10px 10px 10px; padding-top: 10px; height: 100%;">
        <table style="width: 100%; border: 5px; font-family: Arial; font-size: 10pt;">
            <tr>
                <td style="background-color: #d3d3d3; width: 40%; height: 23px; padding-left: 3px;">
                    <asp:Label ID="lblEventName" Text="Event Type" runat="server"></asp:Label>
                </td>
                <td style="width: 60%">
                    <asp:DropDownList ID="ddlInActiveEvents" runat="server" AutoPostBack="false" Width="100%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="background-color: #d3d3d3; height: 23px; padding-left: 3px;">
                    <asp:Label ID="lblAssemblyName" runat="server" Text="Assembly Name"></asp:Label>
                    <span style="color: Red;">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtAssemblyName" runat="server" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="RegGrp"
                        Display="Dynamic" ControlToValidate="txtAssemblyName" Text="Assembly Name is required!"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td style="background-color: #d3d3d3; height: 23px; padding-left: 3px;">
                    <asp:Label ID="lblVersion" runat="server" Text="Version"></asp:Label>
                    <span style="color: Red;">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtVersion" runat="server" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="RegGrp"
                        Display="Dynamic" ControlToValidate="txtVersion" Text="Version  is required!"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td style="background-color: #d3d3d3; height: 23px; padding-left: 3px;">
                    <asp:Label ID="lblCulture" runat="server" Text="Culture"></asp:Label>
                    <span style="color: Red;">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtCulture" runat="server" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="RegGrp"
                        Display="Dynamic" ControlToValidate="txtCulture" Text="Culture  is required!"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td style="background-color: #d3d3d3; height: 23px; padding-left: 3px;">
                    <asp:Label ID="lblPublicKey" runat="server" Text="Public Key Token"></asp:Label>
                    <span style="color: Red;">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtPublicKeyToken" runat="server" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="RegGrp"
                        Display="Dynamic" ControlToValidate="txtPublicKeyToken" Text="Public Key Token  is required!"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td style="background-color: #d3d3d3; height: 23px; padding-left: 3px;">
                    <asp:Label ID="lblClassName" runat="server" Text="Class Name"></asp:Label>
                    <span style="color: Red;">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtClassName" runat="server" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="RegGrp"
                        Display="Dynamic" ControlToValidate="txtClassName" Text="Class Name  is required!"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table style="text-align: right; width: 100%; padding-top: 20px;">
            <tr align="right">
                <td>
                    <asp:Button ID="btnRegister" runat="server" Text="Register" ValidationGroup="RegGrp"
                        Width="70px" />
                    <span></span>
                    <asp:Button ID="btnCancel" runat="server" Text="Close" Width="70px" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
