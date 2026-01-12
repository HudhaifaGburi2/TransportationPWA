using System.Security.Cryptography;
using System.Text;

namespace TransportationAttendance.Infrastructure.Identity;

public static class MD5PasswordHasher
{
    public static string GetMD5Hash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);
        
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }
        
        return sb.ToString();
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var computedHash = GetMD5Hash(password);
        return string.Equals(computedHash, hashedPassword, StringComparison.OrdinalIgnoreCase);
    }
}
