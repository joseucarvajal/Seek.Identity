using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SeekQ.Identity.Models.Profile;

namespace SeekQ.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string EmailConfirmationCode { get; set; }
        public bool MakeFirstNamePublic { get; set; }
        public bool MakeLastNamePublic { get; set; }
        public bool MakeBirthDatePublic { get; set; }

        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string School { get; set; }
        public string Job { get; set; }
        public string About { get; set; }

        public ICollection<UserLanguageKnow> LanguageKnows { get; set; }

        public int? GenderId { get; set; }
        public UserGender Gender { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }
    }
}
