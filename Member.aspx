<%@ Page Title="Member" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Member.aspx.cs" Inherits="Assignment6.Member" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Header --%>
    <div class="intro-panel" style="padding:14px 20px; margin-bottom:20px;">
        <div class="row">
            <div class="col-md-8">
                <h2 style="margin:0;">
                    <span class="glyphicon glyphicon-cloud"></span>
                    <asp:Label ID="lblWelcome" runat="server" />
                </h2>
                <p class="text-muted" style="margin:4px 0 0;">
                    Your 5-day weather planner. Search any ZIP code and save your favorites.
                </p>
            </div>
            <div class="col-md-4 text-right" style="padding-top:10px;">
                <asp:Button ID="btnLogout" runat="server" Text="Sign Out"
                    CssClass="btn btn-default" CausesValidation="false" />
            </div>
        </div>
    </div>

    <div class="row section-grid">

        <%-- Weather Search --%>
        <div class="col-md-8">
            <section class="info-card">
                <h3>5-Day Forecast</h3>

                <div class="input-group" style="margin-bottom:10px;">
                    <asp:TextBox ID="txtMemberZip" runat="server" CssClass="form-control"
                        placeholder="Enter ZIP code (e.g. 85281)" MaxLength="10" />
                    <span class="input-group-btn">
                        <asp:Button ID="btnGetForecast" runat="server" Text="Get Forecast"
                            CssClass="btn btn-primary" />
                    </span>
                </div>

                <asp:HiddenField ID="hdnCurrentZip" runat="server" />

                <small class="text-muted">
                    Normalized ZIP: <asp:Label ID="lblNormalized" runat="server" Text="-" />
                </small>

                <p style="margin-top:8px;">
                    <asp:Label ID="lblStatus" runat="server" CssClass="text-info" />
                </p>

                <asp:BulletedList ID="bltForecast" runat="server" CssClass="forecast-list" />

                <%-- Save ZIP (visible after forecast loads) --%>
                <asp:Panel ID="pnlSaveZip" runat="server" Visible="false" style="margin-top:14px;">
                    <asp:Button ID="btnSaveZip" runat="server" Text="&#9733; Save this ZIP"
                        CssClass="btn btn-sm btn-default" CausesValidation="false" />
                    <asp:Label ID="lblSaveStatus" runat="server" CssClass="text-success"
                        style="margin-left:8px;" />
                </asp:Panel>
            </section>
        </div>

        <%-- Saved ZIPs --%>
        <div class="col-md-4">
            <section class="info-card">
                <h3>Saved ZIP Codes</h3>
                <p class="text-muted" style="font-size:0.85em;">
                    Click a ZIP to load it, or remove ones you no longer need.
                </p>
                <asp:GridView ID="gvSavedZips" runat="server"
                    AutoGenerateColumns="false"
                    DataKeyNames="Zip"
                    CssClass="table table-bordered table-condensed"
                    EmptyDataText="No saved ZIP codes yet."
                    OnRowCommand="gvSavedZips_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Zip" HeaderText="ZIP" />
                        <asp:ButtonField CommandName="LoadZip" Text="Load"
                            ButtonType="Link" HeaderText="" />
                        <asp:ButtonField CommandName="DeleteZip" Text="Remove"
                            ButtonType="Link" HeaderText="" />
                    </Columns>
                </asp:GridView>
            </section>
        </div>

    </div>

    <%-- Change Password --%>
    <section class="info-card" style="margin-top:20px;">
        <h3>Change Password</h3>
        <div class="row">
            <div class="col-md-5">
                <div class="form-group">
                    <label>Current Password</label>
                    <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control"
                        TextMode="Password" placeholder="Current password" />
                </div>
                <div class="form-group">
                    <label>New Password</label>
                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control"
                        TextMode="Password" placeholder="New password (min 6 chars)" />
                </div>
                <div class="form-group">
                    <label>Confirm New Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control"
                        TextMode="Password" placeholder="Re-enter new password" />
                </div>
                <asp:Label ID="lblChangeStatus" runat="server" />
                <div>
                    <asp:Button ID="btnChangePassword" runat="server" Text="Update Password"
                        CssClass="btn btn-default" CausesValidation="false"
                        OnClick="btnChangePassword_Click" />
                </div>
            </div>
        </div>
    </section>

</asp:Content>
