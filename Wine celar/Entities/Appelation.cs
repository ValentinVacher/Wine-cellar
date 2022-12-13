using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wine_cellar.Repositories;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.Repositories;

namespace Wine_cellar.Entities
{
    
    public class Appelation
    {
        
        [Key]
        public int AppelationId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Temps minimal necessaire pour atteindre l'apogée
        /// </summary>
        public int KeepMin { get; set; }
        /// <summary>
        /// Temps maximal pour l'apogée
        /// </summary>
        public int KeepMax { get; set; }
        public WineColor Color { get; set; }
        /// <summary>
        /// Liste des vins correspondant à une appelations
        /// </summary>
        public List<Wine> Wines { get; set; }
    }
}
