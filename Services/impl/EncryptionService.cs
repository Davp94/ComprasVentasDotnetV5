using System;
using System.Security.Cryptography;
using System.Text;

namespace ComprasVentas.Services.impl;

public class EncryptionService(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;


    public string Encrypt(string plainText)
    {
        var aes = Aes.Create(); //123456
        aes.Key = Encoding.UTF8.GetBytes("1232132132131232131"); 
        aes.IV = Encoding.UTF8.GetBytes("3213123");

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        var ms = new MemoryStream();
        var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        var sw = new StreamWriter(cs);
        sw.Write(plainText);

        return Convert.ToBase64String(ms.ToArray());
    }

     public string Decrypt(string cipherText)
    {
        var aes = Aes.Create(); //123456
        aes.Key = Encoding.UTF8.GetBytes("1232132132131232131"); 
        aes.IV = Encoding.UTF8.GetBytes("3213123");

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        var ms = new MemoryStream(Convert.FromBase64String(cipherText));
        var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
