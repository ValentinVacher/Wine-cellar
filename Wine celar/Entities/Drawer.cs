using Wine_cellar.Contexts;

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
            if (Wines.Count >= NbBottleMax)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }

        public void DeleteWines(WineContext wineContext)
        {
            foreach(var wine in Wines)
            {
                wineContext.Wines.Remove(wine);
            }
        }
    }
}
