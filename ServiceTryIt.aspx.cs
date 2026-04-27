using System;
using System.Net;
using System.Web;

// TryItService.aspx.cs
// Demonstrates all services: Assignment 4 (XmlVerification) and Assignment 3 (WCF + REST).
// The WCF calls go directly to the local Service1 class.
// The REST call uses WebClient HTTP GET.

namespace Assignment6
{
    public partial class ServiceTryIt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        // -- Assignment 3: WebDownload via local WCF service class --
        protected void btnWebDownload_Click(object sender, EventArgs e)
        {
            string url = txtDlUrl.Text.Trim();
            if (string.IsNullOrEmpty(url)) { lblWebDownload.Text = "Enter a URL."; return; }
            try
            {
                var svc = new Service1();
                string content = svc.WebDownload(url);
                if (content.Length > 5000)
                    content = content.Substring(0, 5000) + "\n... [TRUNCATED - " + content.Length + " chars]";
                lblWebDownload.Text = HttpUtility.HtmlEncode(content);
            }
            catch (Exception ex) { lblWebDownload.Text = "Error: " + ex.Message; }
        }

        // -- Assignment 3: WordFilter via local WCF service class --
        protected void btnWordFilter_Click(object sender, EventArgs e)
        {
            string text = txtFilterText.Text;
            if (string.IsNullOrEmpty(text)) { lblWordFilter.Text = "Enter text."; return; }
            try
            {
                var svc = new Service1();
                string filtered = svc.WordFilter(text);
                lblWordFilter.Text = HttpUtility.HtmlEncode(filtered);
            }
            catch (Exception ex) { lblWordFilter.Text = "Error: " + ex.Message; }
        }

        // -- Assignment 3: NaturalHazards via REST GET --
        protected void btnHazards_Click(object sender, EventArgs e)
        {
            double lat, lng;
            if (!double.TryParse(txtLat.Text.Trim(), out lat)) { lblHazards.Text = "Invalid latitude."; return; }
            if (!double.TryParse(txtLng.Text.Trim(), out lng)) { lblHazards.Text = "Invalid longitude."; return; }
            try
            {
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath.TrimEnd('/');
                string apiUrl = baseUrl + "/api/NaturalHazards/GetHazards?lat=" + lat + "&lng=" + lng;
                string response = new WebClient().DownloadString(apiUrl);
                response = response.Trim('"').Replace("\\r\\n", "\n").Replace("\\n", "\n");
                lblHazards.Text = HttpUtility.HtmlEncode(response);
            }
            catch (Exception ex) { lblHazards.Text = "Error: " + ex.Message; }
        }
    }
}
