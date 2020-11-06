namespace SeekQ.Identity.Application.Profile.ViewModels
{
    using System;

    public class UserViewModel
    {
        public Guid IdUser { get; set; }
        public bool MakeFirstNamePublic { get; set; }
        public bool MakeLastNamePublic { get; set; }
        public bool MakeBirthDatePublic { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string School { get; set; }
        public string Job { get; set; }
        public string About { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderId { get; set; }
        public int GenderName { get; set; }
        public int LanguageKnowId { get; set; }
        public int LanguageKnowName { get; set; }
    }
}