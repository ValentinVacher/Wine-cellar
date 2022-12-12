using System.Reflection.Metadata.Ecma335;
using Wine_cellar.Entities;

namespace Wine_cellar.ViewModel
{
    public class GetWineViewModel
    {
        public int WineId { get; set; }
        public string WineName { get; set; }
        public int Year { get; set; }
        public WineColor Color { get; set; }
        public string AppelationName { get; set; }
        public string CellarName { get; set; }
        public int DrawerIndex { get; set; }       
    }
}
