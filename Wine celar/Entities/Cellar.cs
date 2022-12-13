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
        /// <summary>
        /// Nombre de tiroirs maximum
        /// </summary>
        public int NbDrawerMax { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// Utilisateur à qui appartient la cave
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Type de cave(vieillissement, conservation ...)
        /// </summary>
        public CellarType CellarType { get; set; }
        /// <summary>
        /// Marque de la cave
        /// </summary>
        public CellarBrand Brand { get; set; }
        /// <summary>
        /// Si la marque n'est pas enregistrer l'utilisateur peut en ajouter une
        /// </summary>
        public string? BrandOther { get; set; }
        public int Temperature { get; set; }
        /// <summary>
        /// Liste des tiroirs dans la cave
        /// </summary>
        public List<Drawer> Drawers { get; set; }


        /// <summary>
        /// Methode pour verifier si la cave est pleine
        /// </summary>
        /// <returns>True or False</returns>
        public bool IsFull()
        {
            if (Drawers.Count() > NbDrawerMax) return true;
            return false;
        }
    }
}
