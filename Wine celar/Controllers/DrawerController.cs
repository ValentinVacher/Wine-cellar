using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;

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
        public async Task<ActionResult<Drawer>> PostDrawer([FromForm] CreateDrawerViewModel createDrawer)
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
        public async Task<ActionResult<Drawer>> UpdateDrawer([FromForm] UpdateDrawerViewModel updatedrawer)
        {
            var drawer= new Drawer 
            {
                DrawerId= updatedrawer.DrawerId,
                Index=updatedrawer.Index,
                NbBottleMax=updatedrawer.NbBottleMax 
            };
            return Ok(await drawerRepository.UpdateDrawerAsync(drawer));
        }

        [HttpDelete]
        public async Task<ActionResult<Drawer>> DeleteDrawer(int cellarId, int index)
        {
            bool success = await drawerRepository.DeleteDrawerAsync(cellarId, index);

            if (success)
                return Ok($"Le tiroir {index} de la cave {cellarId} a été supprimé");
            else
                return Problem($"Erreur lors de la suppression du tiroir");
        }

    }
}
