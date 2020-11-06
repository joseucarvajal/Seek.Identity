using App.Common.SeedWork;

namespace SeekQ.Identity.Models
{
    public class UserGender : Enumeration
    {
        public static UserGender Female = new UserGender(1, "Female");
        public static UserGender Male = new UserGender(2, "Female");
        public static UserGender Agender = new UserGender(3, "Agender");
        public static UserGender Androgyne = new UserGender(4, "Androgyne");
        public static UserGender Androgynous = new UserGender(5, "Androgynous");
        public static UserGender Trasgender = new UserGender(6, "Trasgender");
        public static UserGender TrasgenderMen = new UserGender(7, "Trasgender men");
        public static UserGender TrasgenderWomen = new UserGender(8, "Trasgender women");
        public static UserGender Other = new UserGender(9, "Other");

        public UserGender(int id, string name)
                   : base(id, name)
        {
        }
    }
}
