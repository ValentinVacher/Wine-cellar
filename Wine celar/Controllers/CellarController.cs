using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public CellarController(IWineRepository Repository, IWebHostEnvironment environment)
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
        public async Task<IActionResult> AddCellar()
        {
            var cellar
        }

    }
}
