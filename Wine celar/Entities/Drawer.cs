using Wine_cellar.Contexts;
using Wine_cellar.ViewModel;

namespace Wine_cellar.Entities
{
    public class Drawer
    {
        public int DrawerId { get; set; }
        public int Index { get; set; }
        public int NbBottleMax { get; set; }
        public Cellar Cellar { get; set; }
        public int CellarId { get; set; }
        public List<Wine> Wines { get; set; }

        public bool IsFull()
        {
            if (Wines.Count() >= NbBottleMax) return true;
            return false; 
        }
    }
}
