<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="Assignment6.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hdnActiveTab" runat="server" Value="member" />

    <div class="row" style="margin-top:20px;">

        <%-- Member Login --%>
        <div class="col-md-5">
            <div class="info-card">
                <h2><span class="glyphicon glyphicon-user"></span> Member Login</h2>
                <p class="text-muted">Sign in to access your personal weather planner.</p>
                <div class="form-group">
                    <label for="<%= txtMemberUsername.ClientID %>">Username</label>
                    <asp:TextBox ID="txtMemberUsername" runat="server" CssClass="form-control"
                        placeholder="Enter your username" />
                </div>
                <div class="form-group">
                    <label for="<%= txtMemberPassword.ClientID %>">Password</label>
                    <asp:TextBox ID="txtMemberPassword" runat="server" CssClass="form-control"
                        TextMode="Password" placeholder="Enter your password" />
                </div>
                <asp:Label ID="lblMemberError" runat="server" CssClass="text-danger" />
                <div style="margin-top:14px;">
                    <asp:Button ID="btnMemberLogin" runat="server" Text="Sign In"
                        CssClass="btn btn-primary" />
                    <a href="MemberRegister.aspx" class="btn btn-default" style="margin-left:8px;">
                        Create Account
                    </a>
                </div>
            </div>
        </div>

        <div class="col-md-1 text-center" style="padding-top:80px;">
            <span class="text-muted" style="font-size:1.4em;">|</span>
        </div>

        <%-- Staff Login --%>
        <div class="col-md-5">
            <div class="info-card">
                <h2><span class="glyphicon glyphicon-cog"></span> Staff Login</h2>
                <p class="text-muted">Staff access is granted by the site administrator.</p>
                <div class="form-group">
                    <label for="<%= txtStaffUsername.ClientID %>">Username</label>
                    <asp:TextBox ID="txtStaffUsername" runat="server" CssClass="form-control"
                        placeholder="Staff username" />
                </div>
                <div class="form-group">
                    <label for="<%= txtStaffPassword.ClientID %>">Password</label>
                    <asp:TextBox ID="txtStaffPassword" runat="server" CssClass="form-control"
                        TextMode="Password" placeholder="Staff password" />
                </div>
                <asp:Label ID="lblStaffError" runat="server" CssClass="text-danger" />
                <div style="margin-top:14px;">
                    <asp:Button ID="btnStaffLogin" runat="server" Text="Sign In"
                        CssClass="btn btn-warning" />
                </div>
                <p class="text-muted" style="margin-top:10px; font-size:0.85em;">
                    <strong>Grader test credentials:</strong> TA / Cse445!
                </p>
            </div>
        </div>

    </div>

</asp:Content>
