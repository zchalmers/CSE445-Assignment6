<%@ Page Title="Encryption TryIt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EncryptionTryIt.aspx.cs" Inherits="Assignment6.EncryptionTryIt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .code{background:#0f1923;color:#7effb2;font-family:'Courier New',monospace;padding:12px;margin-top:10px;min-height:40px;white-space:pre-wrap;word-break:break-all;}
</style>

<h2>Encryption & Hash (DLL)</h2>
<p class="text-muted">Hash, encrypt, decrypt, and verify text locally.</p>

<div class="panel">
    <h2>SHA-256 Hash</h2>
    <p>Creates a one-way SHA-256 hash for password storage.</p>
    <label>Text to hash</label>
    <asp:TextBox ID="txtHashInput" runat="server" CssClass="form-control" placeholder="e.g. Cse445!" />
    <asp:Button ID="btnHash" runat="server" Text="Compute SHA-256" CssClass="btn btn-primary" OnClick="btnHash_Click" />
    <div class="code"><asp:Label ID="lblHashResult" runat="server" Text="Result here..." /></div>
</div>

<div class="panel">
    <h2>AES-256 Encrypt & Decrypt</h2>
    <p>Encrypts text and decrypts the most recent result.</p>
    <label>Text to encrypt</label>
    <asp:TextBox ID="txtEncInput" runat="server" CssClass="form-control" placeholder="e.g. Hello Weather Planner!" />
    <asp:Button ID="btnEncrypt" runat="server" Text="AES Encrypt" CssClass="btn btn-primary" OnClick="btnEncrypt_Click" />
    <asp:Button ID="btnDecrypt" runat="server" Text="Decrypt Last" CssClass="btn btn-default" OnClick="btnDecrypt_Click" />
    <div class="code"><asp:Label ID="lblEncResult" runat="server" Text="Result here..." /></div>
</div>

<div class="panel">
    <h2>Verify Hash</h2>
    <p>Checks whether plaintext matches a stored hash.</p>
    <label>Plaintext</label>
    <asp:TextBox ID="txtVerifyPlain" runat="server" CssClass="form-control" placeholder="e.g. Cse445!" />
    <label>Stored hash (paste a SHA-256 hex string)</label>
    <asp:TextBox ID="txtVerifyHash" runat="server" CssClass="form-control" placeholder="64-char hex..." />
    <asp:Button ID="btnVerify" runat="server" Text="Verify Hash" CssClass="btn btn-primary" OnClick="btnVerify_Click" />
    <div class="code"><asp:Label ID="lblVerifyResult" runat="server" Text="Result here..." /></div>
</div>

</asp:Content>
