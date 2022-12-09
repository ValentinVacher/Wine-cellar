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
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await cellarRepository.GetAllsAsync(userId));
        }

        [HttpGet]
        public async Task<IActionResult> GetCellarByName(string name)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cellar = await cellarRepository.GetCellarByName(name, userId);

            if (cellar == null) return NotFound($"Cave {name} non trouver");
            return Ok(cellar);       
        }

        [HttpPost]
        public async Task<IActionResult> AddCellar([FromForm]CreateCellarViewModel cellarViewModel, int Nbr)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var verif = (await cellarRepository.GetAllsAsync(userId)).FirstOrDefault(x => x.Name == cellarViewModel.Name);

            if (verif != null) return BadRequest("Ce nom est déjà pris");

            Cellar cellar = new()
            {
                Name = cellarViewModel.Name,
                NbDrawerMax = cellarViewModel.NbDrawerMax,
                UserId = userId
            };
            var cellarCreated = await cellarRepository.AddCellarAsync(cellar, Nbr);

            return Ok(cellarCreated);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCellar([FromForm]UpdateCellarViewModel UpCellar, string actualname)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cellar = (await cellarRepository.GetCellarByName(actualname, userId)).FirstOrDefault();

            if (cellar == null) return NotFound("Cave introuvable");

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

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cellar = (await cellarRepository.GetCellarByName(name, userId)).FirstOrDefault();

            if (cellar == null) return NotFound("Cave introuvable");

            bool success = await cellarRepository.DeleteCellarAsync(cellar);

            if (success) return Ok($"La cave {name} a été supprimé");

            return NotFound("Cave introuvable");
        }
    }
}
