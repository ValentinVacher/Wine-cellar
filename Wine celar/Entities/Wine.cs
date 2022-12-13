using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.Entities
{
    public class Wine
    {
        public int WineId { get; set; }
        //public string Color { get; set; }
        //public string Appelation { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Correspond au millésime 
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Tiroir dans lequelle est stocker la bouteille
        /// </summary>
        public Drawer Drawer { get; set; }
        public int DrawerId { get; set; }
        public string? PictureName { get; set; }
        public WineColor Color { get; set; }
        public int AppelationId { get; set; }
        public Appelation Appelation { get; set; }
    }
}
