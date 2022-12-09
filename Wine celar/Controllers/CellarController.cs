using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using System.Security.Claims;
using Wine_celar.ViewModel;
using Wine_cellar.Repositories;
using System.Text.Json;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CellarController : ControllerBase
    {
        readonly ICellarRepository cellarRepository;
        readonly IWebHostEnvironment environment;
        public CellarController(ICellarRepository cellarRepository, IWebHostEnvironment environment)
        {
            this.cellarRepository = cellarRepository;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlls()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            return Ok(await cellarRepository.GetAllsAsync(identity));
        }

        [HttpGet]
        public async Task<IActionResult> GetCellarByName(string name)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            var cellar = await cellarRepository.GetCellarByName(name, identity);

            if (cellar == null) return NotFound($"Cave {name} non trouver");
            return Ok(cellar);
        }

        [HttpPost]
        public async Task<IActionResult> AddCellar([FromForm] CreateCellarViewModel cellarViewModel, int Nbr)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return Problem("Vous devez être connecter");

            var verif = (await cellarRepository.GetAllsAsync(identity)).FirstOrDefault(x => x.Name == cellarViewModel.Name);

            if (verif != null) return Problem("Ce nom est déjà pris");

            Cellar cellar = new()
            {
                Name = cellarViewModel.Name,
                NbDrawerMax = cellarViewModel.NbDrawerMax,
                UserId = int.Parse(idCurrentUser.Value)
            };
            var cellarCreated = await cellarRepository.AddCellarAsync(cellar, Nbr);
            if (cellarViewModel != null)
                return Ok(cellarCreated);
            else
                return Problem("Cave non créer");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCellar([FromForm] UpdateCellarViewModel UpCellar, string actualname)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            var cellar = (await cellarRepository.GetCellarByName(actualname, identity)).FirstOrDefault();

            if (cellar == null) return NotFound("Cave introuvable");

            var cellarUpdate = new Cellar()
            {
                CellarId = cellar.CellarId,
                Name = UpCellar.Name,
                UserId = UpCellar.UserId

            };

            return Ok(await cellarRepository.UpdateCellarAsync(cellarUpdate));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCellar(string name)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return Problem("Vous devez être connecter");

            var cellar = (await cellarRepository.GetCellarByName(name, identity)).FirstOrDefault();

            if (cellar == null) return NotFound("Cave introuvable");

            bool success = await cellarRepository.DeleteCellarAsync(cellar);

            if (success)
                return Ok($"La cave {name} a été supprimé");
            else
                return Problem($"Erreur lors de la suppression de la cave");
        }
        [HttpPost]
        public async Task<IActionResult> ImportJson([FromForm]string Jfile)
        {
            var path = Path.Combine(environment.ContentRootPath, "Json\\", Jfile + ".json");
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                
                await cellarRepository.ImportJsonAsync(Jfile);
                //Cellar? cellarJson =
                //await JsonSerializer.DeserializeAsync<Cellar>(Jfile );
                
                stream.Close();
            }
            return Ok();
        }
    }
}
