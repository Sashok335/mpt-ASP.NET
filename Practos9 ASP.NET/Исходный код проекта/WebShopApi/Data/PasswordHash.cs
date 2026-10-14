using System.Security.Cryptography;

namespace WebShopApi.Data;

internal static class PasswordHash
{
    // The version byte, salt and PBKDF2 parameters match AuthHelper in the MVC project.
    public static string Create(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, 10_000, HashAlgorithmName.SHA256, 32);

        var value = new byte[1 + salt.Length + key.Length];
        value[0] = 1;
        salt.CopyTo(value, 1);
        key.CopyTo(value, 1 + salt.Length);

        return Convert.ToBase64String(value);
    }
}
