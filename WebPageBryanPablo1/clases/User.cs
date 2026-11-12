using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebPageBryanPablo1.clases
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                string patron = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9\s])\S{8,}$";

                if (Regex.IsMatch(value ?? "", patron))
                {
                    password = value;
                }
                else
                {
                    throw new System.ArgumentException(
                        "La contraseña no cumple los requisitos de seguridad.");
                }
            }
        }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
    }
}