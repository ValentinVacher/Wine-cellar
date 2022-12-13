using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using System.Security.Claims;
using Wine_celar.ViewModel;
using Wine_cellar.Repositories;
using System.Text.Json;
using Wine_cellar.Tools;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

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
        /// <summary>
        /// Retourne toutes les caves et leur elements
        /// </summary>
        /// 
        /// <response code = "200">Vos caves : </response>
        /// <response code = "404">Cave non trouvé</response>
        /// <returns>Retourne toutes les caves de l'utilisateur</returns>
        [HttpGet]
        public async Task<IActionResult> GetAlls()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await cellarRepository.GetAllsAsync(userId));
        }

        /// <summary>
        /// Permet de voir une cave par son id
        /// </summary>
        /// <param name="id"></param>
        /// <response code = "200">Votre cave : </response>
        /// <response code = "404">La cave choisi n'existe pas</response>
        /// <returns>Retourne la cave avec l'id saisi</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCellarById(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cellar = await cellarRepository.GetCellarById(id, userId);

            if (cellar == null) return NotFound(ErrorCode.CellarNotFound);
            return Ok(cellar);
        }


        /// <summary>
        /// Permet d'ajouter une cave
        /// </summary>
        /// <param name="NbBottle">Nombre bouteilles par tiroir</param>
        /// <response code = "200">La cave a bien été créer</response>
        /// <returns>retourne la cave créer</returns>
        [HttpPost]
        public async Task<IActionResult> AddCellar([FromForm] CreateCellarViewModel cellarViewModel, int NbBottle)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var verif = (await cellarRepository.GetAllsAsync(userId)).FirstOrDefault(x => x.Name == cellarViewModel.Name);

            if (verif != null) return BadRequest(ErrorCode.CellarAlreadyExists);

            var cellar = Convertor.CreateCellar(cellarViewModel);
            cellar.UserId = userId;
            var cellarCreated = await cellarRepository.AddCellarAsync(cellar, NbBottle);

            return Ok(cellarCreated);
        }

        /// <summary>
        /// Permet de modifier les infos d'une cave
        /// </summary>
        /// <param name="upCellar"></param>
        /// <response code = "200">Cave modifié</response>
        /// <returns>Retourne la cave modifié</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCellar([FromForm] UpdateCellarViewModel upCellar)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var update = await cellarRepository.UpdateCellarAsync(upCellar, userId);

            if (update != 0) return Ok(upCellar);

            return NotFound(ErrorCode.CellarNotFound);
        }

        /// <summary>
        /// Permet de supprimer une cave en saisissant son id
        /// </summary>
        /// <param name="cellarId"></param>
        ///  <response code = "200">Cave supprimé</response>
        ///  <response code ="404">Cave introuvable</response>
        /// <returns>Retourne la cave supprimer</returns>
        [HttpDelete("{cellarId}")]
        public async Task<IActionResult> DeleteCellar(int cellarId)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var success = await cellarRepository.DeleteCellarAsync(cellarId, userId);

            if (success != 0) return Ok(cellarId);

            return NotFound(ErrorCode.CellarNotFound);
        }
        
        /// <summary>
        /// Permet de récuperer un fichier Json pour l'ajouter à la base
        /// </summary>
        /// <param name="Jfile"> Nom du fichier </param>
        /// <response code = "200">Fichier ajouter à la base de donnée</response>
        /// <returns>Retourne Ok</returns>
        [HttpPost]
        public async Task<IActionResult> ImportJson([FromForm] string Jfile)
        {
            var path = Path.Combine(environment.ContentRootPath, "Json\\", Jfile + ".json");
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                StreamReader reader = new StreamReader(stream);
                var file = reader.ReadToEnd();
                
                await cellarRepository.ImportJsonAsync(file);
                stream.Close();
            }
            return Ok();
        }
        /// <summary>
        /// Permet d'exporter un fichier Json avec toutes nos caves
        /// </summary>
        /// <param name="name">Nom du fichier créer</param>
        /// <response code = "200">Fichier Json créer</response>
        /// <returns>Retourne OK</returns>
        [HttpGet]
        public async Task<IActionResult> ExportJson(string name)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            await cellarRepository.ExportJsonAsync(name);

            return Ok();
        }
        
    }
}
