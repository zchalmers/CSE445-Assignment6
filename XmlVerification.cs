using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace Assignment6
{
    public static class XmlVerification
    {
        public const string XML_VALID = "https://asuandrew.github.io/cse445-assignment4/NationalParks.xml";
        public const string XML_ERROR = "https://asuandrew.github.io/cse445-assignment4/NationalParksErrors.xml";
        public const string XSD_URL   = "https://asuandrew.github.io/cse445-assignment4/NationalParks.xsd";

        public static string Verify(string xmlUrl, string xsdUrl)
        {
            try
            {
                string xsdContent = Download(xsdUrl);
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(null, XmlReader.Create(new StringReader(xsdContent)));

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas        = schemas;

                string errors = "";
                settings.ValidationEventHandler += (s, e) => { errors += e.Message + "\n"; };

                string xmlContent = Download(xmlUrl);
                XmlReader reader  = XmlReader.Create(new StringReader(xmlContent), settings);
                while (reader.Read()) { }

                return string.IsNullOrEmpty(errors) ? "No errors are found" : errors.Trim();
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Download(xmlUrl));
                XmlElement root = doc.DocumentElement;
                root.RemoveAttribute("xmlns:xsi");
                root.RemoveAttribute("xsi:noNamespaceSchemaLocation");
                return JsonConvert.SerializeXmlNode(root, Newtonsoft.Json.Formatting.Indented, false);
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        private static string Download(string url)
        {
            using (var wc = new System.Net.WebClient())
                return wc.DownloadString(url);
        }
    }
}
