using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WebDPRCalc.Models
{
    public class User
    {
        public string username { get; set; }
        public byte[] password { get; set; }
        public List<Attack> attacks { get; set; }
        public bool validPassword(string password, string username)
        {
            byte[] hashedPassword = hashPassword(password, username);
            return hashedPassword.SequenceEqual(this.password);
        }

        public static byte[] hashPassword(string password, string username)
        {
            byte[] salt = Encoding.UTF8.GetBytes(username);
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
