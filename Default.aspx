<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Assignment6._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="intro-panel">
        <h1>5-Day Weather Planner</h1>
        <p class="lead">
            Enter a ZIP code to get a five-day forecast from our deployed WCF weather service.
            Members can save ZIP codes for quick access, and staff can view site stats
            and registered accounts.
        </p>
        <div class="intro-actions">
            <a class="btn btn-default btn-lg" href="WeatherTryIt.aspx">Open Weather TryIt</a>
            <a class="btn btn-default btn-lg" href="DllTryIt.aspx">Open DLL TryIt</a>
        </div>
    </div>

    <div class="row section-grid">
        <div class="col-md-6">
            <section class="info-card">
                <h2>How To Test</h2>
                <ol>
                    <li>Use <strong>Weather TryIt</strong> to look up any ZIP code.</li>
                    <li>Use <strong>DLL TryIt</strong> to test ZIP validation/normalization.</li>
                    <li>Click <strong>Member Page</strong> to create an account and save ZIPs.</li>
                    <li>Click <strong>Staff Page</strong> and log in as <code>TA / Cse445!</code>
                        to see the member list and site stats.</li>
                </ol>
            </section>
        </div>
        <div class="col-md-6">
            <section class="info-card">
                <h2>Sample Inputs</h2>
                <ul>
                    <li><strong>85281</strong> - valid Arizona ZIP</li>
                    <li><strong>85281-1234</strong> - normalized to first 5 digits by DLL</li>
                    <li><strong>abc</strong> - invalid input (DLL rejects it)</li>
                    <li><strong>94105</strong> - San Francisco, CA</li>
                    <li><strong>10001</strong> - New York, NY</li>
                </ul>
            </section>
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
            Main deployment URL:
            <strong>http://webstrarportal-env.eba-uzcvm8rb.us-west-2.elasticbeanstalk.com/sites/website163/Page0/Default.aspx</strong>
        </p>
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
                        <th>Return Type</th>
                        <th>Description</th>
                        <th>TryIt Link</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>User Control (ASCX)</td>
                        <td>CaptchaControl.Validate</td>
                        <td>None (reads Session + TextBox input)</td>
                        <td>bool</td>
                        <td>Math CAPTCHA user control. Generates a random addition question, stores the answer in Session, and validates the user's answer on submit. Used on the registration page.</td>
                        <td><a href="MemberRegister.aspx">Register Page</a></td>
                    </tr>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>WCF Service (WSDL)</td>
                        <td>Service1.WebDownload / Service1.WordFilter</td>
                        <td>url (string), text (string)</td>
                        <td>string</td>
                        <td>Downloads webpage content and filters text by removing tags and stop words.</td>
                        <td><a href="ServiceTryIt.aspx">Service TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>RESTful Web API</td>
                        <td>NaturalHazards.GetHazards</td>
                        <td>lat (double), lng (double)</td>
                        <td>string (report)</td>
                        <td>Returns earthquake and volcano risk report for given coordinates via USGS APIs.</td>
                        <td><a href="ServiceTryIt.aspx">Service TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>DLL Class Library</td>
                        <td>CryptoHelper.Hash / Encrypt / Decrypt / VerifyHash</td>
                        <td>plaintext (string)</td>
                        <td>string / bool</td>
                        <td>SHA-256 hashing and AES-256 encryption. Local computation only.</td>
                        <td><a href="EncryptionTryIt.aspx">Encryption TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Andrew Courter</td>
                        <td>Utility Class</td>
                        <td>XmlVerification.Verify / Xml2Json</td>
                        <td>xmlUrl (string), xsdUrl (string)</td>
                        <td>string</td>
                        <td>Validates XML against XSD schema, and converts XML to JSON.</td>
                        <td><a href="XmlTryIt.aspx">XML TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>DLL Class Library</td>
                        <td>ZipValidationHelper.ValidateZipInput / HashPassword</td>
                        <td>ZIP input (string) / password (string)</td>
                        <td>ZipValidationResult / string (SHA-256 hex)</td>
                        <td>Validates and normalizes a ZIP code string; also hashes passwords with SHA-256 for credential storage.</td>
                        <td><a href="DllTryIt.aspx">DLL TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>WCF Service</td>
                        <td>WeatherService.Weather5day</td>
                        <td>zipcode (string)</td>
                        <td>string[] (5 lines)</td>
                        <td>Returns a five-day forecast for the given ZIP code. Deployed on WebStrar.</td>
                        <td><a href="WeatherTryIt.aspx">Weather TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>Cookie State</td>
                        <td>LastZip Cookie</td>
                        <td>ZIP string</td>
                        <td>Cookie (expires 30 days)</td>
                        <td>Saves the last valid ZIP so it persists across browser sessions.</td>
                        <td><a href="WeatherTryIt.aspx">Weather TryIt</a></td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>Global.asax</td>
                        <td>Application_Start / Session_Start</td>
                        <td>None</td>
                        <td>Application state counter</td>
                        <td>Seeds Staff.xml with the TA account on startup. Increments a visit counter each session.</td>
                        <td>(counter shown on Staff page)</td>
                    </tr>
                    <tr>
                        <td>Zach Chalmers</td>
                        <td>XML Data Storage</td>
                        <td>Member.xml / Staff.xml read-write</td>
                        <td>username, passwordHash, saved ZIPs</td>
                        <td>XML document state</td>
                        <td>Stores member accounts, staff credentials, and member saved ZIP data in XML files.</td>
                        <td><a href="Member.aspx">Member Page</a> / <a href="Staff.aspx">Staff Page</a></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </section>

</asp:Content>
