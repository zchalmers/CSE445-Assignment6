using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Newtonsoft.Json;
using ZipUtilities;

namespace Assignment6
{
    public partial class Member : Page
    {
        private static readonly string WeatherBaseUrl =
            ConfigurationManager.AppSettings["WeatherServiceBaseUrl"];
        private const string LastZipCookieName = "LastZip";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Must be logged in as Member
            if (!User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            var ticket = ((FormsIdentity)User.Identity).Ticket;
            if (ticket.UserData != "Member")
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/MemberLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblWelcome.Text = "Welcome, " + Server.HtmlEncode(User.Identity.Name) + "!";
                LoadSavedZips();

                // Pre-fill from cookie if available
                HttpCookie zipCookie = Request.Cookies[LastZipCookieName];
                if (zipCookie != null && !string.IsNullOrEmpty(zipCookie.Value))
                {
                    txtMemberZip.Text = zipCookie.Value;
                }
            }
        }

        // Validate ZIP with DLL, then call the weather service
        protected void btnGetForecast_Click(object sender, EventArgs e)
        {
            bltForecast.Items.Clear();
            pnlSaveZip.Visible = false;
            lblSaveStatus.Text = string.Empty;

            var validation = ZipValidationHelper.ValidateZipInput(txtMemberZip.Text);
            lblNormalized.Text = string.IsNullOrWhiteSpace(validation.NormalizedZip) ? "-" : validation.NormalizedZip;

            if (!validation.IsValid)
            {
                lblStatus.Text = validation.Message;
                return;
            }

            string baseUrl = (WeatherBaseUrl ?? "").Trim();
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                lblStatus.Text = "Weather service URL not configured.";
                return;
            }
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            string requestUrl = baseUrl + "Weather5day?zipcode=" +
                Server.UrlEncode(validation.NormalizedZip);

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    string json = client.DownloadString(requestUrl);
                    var lines = JsonConvert.DeserializeObject<List<string>>(json);

                    if (lines == null || lines.Count == 0)
                    {
                        lblStatus.Text = "No forecast data returned.";
                        return;
                    }

                    foreach (string line in lines)
                    {
                        bltForecast.Items.Add(line);
                    }
                }

                // Save last searched zip in cookie
                var cookie = new HttpCookie(LastZipCookieName, validation.NormalizedZip);
                cookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(cookie);

                lblStatus.Text = "Forecast loaded.";
                hdnCurrentZip.Value = validation.NormalizedZip;
                pnlSaveZip.Visible = true;
            }
            catch (Exception)
            {
                lblStatus.Text = "Could not reach the weather service.";
            }
        }

        // Save the current ZIP to this member's XML entry
        protected void btnSaveZip_Click(object sender, EventArgs e)
        {
            string zip = hdnCurrentZip.Value;
            if (string.IsNullOrEmpty(zip)) return;

            string xmlPath = Server.MapPath("~/App_Data/Member.xml");
            if (!System.IO.File.Exists(xmlPath)) return;

            XDocument doc = XDocument.Load(xmlPath);
            XElement memberEl = doc.Root.Elements("Member").FirstOrDefault(m =>
                m.Element("Username")?.Value == User.Identity.Name);

            if (memberEl == null) return;

            XElement savedZipsEl = memberEl.Element("SavedZips");
            if (savedZipsEl == null)
            {
                savedZipsEl = new XElement("SavedZips");
                memberEl.Add(savedZipsEl);
            }

            bool alreadySaved = savedZipsEl.Elements("Zip").Any(z => z.Value == zip);
            if (alreadySaved)
            {
                lblSaveStatus.Text = "Already saved.";
            }
            else
            {
                savedZipsEl.Add(new XElement("Zip", zip));
                doc.Save(xmlPath);
                lblSaveStatus.Text = zip + " saved!";
                LoadSavedZips();
            }
        }

        // GridView row commands for loading/deleting saved ZIPs
        protected void gvSavedZips_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            string zip = gvSavedZips.Rows[rowIndex].Cells[0].Text;

            if (e.CommandName == "LoadZip")
            {
                txtMemberZip.Text = zip;
            }
            else if (e.CommandName == "DeleteZip")
            {
                RemoveSavedZip(zip);
                LoadSavedZips();
            }
        }

        private void RemoveSavedZip(string zip)
        {
            string xmlPath = Server.MapPath("~/App_Data/Member.xml");
            if (!System.IO.File.Exists(xmlPath)) return;

            XDocument doc = XDocument.Load(xmlPath);
            XElement memberEl = doc.Root.Elements("Member").FirstOrDefault(m =>
                m.Element("Username")?.Value == User.Identity.Name);

            if (memberEl != null)
            {
                XElement savedZipsEl = memberEl.Element("SavedZips");
                if (savedZipsEl != null)
                {
                    XElement toRemove = savedZipsEl.Elements("Zip")
                        .FirstOrDefault(z => z.Value == zip);
                    if (toRemove != null)
                    {
                        toRemove.Remove();
                        doc.Save(xmlPath);
                    }
                }
            }
        }

        // Load saved ZIPs from XML into the GridView
        private void LoadSavedZips()
        {
            string xmlPath = Server.MapPath("~/App_Data/Member.xml");
            if (!System.IO.File.Exists(xmlPath)) return;

            XDocument doc = XDocument.Load(xmlPath);
            XElement memberEl = doc.Root.Elements("Member").FirstOrDefault(m =>
                m.Element("Username")?.Value == User.Identity.Name);

            var dt = new DataTable();
            dt.Columns.Add("Zip");

            if (memberEl != null)
            {
                XElement savedZipsEl = memberEl.Element("SavedZips");
                if (savedZipsEl != null)
                {
                    foreach (XElement zipEl in savedZipsEl.Elements("Zip"))
                    {
                        dt.Rows.Add(zipEl.Value);
                    }
                }
            }

            gvSavedZips.DataSource = dt;
            gvSavedZips.DataBind();
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            lblChangeStatus.CssClass = "text-danger";
            lblChangeStatus.Text = string.Empty;

            string oldPw = txtOldPassword.Text;
            string newPw = txtNewPassword.Text;
            string confirmPw = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(oldPw) || string.IsNullOrEmpty(newPw) || string.IsNullOrEmpty(confirmPw))
            {
                lblChangeStatus.Text = "All three fields are required.";
                return;
            }

            if (newPw.Length < 6)
            {
                lblChangeStatus.Text = "New password must be at least 6 characters.";
                return;
            }

            if (newPw != confirmPw)
            {
                lblChangeStatus.Text = "New passwords do not match.";
                return;
            }

            string xmlPath = Server.MapPath("~/App_Data/Member.xml");
            if (!System.IO.File.Exists(xmlPath))
            {
                lblChangeStatus.Text = "Account data not found.";
                return;
            }

            XDocument doc = XDocument.Load(xmlPath);
            string oldHash = ZipValidationHelper.HashPassword(oldPw);

            XElement memberEl = doc.Root.Elements("Member").FirstOrDefault(m =>
                m.Element("Username")?.Value == User.Identity.Name &&
                m.Element("PasswordHash")?.Value == oldHash);

            if (memberEl == null)
            {
                lblChangeStatus.Text = "Current password is incorrect.";
                return;
            }

            memberEl.Element("PasswordHash").Value = ZipValidationHelper.HashPassword(newPw);
            doc.Save(xmlPath);

            txtOldPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;

            lblChangeStatus.CssClass = "text-success";
            lblChangeStatus.Text = "Password updated successfully.";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}
