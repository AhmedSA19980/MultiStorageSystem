using System.Security.Cryptography;
using System.Text;

namespace BlobStorage.Providers.S3.AWS
{
    public class AwsV4Signer
    {
        public string Sign (string key , string message)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(message))).Replace("-","").ToLower();

        }

        public static byte[] HmacSHA256(byte[] key , string data)
        {

            using var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public static string HashHex(byte[] data)
        {
            using var sha256 = SHA256.Create();
            return BitConverter.ToString(sha256.ComputeHash(data)).Replace("-", "").ToLower();
        }
    }

    
}
