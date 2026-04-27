using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ZipUtilities
{
    public static class ZipValidationHelper
    {
        // SHA-256 hash for password storage
        public static string HashPassword(string password)
        {
            if (password == null) password = string.Empty;
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sb = new StringBuilder(64);
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static ZipValidationResult ValidateZipInput(string input)
        {
            string originalInput = input ?? string.Empty;
            string digitsOnly = new string(originalInput.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(originalInput))
            {
                return new ZipValidationResult
                {
                    OriginalInput = originalInput,
                    NormalizedZip = string.Empty,
                    IsValid = false,
                    Message = "ZIP code is required."
                };
            }

            if (digitsOnly.Length < 5)
            {
                return new ZipValidationResult
                {
                    OriginalInput = originalInput,
                    NormalizedZip = digitsOnly,
                    IsValid = false,
                    Message = "ZIP code must contain at least 5 digits."
                };
            }

            string normalizedZip = digitsOnly.Substring(0, 5);
            string message = "ZIP code is valid.";

            if (digitsOnly.Length > 5)
            {
                message = "ZIP code was normalized to the first 5 digits.";
            }
            else if (originalInput != normalizedZip)
            {
                message = "ZIP code was normalized before validation.";
            }

            return new ZipValidationResult
            {
                OriginalInput = originalInput,
                NormalizedZip = normalizedZip,
                IsValid = true,
                Message = message
            };
        }
    }
}
