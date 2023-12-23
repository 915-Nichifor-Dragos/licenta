using Konscious.Security.Cryptography;
using System.Text;

namespace HotelManagement.BusinessLogic.Converters;

public static class PasswordEncrypter
{
    public static string Hash(string password)
    {
        var hasher = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Iterations = 1,
            MemorySize = 32,
            DegreeOfParallelism = 1
        };

        byte[] hashedBytes = hasher.GetBytes(64);

        return Convert.ToBase64String(hashedBytes);
    }
}