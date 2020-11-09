namespace SeekQ.Identity.Models
{
    using App.Common.Repository;

    public class UserLanguageKnow : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int LanguageKnowId { get; set; }
        public LanguageKnow LanguageKnow { get; set; }
    }
}