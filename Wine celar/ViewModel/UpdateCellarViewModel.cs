using Wine_celar.Entities;

namespace Wine_cellar.ViewModel
{
    public class UpdateCellarViewModel
    {
        public int CellarId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public int NbDrawerMax { get; set; }
        public CellarType CellarType { get; set; }
        public CellarBrand Brand { get; set; }
        public string? BrandOther { get; set; }
        public int Temperature { get; set; }


    }
}
