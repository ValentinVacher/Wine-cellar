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

        [HttpGet]
        public async Task<IActionResult> GetAlls()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await cellarRepository.GetAllsAsync(userId));
        }

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

        [HttpPost]
        public async Task<IActionResult> AddCellar([FromForm] CreateCellarViewModel cellarViewModel, int Nbr)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var verif = (await cellarRepository.GetAllsAsync(userId)).FirstOrDefault(x => x.Name == cellarViewModel.Name);

            if (verif != null) return BadRequest(ErrorCode.CellarAlreadyExists);

            var cellar = Convertor.CreateCellar(cellarViewModel);
            cellar.UserId = userId;
            var cellarCreated = await cellarRepository.AddCellarAsync(cellar, Nbr);

            return Ok(cellarCreated);
        }

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
        [HttpPost]
        public async Task<IActionResult> ImportJson([FromForm] string Jfile)
        {
            var path = Path.Combine(environment.ContentRootPath, "Json\\", Jfile + ".json");
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                StreamReader reader = new StreamReader(stream);
                var file = reader.ReadToEnd();
                
                //var deserial = JsonConvert.DeserializeObject<string>(file);

                await cellarRepository.ImportJsonAsync(file);
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

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            await cellarRepository.ExportJsonAsync();

            return Ok();
        }
        
    }
}
