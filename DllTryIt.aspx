<%@ Page Title="DLL TryIt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DllTryIt.aspx.cs" Inherits="Assignment6.DllTryIt" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="info-card tryit-shell">
        <h2>DLL TryIt</h2>
        <p>
            This page tests the local DLL component. The ZIP-validation DLL normalizes input and checks
            whether the ZIP code is valid.
        </p>

        <div class="form-group">
            <label for="<%= txtZipInput.ClientID %>">ZIP Code Input</label>
            <asp:TextBox ID="txtZipInput" runat="server" CssClass="form-control" placeholder="Enter a ZIP code" />
        </div>
        <asp:Button ID="btnValidateZip" runat="server" CssClass="btn btn-primary" Text="Run DLL Validation" OnClick="btnValidateZip_Click" />

        <div class="result-panel">
            <h3>DLL Output</h3>
            <p><strong>Normalized ZIP:</strong> <asp:Label ID="lblNormalizedZip" runat="server" Text="" /></p>
            <p><strong>Validation Result:</strong> <asp:Label ID="lblValidationState" runat="server" Text="" /></p>
            <p><strong>Message:</strong> <asp:Label ID="lblValidationMessage" runat="server" Text="" /></p>
        </div>
    </section>
</asp:Content>
