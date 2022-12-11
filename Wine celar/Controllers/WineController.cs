using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Contexts;
using System.Security.Claims;
using Wine_celar.ViewModel;
using System.Text.Json;

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

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await wineRepository.GetAllWinesAsync(userId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWineById(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var wine = await wineRepository.GetWineByIdAsync(id, userId);

            if (wine == null) return NotFound($"Le vin {id} est introuvable");

            var WineView = new WineViewModel()
            {
                WineId = wine.WineId,
                WineName = wine.Name,
                CellarName = wine.Drawer.Cellar.Name,
                Year = wine.Year,
                Color = wine.Color,
                AppelationName = wine.Appelation.Name,
                DrawerIndex = wine.Drawer.Index
            };

            return Ok(WineView);
        }

        [HttpGet]
        public async Task<IActionResult> GetApogee()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var wines = await wineRepository.GetApogeeAsync(userId);

            if (wines == null) return NotFound("Aucun vin n'est a l'apogée");

            return Ok(wines);
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> GetWineByWord(string word)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wine = await wineRepository.GetWineByWordAsync(word, userId);

            if (wine == null) return NotFound($"Le vin {word} est introuvable");

            var WinesView = new List<WineViewModel>();
            foreach (var w in wine)
            {
                var WineView = new WineViewModel();
                WineView.WineId = w.WineId;
                WineView.WineName = w.Name;
                WineView.CellarName = w.Drawer.Cellar.Name;
                WineView.Year = w.Year;
                WineView.Color = w.Color;
                WineView.AppelationName = w.Appelation.Name;
                WineView.DrawerIndex = w.Drawer.Index;
                WinesView.Add(WineView);
            }

            return Ok(WinesView);
        }
        [HttpGet]
        public async Task<ActionResult<List<Wine>>> GetWineByColor(WineColor color)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wines = await wineRepository.GetWineByColorAsync(color, userId);

            if (wines == null) return NotFound($"Vous n'avez aucun vin {color}");

            var WinesView = new List<WineViewModel>();

            foreach (var w in wines)
            {
                var WineView = new WineViewModel();
                WineView.WineId = w.WineId;
                WineView.WineName = w.Name;
                WineView.CellarName = w.Drawer.Cellar.Name;
                WineView.Year = w.Year;
                WineView.Color = w.Color;
                WineView.AppelationName = w.Appelation.Name;
                WineView.DrawerIndex = w.Drawer.Index;
                WinesView.Add(WineView);
            }
            return Ok(WinesView);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWineAsync([FromForm]
        CreateWineViewModel WineViewModel)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wineCreated = await wineRepository.CreateWineAsync(WineViewModel, userId);

            switch (wineCreated)
            {
                case 1: return NotFound("Tiroir introuvable");
                case 2: return BadRequest("Le tiroir est plein");
                case 3: return NotFound("Le vin et l'appélation ne correspond pas");
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

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            int NbrWineCreate = await wineRepository.DuplicateAsync(WineId, NbrDuplicate, userId);

            if (NbrWineCreate != NbrDuplicate) return Ok($"{NbrDuplicate - NbrWineCreate} vin n'ont pas été crée car il n'y a plus de place dans le tiroir");

            return Ok($"Le vin {WineId} a été dupliquer {NbrWineCreate} fois");
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateWine([FromForm] UpdateWineViewModel wineView)
        //{
        //    var identity = User?.Identity as ClaimsIdentity;

        //    if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

        //    int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
        //    var wineUpdate = await wineRepository.UpdateWineAsync(wineView, userId);

        //    if (wineUpdate == null) return NotFound("Bouteille introuvable");

        //    return Ok(wineUpdate);
        //}

        [HttpPut]
        public async Task<ActionResult<Wine>> Move(int wineId, int drawerId)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wineMove = await wineRepository.MoveAsync(wineId, drawerId, userId);

            switch (wineMove)
            {
                case 1: return NotFound("Vin introuvable");
                case 2: return NotFound("Tiroir introuvable");
                case 3: return BadRequest("Le tiroire est plein");
                default: return Ok($"Le vin {wineId} a bien été déplacer");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWine([FromForm] UpdateWineViewModel wineView)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");
            var UserIdentity = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (await wineRepository.UpdateWineAsync(wineView, UserIdentity) == 0) return NotFound("Aucun vin a modifié");
            return Ok($"le vin{wineView.WineId} a été modifié ");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWine(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var success = await wineRepository.DeleteWineAsync(id, userId);

            if (success != 0) return Ok($"Le vin {id} a été supprimé");
            
            return NotFound("Vin introuvable");
        }

        //Controller méthode ef7 execute delete
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteWineEF(int id)
        //{

        //    var identity = User?.Identity as ClaimsIdentity;

        //    if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

        //    int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    if (await wineRepository.DeleteWineAsync(id,userId) == 0) return NotFound("Vin introuvable");
        //    return Ok($"le Vin {id} demandé a été supprimé");
           
        //}
    }
}
