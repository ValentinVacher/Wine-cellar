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
        public async Task<ActionResult<List<Drawer>>> GetAllDrawers()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await drawerRepository.GetAllDrawersAsync(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Drawer>> GetDrawerById(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var drawer = await drawerRepository.GetDrawerByIdAsync(id, userId);

            if (drawer == null) return NotFound(ErrorCode.DrawerNotFound);

            return Ok(drawer);
        }

        [HttpPost]
        public async Task<ActionResult<Drawer>> PostDrawer([FromForm] CreateDrawerViewModel createDrawer)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var DrawerCreated = await drawerRepository.AddDrawerAsync(createDrawer, userId);

            switch (DrawerCreated)
            {
                case 1: return Ok(createDrawer);
                case 2: return NotFound(ErrorCode.NoSpaceError);
                default: return NotFound(ErrorCode.CellarNotFound);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Drawer>> UpdateDrawer([FromForm] UpdateDrawerViewModel updatedrawer)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var drawer = await drawerRepository.UpdateDrawerAsync(updatedrawer, userId);

            if (drawer == 0) return NotFound(ErrorCode.DrawerNotFound);

            return Ok(updatedrawer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Drawer>> DeleteDrawer(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var success = await drawerRepository.DeleteDrawerAsync(id, userId);

            if (success != 0) return Ok(id);
            
            return NotFound(ErrorCode.DrawerNotFound);
        }
    }
}
