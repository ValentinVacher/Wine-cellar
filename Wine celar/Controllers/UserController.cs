using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.IRepositories;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IWineRepository UserRepository;
        readonly IWebHostEnvironment environment;
        public UserController(IWineRepository Repository, IWebHostEnvironment environment)
        {
            this.UserRepository = UserRepository;
            this.environment = environment;
        }
    }
}
