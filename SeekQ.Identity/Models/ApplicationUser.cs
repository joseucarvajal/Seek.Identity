using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace SeekQ.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string EmailConfirmationCode { get; set; }
        public bool MakeFirstNamePublic { get; set; }
        public bool MakeLastNamePublic { get; set; }
        public bool MakeBirthDatePublic { get; set; }

        public int? GenderId { get; set; }
        public UserGender Gender { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }
    }
}
