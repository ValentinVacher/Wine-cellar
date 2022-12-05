using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.IRepositories;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WineController : ControllerBase
    {
        readonly IWineRepository wineRepository;
        readonly IWebHostEnvironment environment;
        public WineController(IWineRepository Repository, IWebHostEnvironment environment)
        {
            this.wineRepository = wineRepository;
            this.environment = environment;
        }
    }
}
