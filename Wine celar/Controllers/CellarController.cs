using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;
using Wine_cellar.ViewModel;
using Wine_celar.Repositories;
using System.Security.Claims;

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
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return Problem("Vous devez être connecter");

            return Ok(await cellarRepository.GetAllsAsync(identity));
        }

        [HttpGet]
        public async Task<IActionResult> GetCellarByName(string name)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return Problem("Vous devez être connecter");

            var cellar = await cellarRepository.GetCellarByName(name, identity);

            if (cellar == null) return NotFound($"Cave {name} non trouver");
            return Ok(cellar);       
        }

        [HttpPost]
        public async Task<IActionResult> AddCellar(CreateCellarViewModel cellarViewModel, int Nbr)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);
            var verif = (await cellarRepository.GetAllsAsync(identity)).FirstOrDefault(x => x.Name == cellarViewModel.Name);

            if (verif != null) return Problem("Ce nom est déjà pris");

            if (idCurrentUser == null) return Problem("Vous devez être connecter pour ajouter une cave");

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
        public async Task<IActionResult> UpdateCellar(UpdateCellarViewModel UpCellar, string name)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var cellar = (await cellarRepository.GetCellarByName(name, identity)).FirstOrDefault();

            if (cellar == null) return Problem("Cave introuvable");

            var cellarUpdate = new Cellar()
            {
                CellarId = cellar.CellarId,
                Name = UpCellar.Name,
                UserId= UpCellar.UserId
                
            };

            return Ok(await cellarRepository.UpdateCellarAsync(cellarUpdate));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCellar(string name)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var cellar = (await cellarRepository.GetCellarByName(name, identity)).FirstOrDefault();

            if (cellar == null) return Problem("Cave introuvable");

            bool success = await cellarRepository.DeleteCellarAsync(cellar);

            if (success)
                return Ok($"La cave {name} a été supprimé");
            else
                return Problem($"Erreur lors de la suppression de la cave");
        }
    }
}
