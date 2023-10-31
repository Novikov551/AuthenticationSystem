using AuthenticationSystem.Domain.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationSystem.Domain.Services
{
    public static class HashGenerator
    {
        public static string GetHash(string param, string salt = null)
        {
            var saltStr = salt.IsNullOrEmpty() ? param : param += salt;
            using var hash = SHA256.Create();
            var byteArray = hash.ComputeHash(Encoding.UTF8.GetBytes(saltStr));

            return Convert.ToHexString(byteArray).ToLower();
        }
    }
}
