using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wine_celar.Repositories;
using Wine_celar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.Repositories;

namespace Wine_cellar.Entities
{
    public class Appelation
    {
        [Key]
        public int AppelationId { get; set; }
        public string Name { get; set; }
        public int KeepMin { get; set; }
        public int KeepMax { get; set; }
        public WineColor Color { get; set; }
        public List<Wine> Wines { get; set; }

        public Appelation ConvertorCreate(CreateAppelationViewModel viewModel)
        {
            return new Appelation()
            {
                Name = viewModel.Name,
                KeepMin = viewModel.KeepMin,
                KeepMax = viewModel.KeepMax,
                Color = viewModel.Color,
            };
        }

    }
}
