using System.Reflection.Metadata.Ecma335;
using Wine_cellar.Entities;

namespace Wine_celar.ViewModel
{
    public class WineViewModel
    {
        public int WineId { get; set; }
        public string WineName { get; set; }
        public int Year { get; set; }
        public WineColor Color { get; set; }
        public string AppelationName { get; set; }
        public string CellarName { get; set; }
        public int DrawerIndex { get; set; }

        public WineViewModel Convertor(Wine wine)
        {
           return new WineViewModel()
           {
               WineId = wine.WineId,
               WineName = wine.Name,
               CellarName = wine.Drawer.Cellar.Name,
               Year = wine.Year,
               Color = wine.Color,
               AppelationName = wine.Appelation.AppelationName,
               DrawerIndex = wine.Drawer.Index
           };
               
        }
    }
}
