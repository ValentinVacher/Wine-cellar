using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

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
            if(wineContext.Users.FirstOrDefaultAsync(e => e.Email == user.Email) != null)
            {
                return null;
            }

            wineContext.Users.Add(user);
            await wineContext.SaveChangesAsync();
            return user;
        }

        //Permet de mettre à jour un user
        public async Task<User> UpdateUserAsync(User user)
        {
            var UserUpdate = await wineContext.Users.FindAsync(user.UserId);
            if (UserUpdate == null)
                return null;
            //Valeur à modifier
            UserUpdate.Email = user.Email;
            UserUpdate.FirstName = user.FirstName;
            UserUpdate.LastName = user.LastName;

            await wineContext.SaveChangesAsync();
            return UserUpdate;
        }

        //Permet de supprimer un user
        public async Task<bool> DeleteUserAsync(int UserId)
        {
            var UserDelete = await wineContext.Users.Include(c => c.Cellars).ThenInclude(d => d.Drawers).ThenInclude(w => w.Wines).FirstOrDefaultAsync(c => c.UserId == UserId);
            if (UserDelete == null)
                return false;
            //Supprime les caves associées
            foreach (var Cellar in UserDelete.Cellars)
            {
                Cellar.DeleteDrawer(wineContext);
                wineContext.Cellars.Remove(Cellar);
            }
            //Supprime user
            wineContext.Users.Remove(UserDelete);
            await wineContext.SaveChangesAsync();
            return true;
        }

        //Permet de se connecter
        public async Task<User?> LoginUser(string login, string pwd)
        {
            var userConnected = await wineContext.Users.FirstOrDefaultAsync(u =>
               u.Email == login && u.Password == pwd
            );

            return userConnected;
        }
    }
}
