using Microsoft.EntityFrameworkCore;

namespace Wine_cellar.Entities
{
    public class ColorWine
    {
        
        public int ColorId { get; set; }
        public string Name { get; set; }
        public List<Appelation> Appelations { get; set; }
        public List<Wine> Wines { get; set; }
    }
}
