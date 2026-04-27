using System;
using System.Linq;
using System.Web.UI;
using System.Xml.Linq;
using ZipUtilities;

namespace Assignment6
{
    public partial class MemberRegister : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Member.aspx");
            }
        }

        // Register new member: validate inputs + captcha, hash password, write to XML
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            lblRegError.Text = string.Empty;
            lblRegSuccess.Text = string.Empty;

            string username = txtRegUsername.Text.Trim();
            string password = txtRegPassword.Text;
            string confirm = txtRegConfirm.Text;

            if (string.IsNullOrEmpty(username))
            {
                lblRegError.Text = "Username is required.";
                return;
            }

            if (username.Length < 3)
            {
                lblRegError.Text = "Username must be at least 3 characters.";
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                lblRegError.Text = "Password is required.";
                return;
            }

            if (password.Length < 6)
            {
                lblRegError.Text = "Password must be at least 6 characters.";
                return;
            }

            if (password != confirm)
            {
                lblRegError.Text = "Passwords do not match.";
                return;
            }

            // Validate captcha
            var captcha = (CaptchaControl)ucCaptcha;
            if (!captcha.Validate())
            {
                lblRegError.Text = "Wrong answer. Try again.";
                return;
            }

            // Check for duplicate username
            string xmlPath = Server.MapPath("~/App_Data/Member.xml");
            XDocument doc;
            if (System.IO.File.Exists(xmlPath))
            {
                doc = XDocument.Load(xmlPath);
            }
            else
            {
                doc = new XDocument(new XElement("Members"));
            }

            bool duplicate = doc.Root.Elements("Member").Any(m =>
                m.Element("Username")?.Value == username);

            if (duplicate)
            {
                lblRegError.Text = "Username already taken.";
                return;
            }

            // Hash password with DLL and save to XML
            string hash = ZipValidationHelper.HashPassword(password);

            doc.Root.Add(new XElement("Member",
                new XElement("Username", username),
                new XElement("PasswordHash", hash),
                new XElement("RegisteredDate", DateTime.Now.ToShortDateString()),
                new XElement("SavedZips")
            ));

            doc.Save(xmlPath);

            lblRegSuccess.Text = "Account created! You can now ";
            lblRegSuccess.Text += "<a href='MemberLogin.aspx'>sign in</a>.";
            txtRegUsername.Text = string.Empty;
        }
    }
}
