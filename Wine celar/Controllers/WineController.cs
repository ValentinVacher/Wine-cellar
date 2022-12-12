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

        [HttpGet]
        public async Task<IActionResult> GetAllWines()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await wineRepository.GetAllWinesAsync(userId));
        }

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

        [HttpGet]
        public async Task<IActionResult> GetApogee()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) BadRequest(ErrorCode.UnLogError);
                
            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var wines = await wineRepository.GetApogeeAsync(userId);

            if (wines == null) NotFound(ErrorCode.WineNotFound);

            return Ok(wines);
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> GetWineByWord(string word)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wine = await wineRepository.GetWineByWordAsync(word, userId);

            if (wine == null) return NotFound(ErrorCode.WineNotFound);

            var WinesView = new List<GetWineViewModel>();
            foreach (var w in wine)
            {
                var WineView = Convertor.GetViewWine(w);
            }

            return Ok(WinesView);
        }
        [HttpGet]
        public async Task<ActionResult<List<Wine>>> GetWineByColor(WineColor color)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wines = await wineRepository.GetWineByColorAsync(color, userId);

            if (wines == null) return NotFound(ErrorCode.WineNotFound);

            var WinesView = new List<GetWineViewModel>();

            foreach (var w in wines)
            {
                var WineView = Convertor.GetViewWine(w);
                WinesView.Add(WineView);
            }

            return Ok(WinesView);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWineAsync([FromForm]
        CreateWineViewModel WineViewModel)
        {
            var identity = User?.Identity as ClaimsIdentity;
            var idCurrentUser = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (idCurrentUser == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wineCreated = await wineRepository.CreateWineAsync(WineViewModel, userId);

            switch (wineCreated)
            {
                case 1: return NotFound(ErrorCode.WineNotFound);
                case 2: return BadRequest(ErrorCode.NoSpaceError);
                case 3: return BadRequest(ErrorCode.AppelationError);
                default: break;
            }

            if (!string.IsNullOrEmpty(WineViewModel.Picture?.FileName)
                && WineViewModel.Picture.FileName.Length > 0)
            {
                var path = Path.Combine(environment.WebRootPath, "img/",
                    WineViewModel.Picture.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await WineViewModel.Picture.CopyToAsync(stream);
                    stream.Close();
                }
            }

            return Ok(WineViewModel);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Wine>> Duplicate(int id, int NbrDuplicate)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            int NbrWineCreate = await wineRepository.DuplicateAsync(id, NbrDuplicate, userId);

            return Ok(NbrWineCreate);
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateWine([FromForm] UpdateWineViewModel wineView)
        //{
        //    var identity = User?.Identity as ClaimsIdentity;

        //    if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

        //    Drawer userId = Drawer.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
        //    var wineUpdate = await wineRepository.UpdateWineAsync(wineView, userId);

        //    if (wineUpdate == null) return NotFound("Bouteille introuvable");

        //    return Ok(wineUpdate);
        //}

        [HttpPut("{id}")]
        public async Task<ActionResult<Wine>> Move(int id, int drawerId)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wineMove = await wineRepository.MoveAsync(id, drawerId, userId);

            switch (wineMove)
            {
                case 1: return NotFound(ErrorCode.WineNotFound);
                case 2: return NotFound(ErrorCode.DrawerNotFound);
                case 3: return BadRequest(ErrorCode.NoSpaceError);
                default: return Ok(id);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWine([FromForm] UpdateWineViewModel wineView)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            var UserIdentity = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (await wineRepository.UpdateWineAsync(wineView, UserIdentity) == 0) return NotFound(ErrorCode.WineNotFound);

            return Ok(wineView);

        }

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

        //Controller méthode ef7 execute delete
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteWineEF(Drawer id)
        //{

        //    var identity = User?.Identity as ClaimsIdentity;

        //    if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

        //    Drawer userId = Drawer.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    if (await wineRepository.DeleteWineAsync(id,userId) == 0) return NotFound("Vin introuvable");
        //    return Ok($"le Vin {id} demandé a été supprimé");

        //}
    }
}
