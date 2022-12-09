using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.ViewModel;

namespace Wine_celar.Repositories
{
    public class UserRepository : IUserRepository
    {
        //Declaration du context et logger
        readonly WineContext wineContext;
        ILogger<UserRepository> logger;

        //Constructeur
        public UserRepository(WineContext wineContext, ILogger<UserRepository> logger)
        {
            this.wineContext = wineContext;
            this.logger = logger;
        }

        //Permet de recuperer tout les utilisateurs
        public async Task<List<User>> GetAllUserAsync()
        {
            return await wineContext.Users.ToListAsync(); ;
        }

        //Permet de créer un user
        public async Task<User> CreateUserAsync(User user)
        {
            if(await wineContext.Users.Any(e => e.Email == user.Email)) return null;
          
            wineContext.Users.Add(user);
            await wineContext.SaveChangesAsync();
            return user;
        }

        //Permet de mettre à jour un user
        public async Task<int> UpdateUserAsync(UpdateUserViewModel userView)
        {
            return await wineContext.Users.Where(u => u.UserId == userView.UserId).
                ExecuteUpdateAsync(updates => updates
                .SetProperty(u => u.LastName, userView.LastName)
                .SetProperty(u => u.FirstName, userView.FirstName)
                .SetProperty(u => u.Email, userView.Email)
                .SetProperty(u => u.Password, userView.Password));
        }

        //Permet de supprimer un user
        public async Task<int> DeleteUserAsync(int userId)
        {
            await wineContext.Wines.Where(w => w.Drawer.Cellar.UserId == userId).ExecuteDeleteAsync();
            await wineContext.Drawers.Where(d => d.Cellar.UserId == userId).ExecuteDeleteAsync();
            await wineContext.Cellars.Where(c => c.UserId == userId).ExecuteDeleteAsync();

            return await wineContext.Users.Where(u => u.UserId == userId).ExecuteDeleteAsync();
        }

        //Permet de se connecter
        public async Task<User?> LoginUser(string login, string pwd)
        {
            var userConnected = await wineContext.Users.FirstOrDefaultAsync(u => u.Email == login && u.Password == pwd);

            return userConnected;
        }
    }
}
