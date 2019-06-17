using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace APITest.Domain.Shared
{
    public static class Auth
    {
        private static readonly int HashIterations = 10000;
        private static readonly int HashedPasswordLength = 20;
        private static readonly int SaltLength = 16;
        public static SaltedPassword GeneratePassword(string password)
        {
            // Create hashed password and salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltLength]);
            var hashedPassword = new Rfc2898DeriveBytes(password, salt, HashIterations).GetBytes(HashedPasswordLength);

            var hashedPasswordString = Convert.ToBase64String(hashedPassword);

            return new SaltedPassword()
            {
                Salt = salt,
                HashedPassword = hashedPasswordString
            };
        }

        public static bool VerifyPassword(string password, byte[] salt, string hashedPassword)
        {
            return Convert.ToBase64String(new Rfc2898DeriveBytes(password, salt, HashIterations).GetBytes(HashedPasswordLength)) == hashedPassword;
        }
    }

    public class SaltedPassword
    {
        public byte[] Salt { get; set; }
        public string HashedPassword { get; set; }
    }
}
