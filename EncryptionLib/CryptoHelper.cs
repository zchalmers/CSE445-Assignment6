// EncryptionLib/CryptoHelper.cs
// C# Class Library — compiled to EncryptionLib.dll and placed in the web app's bin/ folder.
//
// HOW TO SET UP IN VISUAL STUDIO:
//   1. Solution Explorer → right-click Solution → Add → New Project
//   2. Choose "Class Library (.NET Framework)" → name it "EncryptionLib" → Framework 4.0
//   3. Delete the default Class1.cs, paste this file in as CryptoHelper.cs
//   4. Build EncryptionLib project (Ctrl+Shift+B)
//   5. In NationalParksExplorer project: right-click References → Add Reference
//      → Projects tab → check EncryptionLib → OK
//
// IMPORTANT: All computation is LOCAL.
// Passwords are hashed here before writing to XML files.
// Never calls a web service — never sends data over the network.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionLib
{
    /// <summary>
    /// CryptoHelper: SHA-256 hashing and AES-256-CBC encryption/decryption.
    /// </summary>
    public class CryptoHelper
    {
        // AES-256 requires a 32-byte key and 16-byte IV.
        private static readonly byte[] AesKey = Encoding.UTF8.GetBytes("NatParks2026Key!ABCDEFGHIJKLMNOP"); // 32 bytes
        private static readonly byte[] AesIV  = Encoding.UTF8.GetBytes("NatParksIV!16byt");                // 16 bytes

        /// <summary>
        /// Hash: SHA-256 hash of a plaintext string.
        /// Input:  plaintext string (e.g. a password)
        /// Output: lowercase hex string, 64 chars — one-way, cannot be reversed
        /// </summary>
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

        /// <summary>
        /// VerifyHash: checks if plaintext matches a stored SHA-256 hash.
        /// Input:  plaintext (string), storedHash (string)
        /// Output: true if Hash(plaintext) == storedHash (case-insensitive)
        /// </summary>
        public static bool VerifyHash(string plaintext, string storedHash)
        {
            return string.Equals(Hash(plaintext), storedHash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Encrypt: AES-256-CBC encryption.
        /// Input:  plaintext string
        /// Output: Base64-encoded ciphertext string
        /// </summary>
        public static string Encrypt(string plaintext)
        {
            if (plaintext == null) throw new ArgumentNullException("plaintext");
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = AesKey; aes.IV = AesIV;
                aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7;
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

        /// <summary>
        /// Decrypt: reverses Encrypt.
        /// Input:  Base64 ciphertext (output of Encrypt)
        /// Output: original plaintext string
        /// </summary>
        public static string Decrypt(string cipherBase64)
        {
            if (cipherBase64 == null) throw new ArgumentNullException("cipherBase64");
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = AesKey; aes.IV = AesIV;
                aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7;
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
