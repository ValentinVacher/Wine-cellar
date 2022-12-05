using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_celar.ViewModel;
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
        [HttpGet]
        public async Task<ActionResult<Drawer>> GetDrawer(string cellarName, int index)
        {
            return Ok(await drawerRepository.GetDrawerwithWineAsync(cellarName, index));
        }

        [HttpPost]
        public async Task<ActionResult<Drawer>> PostDrawer(CreateDrawerViewModel createDrawer)
        {
            Drawer drawer = new()
            {
                Index = createDrawer.index,
                NbBottleMax=createDrawer.NbBottleMax,
                CellarId=createDrawer.CellarId

            };
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
        [HttpPut]
        public async Task<ActionResult<Drawer>> UpdateDrawer(Drawer drawer)
        {
            return Ok(await drawerRepository.UpdateDrawerAsync(drawer));
        }
        [HttpDelete]
        public async Task<ActionResult<Drawer>> DeleteDrawer(string cellarName, int index)
        {
            return Ok(await drawerRepository.DeleteDrawerAsync(cellarName, index));
        }

    }
}
