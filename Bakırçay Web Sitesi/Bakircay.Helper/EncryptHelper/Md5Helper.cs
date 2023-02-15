using System.Security.Cryptography;
using System.Text;

namespace Bakircay.Helper.EncryptHelper;

public class Md5Helper
{
  static string Key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";
  private static readonly Random Random = new Random();

  public static string RandomString()
  {
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    return new string(Enumerable.Repeat(chars, 5)
      .Select(s => s[Random.Next(s.Length)]).ToArray());
  }

  public static string Encrypt(string text)
  {
    text = $"{RandomString()}{text}";

    using var md5 = MD5.Create();
    using var tdes = TripleDES.Create();
    tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(Key));
    tdes.Mode = CipherMode.ECB;
    tdes.Padding = PaddingMode.PKCS7;

    using var transform = tdes.CreateEncryptor();
    byte[] textBytes = Encoding.UTF8.GetBytes(text);
    byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
    return Convert.ToBase64String(bytes, 0, bytes.Length);
  }

  public static string Decrypt(string cipher)
  {
    using var md5 = MD5.Create();
    using var tdes = TripleDES.Create();
    tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(Key));
    tdes.Mode = CipherMode.ECB;
    tdes.Padding = PaddingMode.PKCS7;

    using var transform = tdes.CreateDecryptor();
    byte[] cipherBytes = Convert.FromBase64String(cipher);
    byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
    return Encoding.UTF8.GetString(bytes).Remove(0, 5);
  }

  public static string EncryptPassword(string text)
  {
    using var md5 = MD5.Create();
    using var tdes = TripleDES.Create();
    tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(Key));
    tdes.Mode = CipherMode.ECB;
    tdes.Padding = PaddingMode.PKCS7;

    using var transform = tdes.CreateEncryptor();
    byte[] textBytes = Encoding.UTF8.GetBytes(text);
    byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
    return Convert.ToBase64String(bytes, 0, bytes.Length);
  }

  public static string DecryptPassword(string cipher)
  {
    using var md5 = MD5.Create();
    using var tdes = TripleDES.Create();
    tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(Key));
    tdes.Mode = CipherMode.ECB;
    tdes.Padding = PaddingMode.PKCS7;

    using var transform = tdes.CreateDecryptor();
    byte[] cipherBytes = Convert.FromBase64String(cipher);
    byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
    return Encoding.UTF8.GetString(bytes);
  }

  public static string EncryptConnectionString(string source)
  {
    using var tripleDesCryptoService = TripleDES.Create();
    using var hashMd5Provider = MD5.Create();
    byte[] byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(Key));
    tripleDesCryptoService.Key = byteHash;
    tripleDesCryptoService.Mode = CipherMode.ECB;
    byte[] data = Encoding.UTF8.GetBytes(source);

    return Convert.ToBase64String(tripleDesCryptoService.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
  }
  public static string DecryptConnectionString(string encrypt)
  {
    using var tripleDesCryptoService = TripleDES.Create();
    using var hashMd5Provider = MD5.Create();
    byte[] byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(Key));
    tripleDesCryptoService.Key = byteHash;
    tripleDesCryptoService.Mode = CipherMode.ECB;
    byte[] data = Convert.FromBase64String(encrypt);
    return Encoding.UTF8.GetString(tripleDesCryptoService.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
  }
}