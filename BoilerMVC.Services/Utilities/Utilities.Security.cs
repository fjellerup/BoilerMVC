using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BoilerMVC.Services
{
    public partial class Utilities
    {
        public static class Security
        {
            public static string CreateRandomPassword(int passwordLength)
            {
                string allowedChars = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789!@$?_-";
                char[] characters = new char[passwordLength];
                Random random = new Random();

                for (int i = 0; i < passwordLength; i++)
                    characters[i] = allowedChars[random.Next(0, allowedChars.Length)];

                string password = new string(characters);
                return password;
            }

            public static string CreateSalt()
            {
                string salt = Guid.NewGuid().ToString() + DateTime.UtcNow.ToString() + Guid.NewGuid().ToString();
                return salt;
            }

            public static string HashString(string value)
            {
                SHA512CryptoServiceProvider crypto = new SHA512CryptoServiceProvider();
                byte[] data = Encoding.UTF8.GetBytes(value);
                byte[] hashedData = crypto.ComputeHash(data);
                string hashedString = BitConverter.ToString(hashedData);
                return hashedString.Replace("-", "").ToLower();
            }

            public static string HashPassword(string password, string salt)
            {
                string saltedPassword = password + salt;
                return HashString(saltedPassword);
            }
        }
    }
}