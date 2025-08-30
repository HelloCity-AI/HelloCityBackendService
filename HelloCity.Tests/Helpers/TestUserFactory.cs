using HelloCity.Models.Entities;
using HelloCity.Models.Enums;
using HelloCity.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloCity.Tests.Helpers
{
    internal class TestUserFactory
    {
        public static Users CreateTestUser()
        {
            return new Users
            {
                UserId = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                AvatarKey = "avatar.png",
                Gender = Gender.Male,
                Nationality = "Australia",
                City = "Sydney",
                University = "ACU",
                Major = "IT",
                PreferredLanguage = PreferredLanguage.en,
                LastJoinDate = DateTimeHelper.GetUtcNow()  
            };
        }
    }
}
