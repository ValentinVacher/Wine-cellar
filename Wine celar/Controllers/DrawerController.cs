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
        public async Task<ActionResult<Drawer>> PostDrawer([FromForm] CreateDrawerViewModel createDrawer)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return Problem("Vous devez être connecter");

            var DrawerCreated = await drawerRepository.AddDrawerAsync(createDrawer, identity);

            switch (DrawerCreated)
            {
                case 1: return Ok(createDrawer);
                case 2: return Problem("Tiroir non créer, la cave est pleine");
                default: return Problem("Cave non trouvé");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Drawer>> UpdateDrawer([FromForm] UpdateDrawerViewModel updatedrawer)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return Problem("Vous devez être connecter");

            var drawer = await drawerRepository.UpdateDrawerAsync(updatedrawer, identity);

            switch(drawer)
            {
                case 1: return Problem("Le tiroir est introuvable");
                case 2: return Problem("Cave introuvable");
                default: return Ok(updatedrawer);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Drawer>> DeleteDrawer(string cellarName, int index)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return Problem("Vous devez être connecter");

            bool success = await drawerRepository.DeleteDrawerAsync(cellarName, index, identity);

            if (success)
                return Ok($"Le tiroir {index} de la cave {cellarName} a été supprimé");
            else
                return Problem($"Erreur lors de la suppression du tiroir");
        }

    }
}
