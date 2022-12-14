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
using Wine_celar.Entities;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserRepository UserRepository;
        public UserController(IUserRepository Repository)
        {
            this.UserRepository = Repository;
        }

        /// <summary>
        /// Permet de voir la liste des utilisateurs
        /// </summary>
        /// <response code = "200">Liste des utilisateurs : </response>
        /// <returns>Retourne une liste comportant tout les utilisateurs</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            return Ok(await UserRepository.GetAllUserAsync());
        }

        /// <summary>
        /// Permet de se connecter 
        /// </summary>
        /// <param name="login">Email</param>
        /// <param name="pwd">Mot de passe</param>
        /// <response code = "200">Connecter</response>
        /// <response code = "400">Erreur de saisie</response>
        /// <returns>Retourne le nom de l'utilisateur connecter</returns>
        [HttpGet("{login}/{pwd}")]
        public async Task<IActionResult> Login(string login, string pwd)
        {
            var userConnected = await UserRepository.LoginAsync(login, pwd);

            if (userConnected == null) return BadRequest(ErrorCode.LoginError);

            List<Claim> claims = new List<Claim>();

            claims.Add(new(ClaimTypes.Email, userConnected.Email));
            claims.Add(new(ClaimTypes.Name, userConnected.LastName));
            claims.Add(new(ClaimTypes.GivenName, userConnected.FirstName));
            claims.Add(new(ClaimTypes.NameIdentifier, userConnected.UserId.ToString()));

            if (userConnected.IsAdmin) claims.Add(new(ClaimTypes.Role, "admin"));
            else claims.Add(new(ClaimTypes.Role, ""));

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok(login);
        }


        /// <summary>
        /// Permet de se deconnecter
        /// </summary>
        /// <response code = "200">Deconnecter</response>
        /// <returns>Retourne Ok</returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok();
        }


        /// <summary>
        /// Permet de créer un compte utilisateur
        /// </summary>
        /// <param name="userView"></param>
        /// <param name="CGU"></param>
        /// <returns>Retourne l'utilisateur créer</returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] CreateUserViewModel userView, bool CGU)
        {
            if (!CGU) return BadRequest(ErrorCode.CGUError);

            CellarRegex regex= new();

            if(!regex.Password.Match(userView.Password).Success) return BadRequest(ErrorCode.InvalidPassword);
            if (!regex.Email.Match(userView.Email).Success) return BadRequest(ErrorCode.InvalidEmail);

            if(userView.IsAdmin)
            {
                var identity = User?.Identity as ClaimsIdentity;

                if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
                if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);
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

            if(!user.IsOlder()) return BadRequest(ErrorCode.MinorError);

            var userCreated = await UserRepository.CreateUserAsync(user);

            if (userCreated == null) return BadRequest(ErrorCode.EmaiAlreadyExists);
            
            return Ok(userCreated);
        }


        /// <summary>
        /// Permet de modifier les infos d'un utilisateur
        /// </summary>
        /// <param name="userView"></param>
        /// <returns>Retour l'utilisateur modifier</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserViewModel userView)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            CellarRegex regex = new();

            if (!regex.Password.Match(userView.Password).Success) return BadRequest(ErrorCode.InvalidPassword);
            if (!regex.Email.Match(userView.Email).Success) return BadRequest(ErrorCode.InvalidEmail);

            var userUpdate = await UserRepository.UpdateUserAsync(userView);

            if (userUpdate == null) return NotFound(ErrorCode.UserNotFound);
            
            return Ok(userUpdate);
        }


        /// <summary>
        /// Permet de supprimer un utilisateur
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retourne l'id de l'utilisateur supprimer</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);
            if (int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value) == id) return BadRequest(ErrorCode.DeleteUserError);

            var success = await UserRepository.DeleteUserAsync(id);

            if (success != 0) return Ok(id);
            
            return BadRequest(ErrorCode.UserNotFound);
        }
    }
}
