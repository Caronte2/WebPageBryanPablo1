using System;
using System.Text.RegularExpressions;

namespace WebPageBryanPablo1.clases
{
    public class User
    {
        public int UserId { get; set; }

        public string DNI { get; set; }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                string patron = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@€!%*?&#])\S{8,}$";

                if (Regex.IsMatch(value ?? "", patron))
                {
                    password = value;
                }
                else
                {
                    throw new ArgumentException("La contraseña no cumple los requisitos de seguridad.");
                }
            }
        }

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }

        public string Address { get; set; }

        private string phoneNumber;

        public string PhoneNumber 
        {
            get { return phoneNumber; }
            set
            {
                string patron = @"^[0-9]+$";

                if (Regex.IsMatch(value ?? "", patron))
                {
                    phoneNumber = value;
                }
                else
                {
                    throw new ArgumentException("El número de teléfono no cumple los requisitos.");
                }
            }
        }
        public string FullName { get; set; }
    }
}