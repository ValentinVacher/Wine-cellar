using Microsoft.AspNetCore.SignalR;
using Wine_cellar.Contexts;

namespace Wine_cellar.Entities
{
    public class Cellar
    {
        public int CellarId { get; set; }
        public string Name { get; set; }
        public int NbDrawerMax { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Drawer> Drawers { get; set; }

        public bool IsFull()
        {
            if (Drawers.Count() >= NbDrawerMax) return true;
            return false;
        }

        public void DeleteDrawer(WineContext wineContext)
        {
            foreach(var drawer in Drawers)
            {
                drawer.DeleteWines(wineContext);
                wineContext.Drawers.Remove(drawer);
            }
        }
    }
}
