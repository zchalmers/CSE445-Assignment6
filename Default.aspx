<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Assignment6._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="intro-panel">
        <h1>CSE 445 Assignment 6</h1>
        <p class="lead">
            CSE 445 group project web app that lets users look up weather forecasts by ZIP code, save their favorite locations,
            and manage their account. Also includes a staff admin page and various service demos.
        </p>
        <div class="intro-actions">
            <a class="btn btn-default btn-lg" href="WeatherTryIt.aspx">Open Weather TryIt</a>
            <a class="btn btn-default btn-lg" href="DllTryIt.aspx">Open DLL TryIt</a>
            <a class="btn btn-default btn-lg" href="ServiceTryIt.aspx">Open Service TryIt</a>
            <a class="btn btn-default btn-lg" href="EncryptionTryIt.aspx">Open Encryption TryIt</a>
            <a class="btn btn-default btn-lg" href="XmlTryIt.aspx">Open XML TryIt</a>
        </div>
    </div>

<div class="row section-grid">
        <div class="col-md-6">
            <section class="info-card">
                <h2>Member Page</h2>
                <p>
                    Create an account to save ZIP codes and access your weather planner.
                    A CAPTCHA is required during sign-up.
                </p>
                <a class="btn btn-primary" href="Member.aspx">Open Member Page</a>
                <a class="btn btn-default" href="MemberRegister.aspx" style="margin-left:8px;">Register</a>
            </section>
        </div>
        <div class="col-md-6">
            <section class="info-card">
                <h2>Staff Page</h2>
                <p>
                    Staff access is controlled by the site administrator via
                    <code>Staff.xml</code>. Staff test account:
                    <strong>TA / Cse445!</strong>
                </p>
                <a class="btn btn-warning" href="Staff.aspx">Open Staff Page</a>
            </section>
        </div>
    </div>

    <section class="info-card">
        <h2>Service Directory</h2>
        <p class="text-muted">
            Team Contribution: Zach Chalmers 50% | Andrew Courter 50%
        </p>
        <div class="table-responsive">
            <table class="table table-bordered summary-table">
                <thead>
                    <tr>
                        <th>Provider</th>
                        <th>Component Type</th>
                        <th>Operation</th>
                        <th>Parameters</th>
                        <th>Description</th>
                        <th>TryIt</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>User Control (ASCX)</td>
                        <td>CaptchaControl.Validate</td>
                        <td>None</td>
                        <td>Shows a random math question and checks the user's answer. Answer is stored in Session. Used on the register page.</td>
                        <td><a href="MemberRegister.aspx">Register Page</a></td>
                    </tr>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>WCF Service</td>
                        <td>Service1.WebDownload / WordFilter</td>
                        <td>url, text (strings)</td>
                        <td>Downloads a webpage and strips HTML tags and common words from the content.</td>
                        <td><a href="ServiceTryIt.aspx">Service TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>REST API</td>
                        <td>NaturalHazards.GetHazards</td>
                        <td>lat, lng (doubles)</td>
                        <td>Returns a hazard report for a location using USGS earthquake and volcano data.</td>
                        <td><a href="ServiceTryIt.aspx">Service TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>DLL Class Library</td>
                        <td>CryptoHelper.Hash / Encrypt / Decrypt / VerifyHash</td>
                        <td>plaintext (string)</td>
                        <td>Handles SHA-256 hashing and AES encryption/decryption.</td>
                        <td><a href="EncryptionTryIt.aspx">Encryption TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>Utility Class</td>
                        <td>XmlVerification.Verify / Xml2Json</td>
                        <td>xmlUrl, xsdUrl (strings)</td>
                        <td>Validates XML against an XSD schema and can convert it to JSON.</td>
                        <td><a href="XmlTryIt.aspx">XML TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>DLL Class Library</td>
                        <td>ZipValidationHelper.ValidateZipInput / HashPassword</td>
                        <td>zip or password (string)</td>
                        <td>Validates a ZIP code string and returns a result object. HashPassword does SHA-256 hashing for login storage.</td>
                        <td><a href="DllTryIt.aspx">DLL TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>WCF Service</td>
                        <td>WeatherService.Weather5day</td>
                        <td>zipcode (string)</td>
                        <td>Takes a ZIP code and returns a 5-day forecast as a string array. Deployed on WebStrar.</td>
                        <td><a href="WeatherTryIt.aspx">Weather TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>Cookie State</td>
                        <td>LastZip Cookie</td>
                        <td>ZIP string</td>
                        <td>Saves the last searched ZIP in a cookie so it comes back on the next visit (30 day expiry).</td>
                        <td><a href="WeatherTryIt.aspx">Weather TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>Global.asax</td>
                        <td>Application_Start / Session_Start</td>
                        <td>None</td>
                        <td>Application_Start seeds Staff.xml if it doesn't exist. Session_Start increments a visit counter in Application state.</td>
                        <td>(Staff page)</td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>XML Storage</td>
                        <td>Member.xml / Staff.xml</td>
                        <td>username, password, ZIPs</td>
                        <td>Member.xml stores accounts and saved ZIPs. Staff.xml stores staff logins.</td>
                        <td><a href="Member.aspx">Member</a> / <a href="Staff.aspx">Staff</a></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </section>

</asp:Content>
