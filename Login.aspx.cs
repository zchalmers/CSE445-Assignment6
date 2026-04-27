using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml.Linq;
using ZipUtilities;

namespace Assignment6
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Already logged in -> redirect to the appropriate page
                if (User.Identity.IsAuthenticated)
                {
                    var ticket = ((FormsIdentity)User.Identity).Ticket;
                    if (ticket.UserData == "Staff")
                    {
                        Response.Redirect("~/Staff.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/Member.aspx");
                    }
                }

                // Handle logout from navbar link
                if (Request.QueryString["action"] == "logout")
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/Default.aspx");
                }
            }
        }

        // Check member credentials against Member.xml (password hashed via DLL)
        protected void btnMemberLogin_Click(object sender, EventArgs e)
        {
            string username = txtMemberUsername.Text.Trim();
            string password = txtMemberPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMemberError.Text = "Enter both username and password.";
                return;
            }

            string xmlPath = Server.MapPath("~/App_Data/Member.xml");
            if (!System.IO.File.Exists(xmlPath))
            {
                lblMemberError.Text = "No accounts found. Register first.";
                return;
            }

            string hash = ZipValidationHelper.HashPassword(password);
            XDocument doc = XDocument.Load(xmlPath);

            bool found = doc.Root.Elements("Member").Any(m =>
                m.Element("Username")?.Value == username &&
                m.Element("PasswordHash")?.Value == hash);

            if (!found)
            {
                lblMemberError.Text = "Invalid username or password.";
                return;
            }

            // Create auth ticket with "Member" role in UserData
            var authTicket = new FormsAuthenticationTicket( 
                1, username, DateTime.Now, DateTime.Now.AddMinutes(30), false, "Member");
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            string returnUrl = Request.QueryString["ReturnUrl"];
            Response.Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/Member.aspx");
        }

        // Check staff credentials against Staff.xml
        protected void btnStaffLogin_Click(object sender, EventArgs e)
        {
            string username = txtStaffUsername.Text.Trim();
            string password = txtStaffPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblStaffError.Text = "Enter both username and password.";
                return;
            }

            string xmlPath = Server.MapPath("~/App_Data/Staff.xml");
            if (!System.IO.File.Exists(xmlPath))
            {
                lblStaffError.Text = "No staff accounts found.";
                return;
            }

            string hash = ZipValidationHelper.HashPassword(password);
            XDocument doc = XDocument.Load(xmlPath);

            bool found = doc.Root.Elements("Member").Any(m =>
                m.Element("Username")?.Value == username &&
                m.Element("PasswordHash")?.Value == hash);

            if (!found)
            {
                lblStaffError.Text = "Invalid username or password.";
                return;
            }

            var authTicket = new FormsAuthenticationTicket(
                1, username, DateTime.Now, DateTime.Now.AddMinutes(30), false, "Staff");
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            Response.Redirect("~/Staff.aspx");
        }
    }
}
