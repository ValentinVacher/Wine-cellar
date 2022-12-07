using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        IColorRepository colorRepository;
        public ColorController(IColorRepository colorRepository)
        {
            this.colorRepository = colorRepository;
        }
        [HttpGet]
        public async Task<ActionResult<List<ColorWine>>> GetColorswithAppelation()
        {
            return Ok(await colorRepository.GetColorswithAppelationAsync());
        }
        [HttpGet]
        public async Task<ActionResult<ColorWine>> GetColorwithAppelationByName(string colorname)
        {
            return Ok(await colorRepository.GetColorwithAppelationByNameAsync(colorname));
        }
    }
}
