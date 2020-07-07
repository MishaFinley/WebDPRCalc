using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDPRCalc.Models
{
    public class User
    {
        private string username;
        private byte[] password;
        //private Attack[] Attacks;



        public bool validPassword(string password)
        {
            return false;
        }

        public byte[] hashPassword(string password)
        {
            return null;
        }
    }
}
