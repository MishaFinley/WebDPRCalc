using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebDPRCalc.Models
{
    public class User
    {
        private string username;
        private byte[] password;
        private Attack[] Attacks;



        public bool validPassword(string password)
        {
            byte[] hashedPassword = hashPassword(password);
            if(hashedPassword == this.password)
            {
                return true;
            }
            return false;
        }

        public byte[] hashPassword(string password)
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
