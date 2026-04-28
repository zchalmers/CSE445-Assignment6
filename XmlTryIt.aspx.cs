using System;

namespace Assignment6
{
    public partial class XmlTryIt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblSessionCache.Text = Session["LastJsonResult"] != null && Session["LastJsonResult"].ToString() != ""
                ? "Last conversion cached for this session (" + Session["LastJsonResult"].ToString().Length + " chars)."
                : "(nothing cached yet)";
        }

        protected void btnVerifyValid_Click(object sender, EventArgs e)
        {
            string result = XmlVerification.Verify(XmlVerification.XML_VALID, XmlVerification.XSD_URL);
            lblVerifyValid.Text = "XML: " + XmlVerification.XML_VALID + "\nXSD: " + XmlVerification.XSD_URL
                + "\n\nResult: " + result;
        }

        protected void btnVerifyError_Click(object sender, EventArgs e)
        {
            string result = XmlVerification.Verify(XmlVerification.XML_ERROR, XmlVerification.XSD_URL);
            lblVerifyError.Text = "XML: " + XmlVerification.XML_ERROR + "\nXSD: " + XmlVerification.XSD_URL
                + "\n\nResult: " + result;
        }

        protected void btnConvertValid_Click(object sender, EventArgs e)
        {
            string json = XmlVerification.Xml2Json(XmlVerification.XML_VALID);
            Session["LastJsonResult"] = json;
            lblJsonValid.Text = json;
            lblSessionCache.Text = "Cached " + json.Length + " chars for this session.";
        }

        protected void btnConvertCustom_Click(object sender, EventArgs e)
        {
            string url = txtCustomUrl.Text.Trim();
            if (string.IsNullOrEmpty(url)) { lblJsonCustom.Text = "Enter a URL."; return; }
            lblJsonCustom.Text = XmlVerification.Xml2Json(url);
        }
    }
}
