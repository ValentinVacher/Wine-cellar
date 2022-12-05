using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;

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
        public async Task<IActionResult> AddCellar(Cellar cellar, int NbrButtleDrawer)
        {
            var cellarCreated = await cellarRepository.AddCellarAsync(cellar,NbrButtleDrawer);
            if (cellar != null)
                return Ok(cellarCreated);
            else
                return Problem("Cave non créer");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCellar(Cellar cellar)
        {
            return Ok(await cellarRepository.UpdateCellarAsync(cellar));

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCellar(int id)
        {
            return Ok(await cellarRepository.DeleteCellarAsync(id));
        }


    }
}
