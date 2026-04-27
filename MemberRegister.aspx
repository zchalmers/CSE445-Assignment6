<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MemberRegister.aspx.cs" Inherits="Assignment6.MemberRegister" %>
<%@ Register TagPrefix="uc" TagName="CaptchaControl" Src="~/CaptchaControl.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="margin-top:20px;">
        <div class="col-md-6 col-md-offset-3">
            <div class="info-card">
                <h2><span class="glyphicon glyphicon-user"></span> Create Member Account</h2>
                <p class="text-muted">
                    Sign up to save your ZIP codes and use the weather planner.
                </p>

                <div class="form-group">
                    <label for="<%= txtRegUsername.ClientID %>">Username</label>
                    <asp:TextBox ID="txtRegUsername" runat="server" CssClass="form-control"
                        placeholder="Choose a username" MaxLength="50" />
                </div>

                <div class="form-group">
                    <label for="<%= txtRegPassword.ClientID %>">Password</label>
                    <asp:TextBox ID="txtRegPassword" runat="server" CssClass="form-control"
                        TextMode="Password" placeholder="Choose a password" />
                </div>

                <div class="form-group">
                    <label for="<%= txtRegConfirm.ClientID %>">Confirm Password</label>
                    <asp:TextBox ID="txtRegConfirm" runat="server" CssClass="form-control"
                        TextMode="Password" placeholder="Re-enter password" />
                </div>

                <%-- CAPTCHA --%>
                <div class="form-group">
                    <label>Security Check</label>
                    <uc:CaptchaControl ID="ucCaptcha" runat="server" />
                </div>

                <asp:Label ID="lblRegError" runat="server" CssClass="text-danger" />
                <asp:Label ID="lblRegSuccess" runat="server" CssClass="text-success" />

                <div style="margin-top:14px;">
                    <asp:Button ID="btnRegister" runat="server" Text="Create Account"
                        CssClass="btn btn-primary" OnClick="btnRegister_Click" />
                    <a href="MemberLogin.aspx" class="btn btn-default" style="margin-left:8px;">
                        Back to Login
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
