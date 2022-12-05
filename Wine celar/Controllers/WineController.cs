using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Wine_celar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WineController : ControllerBase
    {
        readonly IWineRepository wineRepository;
        readonly IWebHostEnvironment environment;
        public WineController(IWineRepository Repository, IWebHostEnvironment environment)
        {
            this.wineRepository = Repository;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWinesAsync()
        {
            return Ok(await wineRepository.GetAllWinesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWineByIdAsync(int id)
        {
            var wine = await wineRepository.GetWineByIdAsync(id);

            if (wine == null)
                return NotFound($"Le vin {id} est introuvable");

            return Ok(wine);
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> GetWineByWordAsync(string word)
        {
            var wine = await wineRepository.GetWineByWordAsync(word);

            if (wine == null)
                return NotFound($"Le vin {word} est introuvable");

            return Ok(wine);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWineAsync (CreateWineViewModel wineView)
        {
            Wine wine = new()
            {
                Color = wineView.Color,
                Appelation = wineView.Appelation,
                Name = wineView.Name,
                Year = wineView.Year,
                Today = DateTime.Now,
                KeepMax = wineView.KeepMax,
                KeepMin = wineView.KeepMin,
                DrawerId = wineView.DrawerId
            };

            var wineCreated = await wineRepository.CreateWineAsync(wine);

            if (wineCreated ==  null)
            {
                return Problem("Erreur lors de la création de la bouteille");
            }

            return Ok(wineCreated);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWineAsync(Wine wine)
        {
            var wineUpdate = await wineRepository.CreateWineAsync(wine);

            if(wineUpdate == null)
            {
                return Problem("Erreur lors de la mise a jour de la bouteille");
            }

            return Ok(wineUpdate);
        }
    }
}
