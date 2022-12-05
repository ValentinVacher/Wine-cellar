using Wine_celar.Entities;

namespace Wine_celar.IRepositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<User> DeleteUserAsync(int Userid);
        Task<User> LoginUser(string login, string pwd);
    }
}
