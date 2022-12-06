using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;

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
            return Ok(await UserRepository.GetAllUserAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel userView)
        {
            User user = new()
            {
                FirstName= userView.FirstName,
                LastName= userView.LastName,
                DateOfBirth = userView.DateOfBirth,
                Email= userView.Email,
                Password= userView.Password
            };

            var userCreated = await UserRepository.CreateUserAsync(user);

            if (userCreated == null)
            {
                return Problem("Erreur lors de la création de l'utilisateur");
            }

            return Ok(userCreated);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel userView)
        {
            User user = new()
            {
                UserId = userView.UserId,
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Email = userView.Email,
                Password = userView.Password
            };

            var userUpdate = await UserRepository.UpdateUserAsync(user);

            if (userUpdate == null)
            {
                return Problem("Erreur lors de la mise a jour de l'utilisateur");
            }

            return Ok(userUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool success = await UserRepository.DeleteUserAsync(id);

            if (success)
                return Ok($"L'utilisateur {id} a été supprimé");
            else
                return Problem($"Erreur lors de la suppression de l'utilisateur");
        }
    }
}
