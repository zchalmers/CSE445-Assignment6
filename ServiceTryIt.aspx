<%@ Page Title="Services TryIt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ServiceTryIt.aspx.cs" Inherits="Assignment6.ServiceTryIt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .code{background:#0f1923;color:#7effb2;font-family:'Courier New',monospace;padding:12px;margin-top:10px;max-height:280px;overflow-y:auto;white-space:pre-wrap;word-break:break-all;}
</style>

<h2>Services TryIt Page</h2>
<p class="text-muted">WebDownload, WordFilter, and NaturalHazards service tests</p>

<div class="panel">
    <h2>WebDownload (WCF)</h2>
    <div class="info">Calls Service1.WebDownload() through the deployed WCF endpoint.</div>
    <label>URL to download</label>
    <asp:TextBox ID="txtDlUrl" runat="server" Text="https://www.public.asu.edu/~ychen10/teaching/honors.html" CssClass="form-control" />
    <asp:Button ID="btnWebDownload" runat="server" Text="Download" CssClass="btn btn-success" OnClick="btnWebDownload_Click" />
    <div class="code"><asp:Label ID="lblWebDownload" runat="server" Text="Ready." /></div>
</div>

<div class="panel">
    <h2>WordFilter (WCF)</h2>
    <div class="info">Calls Service1.WordFilter() - strips HTML tags and stop words</div>
    <label>Text or HTML to filter</label>
    <asp:TextBox ID="txtFilterText" runat="server" TextMode="MultiLine" CssClass="form-control"
        Text="The quick brown fox is jumping over the lazy dog in a green field and the sun is shining." />
    <asp:Button ID="btnWordFilter" runat="server" Text="Filter Words" CssClass="btn btn-success" OnClick="btnWordFilter_Click" />
    <div class="code"><asp:Label ID="lblWordFilter" runat="server" Text="Ready." /></div>
</div>

<div class="panel">
    <h2>NaturalHazards (RESTful GET)</h2>
    <div class="info">Calls GET api/NaturalHazards/GetHazards?lat=&amp;lng= - USGS earthquake + volcano data</div>
    <label>Latitude</label>
    <asp:TextBox ID="txtLat" runat="server" Text="33.4255" CssClass="form-control" style="max-width:200px;" />
    <label>Longitude</label>
    <asp:TextBox ID="txtLng" runat="server" Text="-111.9400" CssClass="form-control" style="max-width:200px;" />
    <p class="text-muted">May take 15-30 seconds (calls two live USGS APIs).</p>
    <asp:Button ID="btnHazards" runat="server" Text="Get Hazard Report" CssClass="btn btn-success" OnClick="btnHazards_Click" />
    <div class="code"><asp:Label ID="lblHazards" runat="server" Text="Ready." /></div>
</div>

</asp:Content>
