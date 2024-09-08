using Catalogue.Application.Resources;
using System.Security.Cryptography;
using System.Text;

namespace Catalogue.Application.Utils;

public static class Crypto 
{
    private static readonly byte[]? _key;
    private static readonly byte[]? _iv;

    static Crypto()
    {
        string? keyString = Environment.GetEnvironmentVariable("ENCRYPTION_KEY");
        string? ivString = Environment.GetEnvironmentVariable("ENCRYPTION_IV");

        if (string.IsNullOrEmpty(keyString) || string.IsNullOrEmpty(ivString))
        {
            throw new InvalidOperationException(ErrorMessagesResource.ENV_VARIABLES_NULL);
        }

        _key = Encoding.UTF8.GetBytes(keyString);
        _iv = Encoding.UTF8.GetBytes(ivString);

        if (_key.Length != 32 || _iv.Length != 16)
        {
            throw new InvalidOperationException(ErrorMessagesResource.ENV_VARIABLE_INVALID);
        }
    }

    public static string Encrypt(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _key!;
            aesAlg.IV = _iv!;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream ms = new MemoryStream()) 
            {
                using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using(StreamWriter sw = new StreamWriter(cs)) 
                    {
                        sw.Write(plainText);
                    }
                }

                return Convert.ToBase64String(ms.ToArray());
            }
         }
    }

    public static string Decrypt(string cipherText)
    {
        using (Aes aesAlg = Aes.Create()) 
        {
            aesAlg.Key = _key!;
            aesAlg.IV = _iv!; 

            ICryptoTransform encryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
