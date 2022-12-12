using Wine_cellar.Entities;

namespace Wine_cellar.ViewModel
{
    public class UpdateAppelationViewModel
    {
        public int AppelationId { get; set; }
        public string Name { get; set; }
        public int KeepMax { get; set; }
        public int KeepMin { get; set; }
        public WineColor Color { get; set; }
    }
}
