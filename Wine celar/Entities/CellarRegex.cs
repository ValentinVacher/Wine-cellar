using System.Text.RegularExpressions;

namespace Wine_cellar.Entities
{
    public class CellarRegex
    {
        public Regex Password { get; set; } = new(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
        public Regex Email { get; set; } = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
    }
}
