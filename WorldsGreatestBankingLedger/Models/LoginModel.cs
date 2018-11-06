
using System;
using System.Security.Cryptography;
using System.Text;
using Models.Interfaces;

namespace Models
{
    public class LoginModel
    {
        private const string Hyphen = "-"; 
        private readonly IDataService _ds;

        public LoginModel(IDataService ds)
        {
            _ds = ds;
        }

        public bool VerifyCredentials(string name, string passwordAttempt)
        {
            return _ds.VerifyUserCredentials(name, GetStringHash(passwordAttempt));
        }

        public bool RegisterUser(string name, string password)
        {
            return _ds.AddUser(name, GetStringHash(password));
        }

        private static string GetStringHash(string str)
        {
            using (var sha256 = SHA256.Create())
            {
                var byteArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                return BitConverter.ToString(byteArray).Replace(Hyphen, string.Empty).ToLower();
            }
        }
    }
}
