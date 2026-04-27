using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using ZipUtilities;

namespace Assignment6
{
    public partial class WeatherTryIt : Page
    {
        private const string LastZipCookieName = "LastZip";
        private static readonly string WeatherServiceBaseUrl =
            ConfigurationManager.AppSettings["WeatherServiceBaseUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var savedZipCookie = Request.Cookies[LastZipCookieName];
                if (savedZipCookie != null && !string.IsNullOrWhiteSpace(savedZipCookie.Value))
                {
                    txtWeatherZip.Text = savedZipCookie.Value;
                    lblLastZipCookie.Text = "Last saved ZIP: " + savedZipCookie.Value;
                }
            }
        }

        protected void btnGetForecast_Click(object sender, EventArgs e)
        {
            bltForecastResults.Items.Clear();

            var validation = ZipValidationHelper.ValidateZipInput(txtWeatherZip.Text);
            lblWeatherNormalizedZip.Text = string.IsNullOrWhiteSpace(validation.NormalizedZip) ? "(empty)" : validation.NormalizedZip;

            if (!validation.IsValid)
            {
                lblWeatherStatus.Text = validation.Message;
                return;
            }

            string baseUrl = (WeatherServiceBaseUrl ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                lblWeatherStatus.Text = "Weather service URL is not configured";
                return;
            }

            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            string requestUrl = baseUrl + "Weather5day?zipcode=" + Server.UrlEncode(validation.NormalizedZip);

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    string responseText = client.DownloadString(requestUrl);
                    var forecastLines = JsonConvert.DeserializeObject<List<string>>(responseText);

                    if (forecastLines == null || forecastLines.Count == 0)
                    {
                        lblWeatherStatus.Text = "No data";
                        return;
                    }

                    foreach (string line in forecastLines)
                    {
                        bltForecastResults.Items.Add(line);
                    }
                }

                SaveLastZip(validation.NormalizedZip);
                lblWeatherStatus.Text = "Forecast loaded successfully.";
                lblLastZipCookie.Text = "Last saved ZIP: " + validation.NormalizedZip;
            }
            catch (Exception)
            {
                lblWeatherStatus.Text = "Could not reach the weather service.";
            }
        }

        private void SaveLastZip(string zipCode)
        {
            var zipCookie = new HttpCookie(LastZipCookieName, zipCode);
            zipCookie.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(zipCookie);
        }
    }
}
