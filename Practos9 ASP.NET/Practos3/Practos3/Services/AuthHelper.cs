using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Practos3.Services;

public static class AuthHelper
{
    public static string HashPassword(string password)
    {
        var salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, 32);
        var saltedHash = new byte[1 + 16 + 32];
        saltedHash[0] = 0x01;
        Buffer.BlockCopy(salt, 0, saltedHash, 1, 16);
        Buffer.BlockCopy(subkey, 0, saltedHash, 17, 32);
        return Convert.ToBase64String(saltedHash);
    }

    public static bool VerifyPassword(string password, string hashed)
    {
        try
        {
            var bytes = Convert.FromBase64String(hashed);
            if (bytes.Length != 1 + 16 + 32 || bytes[0] != 0x01) return false;
            var salt = new byte[16];
            Buffer.BlockCopy(bytes, 1, salt, 0, 16);
            var storedSubkey = new byte[32];
            Buffer.BlockCopy(bytes, 17, storedSubkey, 0, 32);
            var generated = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, 32);
            return CryptographicOperations.FixedTimeEquals(generated, storedSubkey);
        }
        catch
        {
            return false;
        }
    }
}
