using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUserAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int UserId);
        Task<User> LoginUser(string login, string pwd);
    }
}
