using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> CreateAppelation([FromForm] CreateAppelationViewModel appelViewModel)
        {
            Appelation appel = new()
            {
                AppelationName = appelViewModel.AppelationName,
                KeepMin = appelViewModel.KeepMin,
                KeepMax = appelViewModel.KeepMax
            };
            var AppelationCreated = await AppelationRepository.CreateAppelationAsync(appel);

            if (AppelationCreated == null)
            {
                return Ok("Le vin existe deja");
                
            }
            
            
            return Ok(AppelationCreated);
        }

        [HttpPut]
        public async Task<Appelation> UpdateAppelation([FromForm]CreateAppelationViewModel appelViewModel)
        {
            Appelation appel = new()
            {
                AppelationName = appelViewModel.AppelationName,
                KeepMin = appelViewModel.KeepMin,
                KeepMax = appelViewModel.KeepMax
            };

            var appelUpdate = await AppelationRepository.UpdateAppelationAsync(appel);

            if (appelUpdate == null)
            {
                return null;
            }

            return appelUpdate;
        }
        [HttpDelete]
        public async Task<Appelation> DeleteAppelation(string appelationName)
        {
            var success = await AppelationRepository.DeleteAppelationAsync(appelationName);

            if (success != null)
                return success;
            else
                return null;
        }
    }
}
