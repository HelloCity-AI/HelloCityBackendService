using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloCity.Models.Enums;

namespace HelloCity.Api.DTOs.Users
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set;}
        public required string Avatar { get; set; }
        public Gender Gender { get; set; }
        public string? Nationality { get; set; }
        public string? City { get; set; }
        public string? University { get; set; }
        public string? Major { get; set; }
        public PreferredLanguage PreferredLanguage { get; set; }
        public DateTime LastJoinDate { get; set; }
    }
}
