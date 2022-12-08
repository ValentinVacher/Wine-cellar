using Wine_cellar.Entities;

namespace Wine_cellar.ViewModel
{
    public class UpdateWineViewModel
    {
        public int WineId { get; set; }
        public WineColor Color { get; set; }
        public string Appelation { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int KeepMin { get; set; }
        public int KeepMax { get; set; }
    }
}
