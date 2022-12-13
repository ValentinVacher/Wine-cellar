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

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            return Ok(await UserRepository.GetAllUserAsync());
        }

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

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok();
        }

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

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserViewModel userView)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            var userUpdate = await UserRepository.UpdateUserAsync(userView);

            if (userUpdate == null) return NotFound(ErrorCode.UserNotFound);
            
            return Ok(userUpdate);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);
            if (int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value) == userId) return BadRequest(ErrorCode.DeleteUserError);

            var success = await UserRepository.DeleteUserAsync(userId);

            if (success != 0) return Ok(userId);
            
            return BadRequest(ErrorCode.UserNotFound);
        }
    }
}
