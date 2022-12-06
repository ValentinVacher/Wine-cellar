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
            int Age = DateTime.Now.Year - DateOfBirth.Year;
            if (Age >= 18)
            {
                return true;
            }
            else
                return false;
        }

        public void DeleteCelars()
        {
            foreach(var cellar in Cellars)
            {

            }
        }
    }
}
