using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

namespace Wine_celar.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly WineContext wineContext;

        public UserRepository(WineContext wineContext)
        {
            this.wineContext = wineContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            wineContext.Users.Add(user);
            await wineContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteUserAsync(int Userid)
        {
            var UserDelete = await wineContext.Users.FindAsync(Userid);
            if (UserDelete != null)
                return null;
            foreach (var Cellar in UserDelete.Cellars)
            {
                wineContext.Cellars.Remove(Cellar);
            }
            wineContext.Users.Remove(UserDelete);
            await wineContext.SaveChangesAsync();
            return UserDelete;
        }

        public async Task<User> LoginUser(string login, string pwd)
        {
            var userConnected = await wineContext.Users
                .FirstOrDefaultAsync(u =>
                u.Email == login && u.Password == pwd);

            if (userConnected == null)
                return null;
            if (userConnected.IsOlder())
            {
                await wineContext.SaveChangesAsync();
                return userConnected;
            }
            return userConnected;


        }

        public Task<User> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
