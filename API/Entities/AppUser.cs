using System;
using System.Collections.Generic;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public DateTime Birthdate { get; set; }
        public string Alias { get; set; }
        public DateTime AccountRegistrationDate { get; set; } = DateTime.Now;
        public DateTime LastLogon { get; set; } = DateTime.Now;
        public string Seeking { get; set; }
        public string Interest { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }

        public int GetAge()
        {
            return Birthdate.AgeCalc();
        }
    }
}