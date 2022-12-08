using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Contexts;
using System.Security.Claims;
using Wine_celar.ViewModel;

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
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            return Ok(await wineRepository.GetAllWinesAsync(identity));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWineById(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            var wine = await wineRepository.GetWineByIdAsync(id, identity);

            if (wine == null)
                return NotFound($"Le vin {id} est introuvable");
            var WineView = new WineViewModel()
            {
                WineId = wine.WineId,
                WineName = wine.Name,
                CellarName = wine.Drawer.Cellar.Name,
                Year = wine.Year,
                Color = wine.Color,
                AppelationName = wine.Appelation.AppelationName,
                DrawerIndex = wine.Drawer.Index
            };

            return Ok(WineView);
        }

        [HttpGet]
        public async Task<IActionResult> GetApogee()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            var wines = await wineRepository.GetApogeeAsync(identity);

            if (wines == null) return NotFound("Aucun vin n'est a l'apogée");

            return Ok(wines);
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> GetWineByWord(string word)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            var wine = await wineRepository.GetWineByWordAsync(word, identity);

            if (wine == null) return NotFound($"Le vin {word} est introuvable");

            return Ok(wine);
        }
        [HttpGet]
        public async Task<ActionResult<List<Wine>>> GetWineByColor(WineColor color)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            var wines = await wineRepository.GetWineByColorAsync(color, identity);
            if (wines == null) return NotFound($"Vous n'avez aucun vin {color}");
            return Ok(wines);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWineAsync([FromForm]
        CreateWineViewModel WineViewModel)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return Problem("Vous devez être connecter");

            var wineCreated = await wineRepository.CreateWineAsync(WineViewModel, identity);

            switch (wineCreated)
            {
                case 1: return NotFound("Tiroir introuvable");
                case 2: return Problem("Le tiroir est plein");
                case 3: return Problem("Le vin et l'appélation ne correspond pas");
                default: break;
            }

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

            return Ok(WineViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<Wine>> Duplicate(int WineId, int NbrDuplicate)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            int NbrWineCreate = await wineRepository.DuplicateAsync(WineId, NbrDuplicate, identity);

            if (NbrWineCreate != NbrDuplicate) return Ok($"{NbrDuplicate - NbrWineCreate} vin n'ont pas été crée car il n'y a plus de place dans le tiroir");

            return Ok($"Le vin {WineId} a été dupliquer {NbrWineCreate} fois");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWine([FromForm] UpdateWineViewModel wineView)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            var wineUpdate = await wineRepository.UpdateWineAsync(wineView, identity);

            if (wineUpdate == null) return NotFound("Bouteille introuvable");

            return Ok(wineUpdate);
        }

        [HttpPut]
        public async Task<ActionResult<Wine>> Move(int WineId, int newDrawerIndex, string cellar)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            var wineMove = await wineRepository.MoveAsync(WineId, newDrawerIndex, cellar, identity);

            switch (wineMove)
            {
                case 1: return NotFound("Vin introuvable");
                case 2: return NotFound("Tiroir introuvable");
                case 3: return NotFound("Le tiroire est plein");
                default: return Ok($"Le vin {WineId} a bien été déplacer");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWine(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            bool success = await wineRepository.DeleteWineAsync(id, identity);

            if (success)
                return Ok($"Le vin {id} a été supprimé");
            else
                return Problem("Vin introuvable");
        }
    }
}
