using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace WebDPRCalc.Models
{
    public class User
    {
        public string username { get; set; }
        public byte[] password { get; set; }
        public List<Attack> attacks { get; set; }
        public bool validPassword(string password)
        {
            byte[] hashedPassword = hashPassword(password);
            if (hashedPassword == this.password)
            {
                return true;
            }
            return false;
        }

        public static byte[] hashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);


            return hashed;
        }
    }
}
