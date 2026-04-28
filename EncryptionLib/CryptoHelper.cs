// Local hashing and encryption helpers used by the web application.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionLib
{
    public class CryptoHelper
    {
        // AES-256 requires a 32-byte key and 16-byte IV.
        private static readonly byte[] AesKey = Encoding.UTF8.GetBytes("Weather2026Key!!ABCDEFGHIJKLMNOP");
        private static readonly byte[] AesIV = Encoding.UTF8.GetBytes("WeatherIV!16byte");

        public static string Hash(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder(64);
                foreach (byte b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static bool VerifyHash(string plaintext, string storedHash)
        {
            return string.Equals(Hash(plaintext), storedHash, StringComparison.OrdinalIgnoreCase);
        }

        public static string Encrypt(string plaintext)
        {
            if (plaintext == null) throw new ArgumentNullException("plaintext");
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = AesKey;
                aes.IV = AesIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform enc = aes.CreateEncryptor())
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(plaintext);
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherBase64)
        {
            if (cipherBase64 == null) throw new ArgumentNullException("cipherBase64");
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = AesKey;
                aes.IV = AesIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                byte[] cipherBytes = Convert.FromBase64String(cipherBase64);
                using (ICryptoTransform dec = aes.CreateDecryptor())
                using (MemoryStream ms = new MemoryStream(cipherBytes))
                using (CryptoStream cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                    return sr.ReadToEnd();
            }
        }
    }
}
