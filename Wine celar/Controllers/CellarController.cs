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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCellarByName(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cellar = await cellarRepository.GetCellarById(id, userId);

            if (cellar == null) return NotFound($"Cave {id} non trouver");
            return Ok(cellar);
        }

        [HttpPost]
        public async Task<IActionResult> AddCellar([FromForm] CreateCellarViewModel cellarViewModel, int Nbr)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var verif = (await cellarRepository.GetAllsAsync(userId)).FirstOrDefault(x => x.Name == cellarViewModel.Name);

            if (verif != null) return BadRequest("Ce nom est déjà pris");

            var cellar = Convertor.CreateCellar(cellarViewModel);
            cellar.UserId = userId;
            var cellarCreated = await cellarRepository.AddCellarAsync(cellar, Nbr);

            return Ok(cellarCreated);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCellar([FromForm] UpdateCellarViewModel UpCellar)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await cellarRepository.UpdateCellarAsync(UpCellar, userId));
        }

        [HttpDelete("{cellarId}")]
        public async Task<IActionResult> DeleteCellar(int cellarId)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var success = await cellarRepository.DeleteCellarAsync(cellarId, userId);

            if (success != 0) return Ok($"La cave {cellarId} a été supprimé");

            return NotFound("Cave introuvable");
        }
        [HttpPost]
        public async Task<IActionResult> ImportJson([FromForm] string Jfile)
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

        [HttpGet]
        public async Task<IActionResult> ExportJson()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            
            await cellarRepository.ExportJsonAsync();
            return Ok("la serialisation à marcher.");

        }
        
    }
}
