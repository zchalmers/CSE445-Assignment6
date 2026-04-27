<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaptchaControl.ascx.cs" Inherits="Assignment6.CaptchaControl" %>
<%-- CaptchaControl.ascx - Math CAPTCHA User Control
     Used in: MemberRegister.aspx (registration)
     Public method: bool Validate() - call from parent page on form submit
--%>
<div style="background:#fff8e1;border:1px solid #e0b830;padding:14px 18px;
            margin:8px 0;font-family:Arial,sans-serif;border-left:4px solid #c8941a;">
    <strong>Human Verification</strong>
    <p style="color:#666;margin:5px 0 8px;">Solve this math problem:</p>
    <p style="font-size:1.3rem;font-weight:bold;font-family:'Courier New',monospace;
              background:#fff;display:inline-block;padding:5px 14px;border:1px solid #ccc;">
        <asp:Label ID="lblQuestion" runat="server" />
    </p>
    <br />
    <asp:TextBox ID="txtAnswer" runat="server" placeholder="Answer"
        style="padding:7px 11px;border:1px solid #ccc;font-size:1rem;
               width:90px;margin-top:7px;font-family:'Courier New',monospace;" />
    <asp:Label ID="lblCaptchaMsg" runat="server"
        style="margin-left:10px;font-weight:bold;" />
</div>
