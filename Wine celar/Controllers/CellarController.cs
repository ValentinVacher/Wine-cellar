using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
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
            return Ok(await cellarRepository.GetAllsAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCellarWithAll(int id)
        {
            var cellar = await cellarRepository.GetCellarWithAllAsync(id);
            if (cellar == null)
                return NotFound($"Cave {id} non trouver");
            return Ok(cellar);       
        }
        [HttpPost]
        public async Task<IActionResult> AddCellar(CreateCellarViewModel cellarViewModel, int Nbr)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null)
                return Problem("Vous devez être connecter pour ajouter une cave");

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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCellar(UpdateCellarViewModel UpCellar, int id)
        {
            var cellar = new Cellar()
            {
                CellarId = id,
                Name = UpCellar.Name,
                UserId= UpCellar.UserId
                
            };
            return Ok(await cellarRepository.UpdateCellarAsync(cellar));

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCellar(int id)
        {
            bool success = await cellarRepository.DeleteCellarAsync(id);

            if (success)
                return Ok($"La cave {id} a été supprimé");
            else
                return Problem($"Erreur lors de la suppression de la cave");
        }


    }
}
