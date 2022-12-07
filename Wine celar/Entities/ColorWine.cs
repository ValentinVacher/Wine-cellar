using Wine_cellar.Entities;

namespace Wine_celar.Entities
{
    public class Color
    {
        public int ColorId { get; set; }
        public string Name { get; set; }
        public List<Appelation> Appelations { get; set; }
        public List<Wine> Wines { get; set; }
    }
}
