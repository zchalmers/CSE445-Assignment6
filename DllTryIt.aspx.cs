using System;
using System.Web.UI;
using ZipUtilities;

namespace Assignment6
{
    public partial class DllTryIt : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtZipInput.Text = "85281-1234";
            }
        }

        protected void btnValidateZip_Click(object sender, EventArgs e)
        {
            var result = ZipValidationHelper.ValidateZipInput(txtZipInput.Text);

            lblNormalizedZip.Text = string.IsNullOrWhiteSpace(result.NormalizedZip) ? "(empty)" : result.NormalizedZip;
            lblValidationState.Text = result.IsValid ? "Valid" : "Invalid";
            lblValidationMessage.Text = result.Message;
        }
    }
}
