using System;

namespace Assignment6
{
    public partial class EncryptionTryIt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnHash_Click(object sender, EventArgs e)
        {
            string input = txtHashInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) { lblHashResult.Text = "Enter text."; return; }
            string hash = EncryptionLib.CryptoHelper.Hash(input);
            lblHashResult.Text = "Input   : " + input + "\nSHA-256 : " + hash + "\nLength  : " + hash.Length + " chars";
        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            string input = txtEncInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) { lblEncResult.Text = "Enter text."; return; }
            string cipher = EncryptionLib.CryptoHelper.Encrypt(input);
            Session["LastCipher"] = cipher;
            Session["LastPlain"]  = input;
            lblEncResult.Text = "Plaintext : " + input + "\nEncrypted : " + cipher + "\n\n(Click 'Decrypt Last' to reverse)";
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (Session["LastCipher"] == null) { lblEncResult.Text = "Run Encrypt first."; return; }
            string decrypted = EncryptionLib.CryptoHelper.Decrypt(Session["LastCipher"].ToString());
            lblEncResult.Text = "Cipher    : " + Session["LastCipher"]
                + "\nDecrypted : " + decrypted
                + "\nOriginal  : " + Session["LastPlain"]
                + "\nMatch?    : " + (decrypted == Session["LastPlain"].ToString() ? "Yes" : "No");
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string plain = txtVerifyPlain.Text.Trim();
            string hash  = txtVerifyHash.Text.Trim();
            if (string.IsNullOrEmpty(plain) || string.IsNullOrEmpty(hash)) { lblVerifyResult.Text = "Enter both fields."; return; }
            bool match    = EncryptionLib.CryptoHelper.VerifyHash(plain, hash);
            string computed = EncryptionLib.CryptoHelper.Hash(plain);
            lblVerifyResult.Text = "Plaintext  : " + plain
                + "\nStored hash: " + hash
                + "\nComputed   : " + computed
                + "\nMatch?     : " + (match ? "YES - correct password" : "NO - does not match");
        }
    }
}
