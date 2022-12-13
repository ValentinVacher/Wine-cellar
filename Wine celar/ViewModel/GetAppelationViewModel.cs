using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_celar.ViewModel
{
    public class GetAppelationViewModel
    {
        public int AppelationId { get; set; }
        public string Name { get; set; }
        public int KeepMin { get; set; }
        public int KeepMax { get; set; }
        public WineColor Color { get; set; }
        public List<GetWineViewModel> Wines { get; set; }
    }
}
