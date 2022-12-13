using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Contexts;
using System.Security.Claims;
using Wine_celar.ViewModel;
using System.Text.Json;
using Wine_cellar.Tools;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WineController : ControllerBase
    {
        readonly IWineRepository wineRepository;
        readonly IWebHostEnvironment environment;
        public WineController(IWineRepository Repository, IWebHostEnvironment environment)
        {
            this.wineRepository = Repository;
            this.environment = environment;
        }

        /// <summary>
        /// Permet de voir tout les vins
        /// </summary>
        /// <returns>Retourne une liste de tout les vins</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllWines()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await wineRepository.GetAllWinesAsync(userId));
        }


        /// <summary>
        /// Permet de voir un vin par son id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retourne le vin demander</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWineById(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wine = await wineRepository.GetWineByIdAsync(id, userId);

            if (wine == null) return NotFound(ErrorCode.WineNotFound);

            var WineView = Convertor.GetViewWine(wine);

            return Ok(WineView);
        }


        /// <summary>
        /// Permet de voir les vins à leur apogée
        /// </summary>
        /// <returns>Retourne une liste de vins à leur apogée</returns>
        [HttpGet]
        public async Task<IActionResult> GetWinesByApogee()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) BadRequest(ErrorCode.UnLogError);
                
            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wines = await wineRepository.GetWinesByApogeeAsync(userId);

            if (wines == null) NotFound(ErrorCode.WineNotFound);

            return Ok(wines);
        }


        /// <summary>
        /// Permet de faire une recherche par mot clé
        /// </summary>
        /// <param name="word">Mot clé</param>
        /// <returns>Retourne une liste de vin composé du mot clé saisi</returns>
        [HttpGet("{word}")]
        public async Task<IActionResult> GetWinesByWord(string word)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wine = await wineRepository.GetWinesByWordAsync(word, userId);

            if (wine == null) return NotFound(ErrorCode.WineNotFound);

            var WinesView = new List<GetWineViewModel>();

            foreach (var w in wine)
            { 
                var WineView = Convertor.GetViewWine(w);
                WinesView.Add(WineView);
            }
            
            return Ok(WinesView);
        }


        /// <summary>
        /// Permet de recuperer les vins correspondant à une couleur
        /// </summary>
        /// <param name="color">Couleur rechercher</param>
        /// <returns>Retourne une liste de vins associé à la couleur choisi</returns>
        [HttpGet]
        public async Task<ActionResult<List<Wine>>> GetWinesByColor(WineColor color)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wines = await wineRepository.GetWinesByColorAsync(color, userId);

            if (wines == null) return NotFound(ErrorCode.WineNotFound);

            var WinesView = new List<GetWineViewModel>();

            foreach (var w in wines)
            {
                var WineView = Convertor.GetViewWine(w);
                WinesView.Add(WineView);
            }

            return Ok(WinesView);
        }
        /// <summary>
        /// Permet de créer un vin
        /// </summary>
        /// <param name="wineViewModel"></param>
        /// <returns>Retourne le vin créer</returns>
        [HttpPost]
        public async Task<IActionResult> CreateWineAsync([FromForm]
        CreateWineViewModel wineViewModel)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wineCreated = await wineRepository.CreateWineAsync(wineViewModel, userId);

            switch (wineCreated)
            {
                case 1: return NotFound(ErrorCode.WineNotFound);
                case 2: return BadRequest(ErrorCode.NoSpaceError);
                case 3: return BadRequest(ErrorCode.AppelationError);
                default: break;
            }

            if (!string.IsNullOrEmpty(wineViewModel.Picture?.FileName)
                && wineViewModel.Picture.FileName.Length > 0)
            {
                var path = Path.Combine(environment.WebRootPath, "img/", wineViewModel.Picture.FileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await wineViewModel.Picture.CopyToAsync(stream);
                    stream.Close();
                }
            }

            return Ok(wineViewModel);
        }


        /// <summary>
        /// Permet de dupliquer un vin autant de fois qu'on le souhaite
        /// </summary>
        /// <param name="id">id du vin à dupliquer</param>
        /// <param name="NbrDuplicate">nombre de copie voulu</param>
        /// <returns>Retourne le nombre de vin dupliquer</returns>
        [HttpPost("{id}")]
        public async Task<ActionResult<Wine>> DuplicateWine(int id, int NbrDuplicate)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            int NbrWineCreate = await wineRepository.DuplicateAsync(id, NbrDuplicate, userId);

            return Ok(NbrWineCreate);
        }

      /// <summary>
      /// Permet de modifier les infos d'un vin
      /// </summary>
      /// <param name="wineView"></param>
      /// <returns>Retourne le vin modifier</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateWine([FromForm] UpdateWineViewModel wineView)
        {
            var identity = User?.Identity as ClaimsIdentity;
            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            var UserIdentity = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (await wineRepository.UpdateWineAsync(wineView, UserIdentity) == 0) return NotFound(ErrorCode.WineNotFound);
            return Ok(wineView);
        }


        /// <summary>
        /// Permet de déplacer un vin dans un autre tiroir
        /// </summary>
        /// <param name="id">Id du vin à déplacer</param>
        /// <param name="drawerId">Id du tiroir ou sera deplacer le vin</param>
        /// <returns>Retourne l'id du vin déplacer</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Wine>> MoveWine(int id, int drawerId)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wineMove = await wineRepository.MoveWineAsync(id, drawerId, userId);

            switch (wineMove)
            {
                case 1: return NotFound(ErrorCode.WineNotFound);
                case 2: return NotFound(ErrorCode.DrawerNotFound);
                case 3: return BadRequest(ErrorCode.NoSpaceError);
                default: return Ok(id);
            }
        }


        /// <summary>
        /// Permet de supprimer un vin
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retourne l'id du vin supprimer</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWine(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var success = await wineRepository.DeleteWineAsync(id, userId);

            if (success != 0) return Ok(id);
            return NotFound(ErrorCode.WineNotFound);
        }
    }
}
