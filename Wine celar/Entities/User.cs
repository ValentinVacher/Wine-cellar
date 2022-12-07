using System.Security.Claims;

namespace Wine_cellar.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Cellar> Cellars { get; set; }


        public bool IsOlder()
        {
            var age = DateTime.Now - DateOfBirth;

            var age2 = age.Days / 365.25;

            if (age2 >= 18)
            {
                return true;
            }
            else
                return false;
        }

        
    }
}
