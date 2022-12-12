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

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await drawerRepository.GetAllsAsync(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Drawer>> GetDrawerByIndex(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            Drawer drawer = await drawerRepository.GetDrawerByIdAsync(id, userId);

            if (drawer == null) return NotFound("Le tiroir est introuvable");

            return Ok(drawer);
        }

        [HttpPost]
        public async Task<ActionResult<Drawer>> PostDrawer([FromForm] CreateDrawerViewModel createDrawer)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var DrawerCreated = await drawerRepository.AddDrawerAsync(createDrawer, userId);

            switch (DrawerCreated)
            {
                case 1: return Ok(createDrawer);
                case 2: return NotFound("Tiroir non créer, la cave est pleine");
                default: return NotFound("Cave non trouvé");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Drawer>> UpdateDrawer([FromForm] UpdateDrawerViewModel updatedrawer)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var drawer = await drawerRepository.UpdateDrawerAsync(updatedrawer, userId);

            if (drawer == 0) return NotFound("Aucun tiroir n'a été modifié");

            return Ok("Le tiroir a été modifié");
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Drawer>> DeleteDrawer(int Id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var success = await drawerRepository.DeleteDrawerAsync(Id, userId);

            if (success != 0) return Ok($"Le tiroir {Id} a été supprimé");
            
            return NotFound($"Le tiroir est introuvable");
        }

    }
}
