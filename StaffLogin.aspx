<%@ Page Title="Staff Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="StaffLogin.aspx.cs" Inherits="Assignment6.StaffLogin" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" style="margin-top:20px;">
        <div class="col-md-6 col-md-offset-3">
            <div class="info-card">
                <h2><span class="glyphicon glyphicon-cog"></span> Staff Login</h2>
                <p class="text-muted">Staff access is granted by the site administrator.</p>
                <div class="form-group">
                    <label for="<%= txtUsername.ClientID %>">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"
                        placeholder="Staff username" />
                </div>
                <div class="form-group">
                    <label for="<%= txtPassword.ClientID %>">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                        TextMode="Password" placeholder="Staff password" />
                </div>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger" />
                <div style="margin-top:14px;">
                    <asp:Button ID="btnLogin" runat="server" Text="Sign In"
                        CssClass="btn btn-warning" OnClick="btnLogin_Click" />
                </div>
                <p class="text-muted" style="margin-top:10px; font-size:0.85em;">
                    <strong>Grader test credentials:</strong> TA / Cse445!
                </p>
            </div>
        </div>
    </div>

</asp:Content>
