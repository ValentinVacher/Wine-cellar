using Wine_cellar.Entities;

namespace Wine_cellar.ViewModel
{
    public class GetDrawerViewModel
    {
        public int DrawerId { get; set; }
        public int NbBottleMax { get; set; }
        public string CellarName { get; set; }
        public List<GetWineViewModel> Wines { get; set; }
    }
}
