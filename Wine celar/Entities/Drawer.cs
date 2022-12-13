using Wine_cellar.Contexts;
using Wine_cellar.ViewModel;

namespace Wine_cellar.Entities
{
    public class Drawer
    {
        public int DrawerId { get; set; }
        public int Index { get; set; }
        public int NbBottleMax { get; set; }
        /// <summary>
        /// Cave à laquelle appartient le tiroir
        /// </summary>
        public Cellar Cellar { get; set; }
        public int CellarId { get; set; }
        /// <summary>
        /// Liste de vin contenu dans le tiroir
        /// </summary>
        public List<Wine> Wines { get; set; }


        /// <summary>
        /// Permet de vérifier si le tiroir est plein
        /// </summary>
        /// <returns>True or False</returns>
        public bool IsFull()
        {
            if (Wines.Count() >= NbBottleMax) return true;
            return false; 
        }
    }
}
