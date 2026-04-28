using System;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.Web;

namespace Assignment6
{
    public partial class ServiceTryIt : System.Web.UI.Page
    {
        private static readonly string Service1Url =
            ConfigurationManager.AppSettings["Service1BaseUrl"];

        private static readonly string NaturalHazardsBaseUrl =
            ConfigurationManager.AppSettings["NaturalHazardsBaseUrl"];

        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnWebDownload_Click(object sender, EventArgs e)
        {
            string url = txtDlUrl.Text.Trim();
            if (string.IsNullOrEmpty(url)) { lblWebDownload.Text = "Enter a URL."; return; }
            try
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(Service1Url);
                using (var factory = new ChannelFactory<IService1>(binding, endpoint))
                {
                    IService1 svc = factory.CreateChannel();
                    string content = svc.WebDownload(url);
                    if (content.Length > 5000)
                        content = content.Substring(0, 5000) + "\n... [TRUNCATED - " + content.Length + " chars]";
                    lblWebDownload.Text = HttpUtility.HtmlEncode(content);
                }
            }
            catch (Exception ex) { lblWebDownload.Text = "Error: " + ex.Message; }
        }

        protected void btnWordFilter_Click(object sender, EventArgs e)
        {
            string text = txtFilterText.Text;
            if (string.IsNullOrEmpty(text)) { lblWordFilter.Text = "Enter text."; return; }
            try
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(Service1Url);
                using (var factory = new ChannelFactory<IService1>(binding, endpoint))
                {
                    IService1 svc = factory.CreateChannel();
                    string filtered = svc.WordFilter(text);
                    lblWordFilter.Text = HttpUtility.HtmlEncode(filtered);
                }
            }
            catch (Exception ex) { lblWordFilter.Text = "Error: " + ex.Message; }
        }

        protected void btnHazards_Click(object sender, EventArgs e)
        {
            double lat, lng;
            if (!double.TryParse(txtLat.Text.Trim(), out lat)) { lblHazards.Text = "Invalid latitude."; return; }
            if (!double.TryParse(txtLng.Text.Trim(), out lng)) { lblHazards.Text = "Invalid longitude."; return; }
            try
            {
                string apiUrl = NaturalHazardsBaseUrl + "/api/NaturalHazards/GetHazards?lat=" + lat + "&lng=" + lng;
                string response = new WebClient().DownloadString(apiUrl);
                response = response.Trim('"').Replace("\\r\\n", "\n").Replace("\\n", "\n");
                lblHazards.Text = HttpUtility.HtmlEncode(response);
            }
            catch (Exception ex) { lblHazards.Text = "Error: " + ex.Message; }
        }
    }
}
