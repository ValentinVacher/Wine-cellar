using Wine_cellar.Entities;

namespace Wine_cellar.ViewModel
{
    public class UpdateWineViewModel
    {
        public int WineId { get; set; }
        public WineColor Color { get; set; }
        public string Name { get; set; }
        public int AppelationId { get; set; }
        

    }
}
