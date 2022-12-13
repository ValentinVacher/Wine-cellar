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


        /// <summary>
        /// Permet de récupérer touts les tiroirs de l'utilisateur
        /// </summary>
        /// <response code = "200">Tiroirs et contenu : </response>
        /// <returns>Retourne touts les tiroirs et leur contenu</returns>
        [HttpGet]
        public async Task<ActionResult<List<Drawer>>> GetDrawers()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await drawerRepository.GetAllsAsync(userId));
        }


        /// <summary>
        /// Permet de voir un tiroir par recherche d'id
        /// </summary>
        /// <param name="id"></param>
        /// <response code ="200">Tiroir et contenu : </response>
        /// <response code = "404">Tiroir introuvable</response>
        /// <returns>Retourne le tiroirs correspondant à l'id saisi</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Drawer>> GetDrawerByIndex(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var drawer = await drawerRepository.GetDrawerByIdAsync(id, userId);

            if (drawer == null) return NotFound(ErrorCode.DrawerNotFound);

            return Ok(drawer);
        }


        /// <summary>
        /// Permet de créer un tiroir
        /// </summary>
        /// <param name="createDrawer"></param>
        /// <response code = "200">Tiroir créer : </response>
        /// <returns>Retourne le tiroir créer</returns>
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


        /// <summary>
        /// Permet de modifier les informations d'un tiroir 
        /// </summary>
        /// <param name="updatedrawer"></param>
        /// <response code = "200">Tiroir modifié : </response> 
        /// <response code = "404">Tiroir introuvable</response>
        /// <returns>Retourne le tiroir modifier</returns>
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


        /// <summary>
        /// Permet de supprimer un tiroir
        /// </summary>
        /// <param name="id"></param>
        /// <response code = "200">Tiroir supprimé</response>
        /// <response code = "404">Tiroir introuvable</response>
        /// <returns>Retourne l'id du tiroir supprimer </returns>
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
