using System;
using System.Web.UI;

// CaptchaControl.ascx.cs
// Generates a random addition problem, stores the answer in Session,
// and exposes Validate() for parent pages to call on submit.
namespace Assignment6
{
    public partial class CaptchaControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Generate a new question only on the first load, not on postback.
            // The correct answer lives in Session so it survives the round-trip.
            if (!IsPostBack) GenerateQuestion();
        }

        // GenerateQuestion: picks two random integers 1-15, displays them, stores sum.
        private void GenerateQuestion()
        {
            Random rnd = new Random();
            int a = rnd.Next(1, 16);
            int b = rnd.Next(1, 16);
            Session["CaptchaAnswer"] = a + b;
            lblQuestion.Text = a + " + " + b + " = ?";
        }

        // Validate: called by parent page on button click.
        // Returns true if user's answer matches Session-stored correct answer.
        // Regenerates the question on failure.
        public bool Validate()
        {
            int correct = Session["CaptchaAnswer"] != null ? (int)Session["CaptchaAnswer"] : -999;
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
