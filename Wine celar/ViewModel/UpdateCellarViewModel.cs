using Wine_celar.Entities;

namespace Wine_cellar.ViewModel
{
    public class UpdateCellarViewModel : CreateCellarViewModel
    {
        public int CellarId { get; set; }
        public int UserId { get; set; }
    }
}
