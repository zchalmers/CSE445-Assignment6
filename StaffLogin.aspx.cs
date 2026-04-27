using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml.Linq;
using ZipUtilities;

namespace Assignment6
{
    public partial class StaffLogin : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["action"] == "logout")
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                // Already logged in as staff, go straight there
                if (User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Staff.aspx");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Enter both username and password.";
                return;
            }

            string xmlPath = Server.MapPath("~/App_Data/Staff.xml");
            if (!System.IO.File.Exists(xmlPath))
            {
                lblError.Text = "No staff accounts found.";
                return;
            }

            string hash = ZipValidationHelper.HashPassword(password);
            XDocument doc = XDocument.Load(xmlPath);

            bool found = doc.Root.Elements("Member").Any(m =>
                m.Element("Username")?.Value == username &&
                m.Element("PasswordHash")?.Value == hash);

            if (!found)
            {
                lblError.Text = "Invalid username or password.";
                return;
            }

            // Sign out any existing session first (e.g. member switching to staff)
            FormsAuthentication.SignOut();

            var authTicket = new FormsAuthenticationTicket(
                1, username, DateTime.Now, DateTime.Now.AddMinutes(30), false, "Staff");
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            Response.Redirect("~/Staff.aspx");
        }
    }
}
