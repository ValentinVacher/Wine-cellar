using Wine_cellar.Entities;

namespace Wine_celar.ViewModel
{
    public class CreateAppelationViewModel
    {
        public string AppelationName { get; set; }
        public int KeepMax { get; set; }
        public int KeepMin { get; set; }

        public WineColor Color { get; set;}
    }
}
