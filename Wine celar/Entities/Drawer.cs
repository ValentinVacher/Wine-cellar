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


        public Drawer ConvertorCreate(CreateDrawerViewModel viewModel)
        {
            return new Drawer()
            {
                Index = viewModel.index,
                NbBottleMax = viewModel.NbBottleMax,
                CellarId = viewModel.CellarId,
            };
        }

        public bool IsFull()
        {
            if (Wines.Count() >= NbBottleMax)
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
