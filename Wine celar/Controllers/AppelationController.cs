using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using Wine_celar.Repositories;
using Wine_celar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;

namespace Wine_celar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AppelationController : ControllerBase
    {
        readonly IAppelationRepository AppelationRepository;
        readonly IWebHostEnvironment environment;
        public AppelationController(IAppelationRepository Repository, IWebHostEnvironment environment)
        {
            this.AppelationRepository = Repository;
            this.environment = environment;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAppelation()
        {
            return Ok(await AppelationRepository.GetAllAppelationsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetAppelation(string appelationName)
        {
            var appel = await AppelationRepository.GetAppelationAsync(appelationName);

            if (appel == null)
                return NotFound($"Le vin {appelationName} est introuvable");

            return Ok(appel);
        }

        [HttpGet]
        public async Task<ActionResult<List<Appelation>>> GetAppelationsByColor(WineColor color)
        {
            var appelations = await AppelationRepository.GetAppelationsByColoAsync(color);
            if (appelations == null) return NotFound($"Aucune Appelation pour un vin {color}");
            return appelations;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppelation([FromForm] CreateAppelationViewModel appelViewModel)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest("Vous devez être admin");

            var appel = new Appelation().ConvertorCreate(appelViewModel);
            var AppelationCreated = await AppelationRepository.CreateAppelationAsync(appel);

            if (AppelationCreated == null) return BadRequest("L'appelation existe deja");

            return Ok(AppelationCreated);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppelation([FromForm] CreateAppelationViewModel appelViewModel)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest("Vous devez être admin");

            Appelation appel = new()
            {
                AppelationName = appelViewModel.AppelationName,
                KeepMin = appelViewModel.KeepMin,
                KeepMax = appelViewModel.KeepMax
            };

            var appelUpdate = await AppelationRepository.UpdateAppelationAsync(appel);

            if (appelUpdate == null) return NotFound("Appelation introuvable");

            return Ok(appelUpdate);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAppelation(string appelationName)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest("Vous devez être admin");

            var success = await AppelationRepository.DeleteAppelationAsync(appelationName);

            if (success != null) return Ok(success);

            return NotFound("Appelation introuvable");
        }
    }
}
