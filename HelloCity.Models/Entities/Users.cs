using HelloCity.Models.Enums;
using GenderEnum = HelloCity.Models.Enums.Gender;
using static System.Net.WebRequestMethods;

namespace HelloCity.Models.Entities
{
    public class Users
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        /// <summary>
        ///  URL of the user's avatar.
        ///  Currently， uses a placeholder. To be replaced with an S3-hosted image after AWS integration.
        /// </summary>
        public string Avatar { get; set; } = "https://via.placeholder.com/150";
        public Gender Gender { get; set; } = GenderEnum.PreferNotToSay;
        public string? Nationality { get; set; }
        public string? City { get; set; }
        public string? University { get; set; }
        public string? Major { get; set; }
        public PreferredLanguage PreferredLanguage { get; set; } = PreferredLanguage.en;
        public DateTime LastJoinDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<ChecklistItem> ChecklistItems { get; set; } = new();
    }
}
