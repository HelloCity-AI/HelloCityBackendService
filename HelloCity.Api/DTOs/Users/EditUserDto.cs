using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloCity.Models.Enums;
using HelloCity.Models.Utils;


namespace HelloCity.Api.DTOs.Users
{
    public class EditUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public Gender Gender { get; set; }
        public string? Nationality { get; set; }
        public string? City { get; set; }
        public PreferredLanguage PreferredLanguage { get; set; }
    }
}