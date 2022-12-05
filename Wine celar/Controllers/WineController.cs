using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WineController : ControllerBase
    {
        readonly IWineRepository postRepository;
        readonly IWebHostEnvironment environment;
        public PostController(IPostRepository postRepository, IWebHostEnvironment environment)
        {
            this.postRepository = postRepository;
            this.environment = environment;
        }
    }
}
