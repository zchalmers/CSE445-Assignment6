<%@ Page Title="XML TryIt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="XmlTryIt.aspx.cs" Inherits="Assignment6.XmlTryIt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .code{background:#0f1923;color:#7effb2;font-family:'Courier New',monospace;padding:12px;margin-top:10px;max-height:380px;overflow-y:auto;white-space:pre-wrap;word-break:break-all;}
</style>

<h2>XML TryIt Page</h2>
<p class="text-muted">XmlVerification: Verify + Xml2Json</p>

<!-- Verify Valid XML -->
<div class="panel">
    <h2>Validate Sample XML</h2>
    <asp:Button ID="btnVerifyValid" runat="server" Text="Validate XML" CssClass="btn btn-primary" OnClick="btnVerifyValid_Click" />
    <div class="code"><asp:Label ID="lblVerifyValid" runat="server" Text="Ready." /></div>
</div>

<!-- Verify Error XML -->
<div class="panel">
    <h2>Validate XML With Errors</h2>
    <asp:Button ID="btnVerifyError" runat="server" Text="Validate Error Sample" CssClass="btn btn-primary" OnClick="btnVerifyError_Click" />
    <div class="code"><asp:Label ID="lblVerifyError" runat="server" Text="Ready." /></div>
</div>

<!-- Convert to JSON -->
<div class="panel">
    <h2>Convert Sample XML to JSON</h2>
    <asp:Button ID="btnConvertValid" runat="server" Text="Convert Valid XML to JSON" CssClass="btn btn-success" OnClick="btnConvertValid_Click" />
    <div class="code"><asp:Label ID="lblJsonValid" runat="server" Text="Ready." /></div>
    <div class="sess"><strong>Cached conversion:</strong> <asp:Label ID="lblSessionCache" runat="server" Text="(nothing cached yet)" /></div>
</div>

<div class="panel">
    <h2>Convert Custom XML URL</h2>
    <label>Custom XML URL</label>
    <asp:TextBox ID="txtCustomUrl" runat="server" CssClass="form-control"
        Text="https://asuandrew.github.io/cse445-assignment4/NationalParks.xml" />
    <asp:Button ID="btnConvertCustom" runat="server" Text="Convert Custom XML to JSON" CssClass="btn btn-primary" OnClick="btnConvertCustom_Click" />
    <div class="code"><asp:Label ID="lblJsonCustom" runat="server" Text="Ready." /></div>
</div>

</asp:Content>
