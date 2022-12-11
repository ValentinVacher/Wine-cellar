using System.Security.Claims;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface IUserRepository
    {
        //Permet de recuperer tout les users
        Task<List<User>> GetAllUserAsync();
        //Permet de créer un user 
        Task<User> CreateUserAsync(User user);
        //Permet de mettre à jour les infos du user
        Task<int> UpdateUserAsync(UpdateUserViewModel userView);
        //Permet de supprimer un user
        Task<int> DeleteUserAsync(int UserId);
        //Permet de se connecter en tant que user
        Task<User> LoginUser(string login, string pwd);
    }
}
