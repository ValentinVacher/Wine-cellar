using Microsoft.AspNetCore.SignalR;
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
        //public int temperature { get; set; }
        public List<Drawer> Drawers { get; set; }

        public static Cellar ConvertorCreate(CreateCellarViewModel viewModel)
        {
            return new Cellar()
            {
                Name = viewModel.Name,
                NbDrawerMax = viewModel.NbDrawerMax
            };
        }

        public bool IsFull()
        {
            if (Drawers.Count() > NbDrawerMax) return true;
            return false;
        }
    }
}
