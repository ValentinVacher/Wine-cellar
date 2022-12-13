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
        /// <summary>
        /// Liste des caves appartenant à l'utilisateur
        /// </summary>
        public List<Cellar> Cellars { get; set; }
        /// <summary>
        /// Verifie si l'utilisateur est un admin
        /// </summary>
        public bool IsAdmin { get; set; } = false;


        /// <summary>
        /// Methode pour verifier si l'utilisateur à l'age légal
        /// </summary>
        /// <returns>True or False</returns>
        public bool IsOlder()
        {
            var age = DateTime.Now - DateOfBirth;
            var age2 = age.Days / 365.25;

            if (age2 >= 18) return true;    
            return false;
        }     
    }
}
