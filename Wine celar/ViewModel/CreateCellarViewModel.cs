using Wine_celar.Entities;
using Wine_cellar.Entities;

namespace Wine_cellar.ViewModel
{
    public class CreateCellarViewModel
    {
        public string Name { get; set; }
        public int NbDrawerMax { get; set; }
        public CellarType CellarType { get; set; }
        public CellarBrand Brand { get; set; }
        public string? BrandOther { get; set; }
        public int Temperature { get; set; }

    }
}
