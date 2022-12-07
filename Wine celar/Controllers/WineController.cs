using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Contexts;

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
        public async Task<IActionResult> GetAllWines()
        {
            return Ok(await wineRepository.GetAllWinesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWineById(int id)
        {
            var wine = await wineRepository.GetWineByIdAsync(id);

            if (wine == null)
                return NotFound($"Le vin {id} est introuvable");

            return Ok(wine);
        }

        [HttpGet]
        public async Task<IActionResult> GetApogee()
        {
            return Ok(await wineRepository.GetApogeeAsync());
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> GetWineByWord(string word)
        {
            var wine = await wineRepository.GetWineByWordAsync(word);

            if (wine == null)
                return NotFound($"Le vin {word} est introuvable");

            return Ok(wine);
        }
        [HttpPost]
        public async Task<IActionResult> CreateWineWithPictureAsync([FromForm]
        CreateWineViewModel WineViewModel)
        {
            var wine = new Wine()
            {
                Color = WineViewModel.Color,
                Appelation = WineViewModel.Appelation,
                Name = WineViewModel.Name,
                Year = WineViewModel.Year,
                KeepMin = WineViewModel.KeepMin,
                KeepMax = WineViewModel.KeepMax,
                DrawerId = WineViewModel.DrawerId,
                PictureName = WineViewModel.Picture?.FileName ?? "",
            };
            var wineCreated = await wineRepository.CreateWineWithPictureAsync(wine);
            if (!string.IsNullOrEmpty(WineViewModel.Picture?.FileName)
                && WineViewModel.Picture.FileName.Length > 0)
            {
                var path = Path.Combine(environment.WebRootPath, "img/",
                    WineViewModel.Picture.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await WineViewModel.Picture.CopyToAsync(stream);
                    stream.Close();
                }
            }

            if (wineCreated == null)
            {
                return Problem("Erreur lors de la création de la bouteille");
            }

            return Ok(wineCreated);



        }






        [HttpPost]
        public async Task<IActionResult> CreateWine ([FromForm] CreateWineViewModel wineView)
        {
            Wine wine = new()
            {
                Color = wineView.Color,
                Appelation = wineView.Appelation,
                Name = wineView.Name,
                Year = wineView.Year, 
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

        [HttpPost]
        public async Task<ActionResult<Wine>> Duplicate(int WineId, int NbrDuplicate)
        {
            return Ok(await wineRepository.DuplicateAsync(WineId, NbrDuplicate));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWine([FromForm] UpdateWineViewModel wineView)
        {
            Wine wine = new()
            {
                WineId = wineView.WineId,
                Color = wineView.Color,
                Appelation = wineView.Appelation,
                Name = wineView.Name,
                Year = wineView.Year,
                KeepMin = wineView.KeepMin,
                KeepMax = wineView.KeepMax
                //DrawerId = wineView.DrawerId
            };

            var wineUpdate = await wineRepository.UpdateWineAsync(wine);

            if(wineUpdate == null)
            {
                return Problem("Erreur lors de la mise a jour de la bouteille");
            }

            return Ok(wineUpdate);
        }

        [HttpPut]
        public async Task<ActionResult<Wine>> Move(int WineId, int newDrawerId)
        {
            return Ok(await wineRepository.MoveAsync(WineId, newDrawerId));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWine(int id)
        {
            bool success = await wineRepository.DeleteWineAsync(id);

            if (success)
                return Ok($"Le vin {id} a été supprimé");
            else
                return Problem($"Erreur lors de la suppression du vin");
        }
    }
}
