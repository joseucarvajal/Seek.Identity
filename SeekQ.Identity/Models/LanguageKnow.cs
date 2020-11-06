namespace SeekQ.Identity.Models
{
    using System.Collections.Generic;
    using App.Common.SeedWork;

    public class LanguageKnow : Enumeration
    {
        public LanguageKnow(int id, string name)
                   : base(id, name)
        {
        }

        public ICollection<UserLanguageKnow> LanguageKnows { get; set; }
    }
}
