using System.Security.Cryptography;
using System.Text;

namespace TransportationAttendance.Infrastructure.Identity;

public static class MD5PasswordHasher
{
    public static string GetMD5Hash(string input)
    {
        var enc = new UTF8Encoding();
        using var md5Hasher = MD5.Create();
        var hashBytes = md5Hasher.ComputeHash(enc.GetBytes(input));
        return enc.GetString(hashBytes);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var computedHash = GetMD5Hash(password);
        return string.Equals(computedHash, hashedPassword, StringComparison.Ordinal);
    }
}
