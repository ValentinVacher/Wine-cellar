using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DrawerController : ControllerBase
    {
        IDrawerRepository drawerRepository;

        public DrawerController(IDrawerRepository drawerRepository)
        {
            this.drawerRepository = drawerRepository;

        }
        [HttpGet]
        public async Task<ActionResult<List<Drawer>>> GetDrawers()
        {
            return Ok(await drawerRepository.GetAllWithWineAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Drawer>> GetDrawer(int id)
        {
            return Ok(drawerRepository.GetDrawerwithWineAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<Drawer>> PostDrawer(Drawer drawer)
        {
            var DrawerCreated = await drawerRepository.AddDrawerAsync(drawer);
            if (DrawerCreated != null)
            {
                return Ok(DrawerCreated);
            }
            else
            {
                return Problem("tiroir non créer");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Drawer>> UpdateDrawer(Drawer drawer)
        {
            return Ok(await drawerRepository.UpdateDrawerAsync(drawer));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Drawer>> DeleteDrawer(int id)
        {
            return Ok(await drawerRepository.DeleteDrawerAsync(id));
        }

    }
}
