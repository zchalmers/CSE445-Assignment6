using System;
using System.Web.UI;

namespace Assignment6
{
    public partial class CaptchaControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) GenerateQuestion();
        }

        private void GenerateQuestion()
        {
            Random rnd = new Random();
            int a = rnd.Next(1, 16);
            int b = rnd.Next(1, 16);
            Session["CaptchaAnswer"] = a + b;
            lblQuestion.Text = a + " + " + b + " = ?";
        }

        public bool Validate()
        {
            int correct = Session["CaptchaAnswer"] != null ? (int)Session["CaptchaAnswer"] : 0;
            int userAnswer;

            if (!int.TryParse(txtAnswer.Text.Trim(), out userAnswer))
            {
                lblCaptchaMsg.Text      = "Enter a number.";
                lblCaptchaMsg.ForeColor = System.Drawing.Color.Red;
                GenerateQuestion();
                return false;
            }

            if (userAnswer == correct)
            {
                lblCaptchaMsg.ForeColor = System.Drawing.Color.Green;
                return true;
            }
            else
            {
                lblCaptchaMsg.Text      = "Wrong - try the new question.";
                lblCaptchaMsg.ForeColor = System.Drawing.Color.Red;
                GenerateQuestion();
                return false;
            }
        }
    }
}
