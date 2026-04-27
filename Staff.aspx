<%@ Page Title="Staff" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Staff.aspx.cs" Inherits="Assignment6.Staff" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Header --%>
    <div class="intro-panel" style="padding:14px 20px; margin-bottom:20px;">
        <div class="row">
            <div class="col-md-8">
                <h2 style="margin:0;">
                    <span class="glyphicon glyphicon-cog"></span>
                    <asp:Label ID="lblStaffWelcome" runat="server" />
                </h2>
                <p class="text-muted" style="margin:4px 0 0;">
                    Staff administration panel - site statistics and member management.
                </p>
            </div>
            <div class="col-md-4 text-right" style="padding-top:10px;">
                <asp:Button ID="btnStaffLogout" runat="server" Text="Sign Out"
                    CssClass="btn btn-default" CausesValidation="false" />
            </div>
        </div>
    </div>

    <%-- Stats --%>
    <div class="row section-grid">
        <div class="col-md-4">
            <section class="info-card text-center">
                <h4 class="text-muted">Total Sessions</h4>
                <p style="font-size:2.5em; font-weight:bold; margin:0;">
                    <asp:Label ID="lblTotalVisits" runat="server" Text="0" />
                </p>
                <small class="text-muted">Unique browser sessions since last server restart.</small>
            </section>
        </div>
        <div class="col-md-4">
            <section class="info-card text-center">
                <h4 class="text-muted">Registered Members</h4>
                <p style="font-size:2.5em; font-weight:bold; margin:0;">
                    <asp:Label ID="lblTotalMembers" runat="server" Text="0" />
                </p>
                <small class="text-muted">Accounts stored in Member.xml.</small>
            </section>
        </div>
        <div class="col-md-4">
            <section class="info-card text-center">
                <h4 class="text-muted">Staff Access</h4>
                <p style="font-size:2.5em; font-weight:bold; margin:0;">
                    <span class="glyphicon glyphicon-ok text-success"></span>
                </p>
                <small class="text-muted">Credentials verified against Staff.xml.</small>
            </section>
        </div>
    </div>

    <%-- Members --%>
    <section class="info-card">
        <h3>Registered Members</h3>
        <p class="text-muted">All accounts currently in Member.xml.</p>
        <div class="table-responsive">
            <asp:GridView ID="gvMembers" runat="server"
                AutoGenerateColumns="false"
                CssClass="table table-bordered table-striped"
                EmptyDataText="No members have registered yet.">
                <Columns>
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:BoundField DataField="RegisteredDate" HeaderText="Registered" />
                    <asp:BoundField DataField="SavedZipCount" HeaderText="Saved ZIPs" />
                </Columns>
            </asp:GridView>
        </div>
    </section>

</asp:Content>
