using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;
using System.Security.Claims;

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
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return Problem("Vous devez être connecter");

            return Ok(await drawerRepository.GetAllWithWineAsync(identity));
        }

        [HttpGet]
        public async Task<ActionResult<Drawer>> GetDrawerByIndex(string cellarName, int index)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return Problem("Vous devez être connecter");

            if (await drawerRepository.GetDrawerwithWineAsync(cellarName, index, identity) == null) return Problem("Le tiroir est introuvable");

            return Ok(await drawerRepository.GetDrawerwithWineAsync(cellarName, index, identity));
        }

        [HttpPost]
        public async Task<ActionResult<Drawer>> PostDrawer(CreateDrawerViewModel createDrawer)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            Drawer drawer = new()
            {
                Index = createDrawer.index,
                NbBottleMax = createDrawer.NbBottleMax,
            };
            var DrawerCreated = await drawerRepository.AddDrawerAsync(createDrawer, identity);
            if (DrawerCreated == 1) return Ok(DrawerCreated);
        
            else if(DrawerCreated == 2)  return Problem("Tiroir non créer, la cave est pleine");

            else return Problem("Cave non trouvé");
        }

        [HttpPut]
        public async Task<ActionResult<Drawer>> UpdateDrawer(UpdateDrawerViewModel updatedrawer)
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
