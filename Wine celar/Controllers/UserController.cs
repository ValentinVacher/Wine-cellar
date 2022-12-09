using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Security.Principal;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserRepository UserRepository;
        readonly IWebHostEnvironment environment;
        public UserController(IUserRepository Repository, IWebHostEnvironment environment)
        {
            this.UserRepository = Repository;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest("Vous devez être admin");

            return Ok(await UserRepository.GetAllUserAsync());
        }

        [HttpGet("{login}/{pwd}")]
        public async Task<IActionResult> LoginAsync(string login, string pwd)
        {
            var userConnected = await UserRepository.LoginUser(login, pwd);

            if (userConnected == null) return BadRequest($"Erreur lors du login, vérifiez le login ou mot de passe");

            List<Claim> claims = new List<Claim>();

            claims.Add(new(ClaimTypes.Email, userConnected.Email));
            claims.Add(new(ClaimTypes.Name, userConnected.LastName));
            claims.Add(new(ClaimTypes.GivenName, userConnected.FirstName));
            claims.Add(new(ClaimTypes.NameIdentifier, userConnected.UserId.ToString()));

            if (userConnected.IsAdmin)
            {
                claims.Add(new(ClaimTypes.Role, "admin"));
            }
            else
            {
                claims.Add(new(ClaimTypes.Role, ""));
            }

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok($"{userConnected.LastName} logged");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok("Logout");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] CreateUserViewModel userView, bool CGU)
        {
            if (!CGU) return BadRequest("Vous devez accepter les condition generale d'utilisation");

            Regex regexPsw = new(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
            if(!regexPsw.Match(userView.Password).Success) return BadRequest("Mot de passe incorrect");

            Regex regexMail = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regexMail.Match(userView.Email).Success) return BadRequest("Email invalide");

            if(userView.IsAdmin)
            {
                var identity = User?.Identity as ClaimsIdentity;

                if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

                if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest("Vous devez être admin");
            }

            User user = new()
            {
                FirstName= userView.FirstName,
                LastName= userView.LastName,
                DateOfBirth = userView.DateOfBirth,
                Email= userView.Email,
                Password= userView.Password,
                IsAdmin= userView.IsAdmin
            };

            if(!user.IsOlder()) return BadRequest("L'alcool est interdit au moins de 18 ans");

            var userCreated = await UserRepository.CreateUserAsync(user);

            if (userCreated == null) return BadRequest("Cette adresse email est déja associé a un compte");
            
            return Ok(userCreated);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserViewModel userView)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");

            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest("Vous devez être admin");

            User user = new()
            {
                UserId = userView.UserId,
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Email = userView.Email,
                Password = userView.Password
            };

            var userUpdate = await UserRepository.UpdateUserAsync(user);

            if (userUpdate == null) return NotFound("Utilisateur introuvable");
            
            return Ok(userUpdate);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest("Vous devez être connecter");
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest("Vous devez être admin");
            if (int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value) == userId) return BadRequest("Vous ne pouver pas suprimer votre compte");

            var success = await UserRepository.DeleteUserAsync(userId);

            if (success != 0) return Ok($"L'utilisateur {userId} a été supprimé");
            
            return BadRequest("Utilisateur introuvable");
        }
    }
}
