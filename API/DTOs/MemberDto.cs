using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public int Age { get; set; }
        public string Alias { get; set; }
        public DateTime AccountRegistrationDate { get; set; }
        public DateTime LastLogon { get; set; }
        public string Seeking { get; set; }
        public string Interest { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public string ProfilePhoto { get; set; }
    }
}