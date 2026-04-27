<%@ Page Title="Member Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MemberLogin.aspx.cs" Inherits="Assignment6.MemberLogin" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" style="margin-top:20px;">
        <div class="col-md-6 col-md-offset-3">
            <div class="info-card">
                <h2><span class="glyphicon glyphicon-user"></span> Member Login</h2>
                <p class="text-muted">Sign in to access your weather planner.</p>
                <div class="form-group">
                    <label for="<%= txtUsername.ClientID %>">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"
                        placeholder="Enter your username" />
                </div>
                <div class="form-group">
                    <label for="<%= txtPassword.ClientID %>">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                        TextMode="Password" placeholder="Enter your password" />
                </div>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger" />
                <div style="margin-top:14px;">
                    <asp:Button ID="btnLogin" runat="server" Text="Sign In"
                        CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                    <a href="MemberRegister.aspx" class="btn btn-default" style="margin-left:8px;">
                        Create Account
                    </a>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
