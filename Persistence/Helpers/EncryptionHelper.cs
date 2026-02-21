using System.Security.Cryptography;
using System.Text;

namespace Persistence.Helpers
{
    public static class EncryptionHelper
    {
        private static readonly string Key = "1234567890123456";
        private static readonly string IV = "1234567890123456";

        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            var encryptor = aes.CreateEncryptor();
            var buffer = Encoding.UTF8.GetBytes(plainText);

            return Convert.ToBase64String(
                encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public static string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            var decryptor = aes.CreateDecryptor();
            var buffer = Convert.FromBase64String(cipherText);

            return Encoding.UTF8.GetString(
                decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }
    }
}
