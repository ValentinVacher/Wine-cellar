using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wine_cellar.Entities;

namespace Wine_cellar.Entities
{
    public class Wine
    {
        public int WineId { get; set; }
        //public string Color { get; set; }
        //public string Appelation { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        //public int KeepMin { get; set; }
        //public int KeepMax { get; set;}
        public Drawer Drawer { get; set; }
        public int DrawerId { get; set; }
        public string? PictureName { get; set; }
        public WineColor Color { get; set; }
        
        public int AppelationId { get; set; }
        public Appelation Appelation { get; set; }



    }
}
