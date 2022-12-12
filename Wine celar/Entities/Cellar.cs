using Microsoft.AspNetCore.SignalR;
using Wine_celar.Entities;
using Wine_cellar.Contexts;
using Wine_cellar.ViewModel;

namespace Wine_cellar.Entities
{
    public class Cellar
    {
        public int CellarId { get; set; }
        public string Name { get; set; }
        public int NbDrawerMax { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public CellarType CellarType { get; set; }
        public CellarBrand Brand { get; set; }
        public string? BrandOther { get; set; }
        public int Temperature { get; set; }
        public List<Drawer> Drawers { get; set; }



        public bool IsFull()
        {
            if (Drawers.Count() > NbDrawerMax) return true;
            return false;
        }
    }
}
