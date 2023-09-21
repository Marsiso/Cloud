using Cloud.Domain.Application.Models;

namespace Cloud.Data.Tests.Helpers;

public static class TestDataGenerator
{
    public static class Users
    {
        public static User GetUser()
        {
            return new User
            {
                ID = 1,
                IsActive = true,
                CreatedBy = default,
                UpdatedBy = default,
                DateCreated = DateTime.UtcNow - TimeSpan.FromDays(30),
                DateUpdated = DateTime.UtcNow - TimeSpan.FromDays(15),
                GivenName = "givenname",
                FamilyName = "familyname",
                Email = "givenname.familyname",
                HasEmailConfirmed = true,
                Password = default,
                PasswordSalt = default,
                ProfilePhotoUrl = default
            };
        }
    }
}
