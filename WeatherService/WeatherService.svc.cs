using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace WeatherService
{
    public class Service1 : IWeatherService
    {
        public string[] Weather5day(string zipcode)
        {
            if (string.IsNullOrWhiteSpace(zipcode))
            {
                return new[] { "Enter a zip code." };
            }

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string zip = new string(zipcode.Where(char.IsDigit).ToArray());
                if (zip.Length < 5)
                {
                    return new[] { "Enter a valid zip code." };
                }

                zip = zip.Substring(0, 5);

                string latLonRequest =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<SOAP-ENV:Envelope SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" " +
                    "xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                    "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                    "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                    "<SOAP-ENV:Body>" +
                    "<ns1:LatLonListZipCode xmlns:ns1=\"uri:DWMLgen\">" +
                    "<zipCodeList xsi:type=\"xsd:string\">" + zip + "</zipCodeList>" +
                    "<XMLformat xsi:type=\"xsd:string\">DWML</XMLformat>" +
                    "<listType xsi:type=\"xsd:string\">zipCode</listType>" +
                    "</ns1:LatLonListZipCode>" +
                    "</SOAP-ENV:Body>" +
                    "</SOAP-ENV:Envelope>";

                string latLonResponse;
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "text/xml; charset=utf-8";
                    latLonResponse = client.UploadString("https://digital.weather.gov/xml/SOAP_server/ndfdXMLserver.php", latLonRequest);
                }

                XDocument latLonSoap = XDocument.Parse(latLonResponse);
                XElement latLonOut = latLonSoap.Descendants().FirstOrDefault(x => x.Name.LocalName == "listLatLonOut");
                if (latLonOut == null || string.IsNullOrWhiteSpace(latLonOut.Value))
                {
                    return new[] { "No data for this zip code." };
                }

                XDocument latLonDoc = XDocument.Parse(latLonOut.Value);
                XElement latLonList = latLonDoc.Descendants().FirstOrDefault(x => x.Name.LocalName == "latLonList");
                if (latLonList == null || string.IsNullOrWhiteSpace(latLonList.Value))
                {
                    return new[] { "No data for this zip code." };
                }

                string[] parts = latLonList.Value.Split(',');
                string latitude = parts[0].Trim();
                string longitude = parts[1].Trim();

                string forecastRequest =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<SOAP-ENV:Envelope SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" " +
                    "xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                    "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                    "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                    "<SOAP-ENV:Body>" +
                    "<ns1:NDFDgenByDay xmlns:ns1=\"uri:DWMLgenByDay\">" +
                    "<latitude xsi:type=\"xsd:string\">" + latitude + "</latitude>" +
                    "<longitude xsi:type=\"xsd:string\">" + longitude + "</longitude>" +
                    "<startDate xsi:type=\"xsd:string\">" + DateTime.Today.ToString("yyyy-MM-dd") + "</startDate>" +
                    "<numDays xsi:type=\"xsd:string\">5</numDays>" +
                    "<Unit xsi:type=\"xsd:string\">e</Unit>" +
                    "<XMLformat xsi:type=\"xsd:string\">DWML</XMLformat>" +
                    "<format xsi:type=\"xsd:string\">24 hourly</format>" +
                    "</ns1:NDFDgenByDay>" +
                    "</SOAP-ENV:Body>" +
                    "</SOAP-ENV:Envelope>";

                string forecastResponse;
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "text/xml; charset=utf-8";
                    forecastResponse = client.UploadString("https://digital.weather.gov/xml/SOAP_server/ndfdXMLserver.php", forecastRequest);
                }

                XDocument forecastSoap = XDocument.Parse(forecastResponse);
                XElement forecastOut = forecastSoap.Descendants().FirstOrDefault(x => x.Name.LocalName == "XMLByDayOut");
                if (forecastOut == null || string.IsNullOrWhiteSpace(forecastOut.Value))
                {
                    return new[] { "Forecast not available." };
                }

                XDocument forecastDoc = XDocument.Parse(forecastOut.Value);

                Dictionary<string, List<DateTimeOffset>> layouts = new Dictionary<string, List<DateTimeOffset>>();
                foreach (XElement layout in forecastDoc.Descendants().Where(x => x.Name.LocalName == "time-layout"))
                {
                    XElement key = layout.Descendants().FirstOrDefault(x => x.Name.LocalName == "layout-key");
                    if (key == null)
                    {
                        continue;
                    }

                    List<DateTimeOffset> times = new List<DateTimeOffset>();
                    foreach (XElement start in layout.Descendants().Where(x => x.Name.LocalName == "start-valid-time"))
                    {
                        DateTimeOffset time;
                        if (DateTimeOffset.TryParse(start.Value, out time))
                        {
                            times.Add(time);
                        }
                    }

                    layouts[key.Value] = times;
                }

                XElement maxNode = forecastDoc.Descendants().FirstOrDefault(x => x.Name.LocalName == "temperature" && (string)x.Attribute("type") == "maximum");
                XElement minNode = forecastDoc.Descendants().FirstOrDefault(x => x.Name.LocalName == "temperature" && (string)x.Attribute("type") == "minimum");
                XElement weatherNode = forecastDoc.Descendants().FirstOrDefault(x => x.Name.LocalName == "weather");
                XElement popNode = forecastDoc.Descendants().FirstOrDefault(x => x.Name.LocalName == "probability-of-precipitation");

                if (maxNode == null || minNode == null || weatherNode == null)
                {
                    return new[] { "Forecast not available." };
                }

                string dayLayout = (string)maxNode.Attribute("time-layout");
                List<DateTimeOffset> days = layouts.ContainsKey(dayLayout) ? layouts[dayLayout] : new List<DateTimeOffset>();
                List<string> highs = maxNode.Descendants().Where(x => x.Name.LocalName == "value").Select(x => x.Value).ToList();
                List<string> lows = minNode.Descendants().Where(x => x.Name.LocalName == "value").Select(x => x.Value).ToList();
                List<string> summaries = weatherNode.Descendants()
                    .Where(x => x.Name.LocalName == "weather-conditions")
                    .Select(x => (string)x.Attribute("weather-summary") ?? "No summary")
                    .ToList();

                List<DateTimeOffset> popTimes = new List<DateTimeOffset>();
                List<string> pops = new List<string>();
                if (popNode != null)
                {
                    string popLayout = (string)popNode.Attribute("time-layout");
                    if (!string.IsNullOrWhiteSpace(popLayout) && layouts.ContainsKey(popLayout))
                    {
                        popTimes = layouts[popLayout];
                    }

                    pops = popNode.Descendants().Where(x => x.Name.LocalName == "value").Select(x => x.Value).ToList();
                }

                List<string> result = new List<string>();
                for (int i = 0; i < days.Count && i < highs.Count && i < lows.Count; i++)
                {
                    int highestPop = 0;

                    for (int j = 0; j < popTimes.Count && j < pops.Count; j++)
                    {
                        DateTimeOffset end = i + 1 < days.Count ? days[i + 1] : days[i].AddDays(1);
                        if (popTimes[j] >= days[i] && popTimes[j] < end)
                        {
                            int currentPop;
                            if (int.TryParse(pops[j], out currentPop) && currentPop > highestPop)
                            {
                                highestPop = currentPop;
                            }
                        }
                    }

                    string summary = i < summaries.Count ? summaries[i] : "No summary";
                    string line =
                        days[i].ToString("ddd, MMM d", CultureInfo.InvariantCulture) +
                        ": " + summary +
                        ", High " + highs[i] + " F" +
                        ", Low " + lows[i] + " F" +
                        ", Precipitation " + highestPop + "%";

                    result.Add(line);
                }

                if (result.Count == 0)
                {
                    return new[] { "Forecast not available." };
                }

                return result.ToArray();
            }
            catch
            {
                return new[] { "Could not get weather data." };
            }
        }
    }
}
