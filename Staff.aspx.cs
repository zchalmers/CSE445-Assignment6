using System;
using System.Data;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Xml.Linq;

namespace Assignment6
{
    public partial class Staff : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Must be logged in as Staff
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/StaffLogin.aspx");
                return;
            }

            var ticket = ((FormsIdentity)User.Identity).Ticket;
            if (ticket.UserData != "Staff")
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/StaffLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblStaffWelcome.Text = "Welcome, " + Server.HtmlEncode(User.Identity.Name) + "!";
                LoadStats();
                LoadMembers();
            }
        }

        // Pull stats from Application state + Member.xml
        private void LoadStats()
        {
            int visits = (int)Application["TotalVisits"];
            lblTotalVisits.Text = visits.ToString();

            string xmlPath = Server.MapPath("~/App_Data/Member.xml");
            if (System.IO.File.Exists(xmlPath))
            {
                XDocument doc = XDocument.Load(xmlPath);
                lblTotalMembers.Text = doc.Root.Elements("Member").Count().ToString();
            }
            else
            {
                lblTotalMembers.Text = "0";
            }
        }

        // Read all members from XML and bind to GridView
        private void LoadMembers()
        {
            string xmlPath = Server.MapPath("~/App_Data/Member.xml");
            if (!System.IO.File.Exists(xmlPath)) return;

            XDocument doc = XDocument.Load(xmlPath);

            var dt = new DataTable();
            dt.Columns.Add("Username");
            dt.Columns.Add("RegisteredDate");
            dt.Columns.Add("SavedZipCount");

            foreach (XElement m in doc.Root.Elements("Member"))
            {
                string username = m.Element("Username")?.Value;
                string regRaw = m.Element("RegisteredDate")?.Value;

                string regFormatted = "-";
                if (!string.IsNullOrEmpty(regRaw))
                {
                    try
                    {
                        regFormatted = DateTime.Parse(regRaw).ToString("MM/dd/yyyy");
                    }
                    catch
                    {
                        regFormatted = regRaw;
                    }
                }

                XElement savedZipsEl = m.Element("SavedZips");
                int zipCount = savedZipsEl != null ? savedZipsEl.Elements("Zip").Count() : 0;

                dt.Rows.Add(username, regFormatted, zipCount.ToString());
            }

            gvMembers.DataSource = dt;
            gvMembers.DataBind();
        }

        protected void btnStaffLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}
