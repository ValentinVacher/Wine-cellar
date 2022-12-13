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
using Wine_cellar.Tools;
using Wine_cellar.ViewModel;

namespace Wine_celar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AppelationController : ControllerBase
    {
        readonly IAppelationRepository AppelationRepository;
        public AppelationController(IAppelationRepository Repository)
        {
            this.AppelationRepository = Repository;
        }


        /// <summary>
        /// Permet d'afficher toute les appellations
        /// </summary>
        /// <response code = "200">Les appellations : </response>
        /// <returns>Retourne une liste de toutes les appellations</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAppelations()
        {
            return Ok(await AppelationRepository.GetAllAppelationsAsync());
        }

        /// <summary>
        /// Permet de voir une appellation choisi par id
        /// </summary>
        /// <response code ="200">Appelation : </response>
        /// <response code = "404">L'appellation n'existe pas</response>
        /// <param name="id"></param>
        /// <returns>Retourne l'appelation choisi</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppelationById(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var appel = await AppelationRepository.GetAppelationByIdAsync(id, userId);

            if (appel == null) return NotFound(ErrorCode.AppelationNotFound);

            return Ok(appel);
        }

        /// <summary>
        /// Permet de récupérer les appellations correspondant à une couleur spécifique
        /// </summary>
        /// <param name="color">Indiquez la couleur souhaiter</param>
        /// <response code = "200">Appellations : </response>
        /// <response code = "404">Appellations introuvable</response>
        /// <returns>Retourne les appelations coreespondante à la couleur saisi</returns>
        [HttpGet]
        public async Task<ActionResult<List<Appelation>>> GetAppelationsByColor(WineColor color)
        {
            var appelations = await AppelationRepository.GetAppelationsByColorAsync(color);

            if (appelations == null) return NotFound(ErrorCode.AppelationNotFound);

            return Ok(appelations);
        }

        /// <summary>
        /// Permet de créer une appellation (Uniquement pour l'admin)
        /// </summary>
        /// <param name="appelViewModel"></param>
        /// <response code ="200">Appellation créer : </response>
        /// <returns>Retourne l'appellation créer</returns>
        [HttpPost]
        public async Task<IActionResult> AddAppelation([FromForm] CreateAppelationViewModel appelViewModel)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            var appel = Convertor.CreateAppelation(appelViewModel);
            var AppelationCreated = await AppelationRepository.CreateAppelationAsync(appel);

            if (AppelationCreated == null) return BadRequest(ErrorCode.AppelationAlreadyExists);

            return Ok(AppelationCreated);
        }

        /// <summary>
        /// Permet de modifier les infos d'une appellation (Uniquement par l'admin)
        /// </summary>
        /// <param name="appelation">Nom de l'appellation à modifier</param>
        /// <response code = "200">Appellation modifiée : </response>
        /// <response code = "404">Appellation introuvable</response>
        /// <returns>Retourne l'appellation modifier</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateAppelation([FromForm] UpdateAppelationViewModel appelation)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            var appelUpdate = await AppelationRepository.UpdateAppelationAsync(appelation);

            if (appelUpdate == 0) return NotFound(ErrorCode.AppelationNotFound);

            return Ok(appelUpdate);
        }


        /// <summary>
        /// Permet de supprimer une appellation (Uniquement pour l'admin)
        /// </summary>
        /// <param name="appelationId">Id de l'appellation à supprimer</param>
        /// <returns>Retourne l'id de l'appellation supprimer</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAppelation(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            var success = await AppelationRepository.DeleteAppelationAsync(id);

            if (success == 0) NotFound(ErrorCode.AppelationNotFound);

            return Ok(id);
        }
    }
}
