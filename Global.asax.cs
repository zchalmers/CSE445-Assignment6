using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Linq;
using ZipUtilities;

namespace Assignment6
{
    public class Global_asax : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["TotalVisits"] = 0;
            SeedStaffXml();
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Increment visit counter
            lock (Application)
            {
                int visits = (int)Application["TotalVisits"];
                Application["TotalVisits"] = visits + 1;
            }
        }

        // Make sure Staff.xml has the TA grader account
        private void SeedStaffXml()
        {
            string staffPath = HttpContext.Current.Server.MapPath("~/App_Data/Staff.xml");

            XDocument doc;
            if (System.IO.File.Exists(staffPath))
            {
                doc = XDocument.Load(staffPath);
            }
            else
            {
                doc = new XDocument(new XElement("Staff"));
            }

            bool taExists = doc.Root.Elements("Member").Any(
                m => m.Element("Username")?.Value == "TA");

            if (!taExists)
            {
                string taHash = ZipValidationHelper.HashPassword("Cse445!");
                doc.Root.Add(new XElement("Member",
                    new XElement("Username", "TA"),
                    new XElement("PasswordHash", taHash)
                ));
                doc.Save(staffPath);
            }
        }
    }
}
