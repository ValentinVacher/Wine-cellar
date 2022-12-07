using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wine_celar.Entities;

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
        public string PictureName { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }

       
    }
}
