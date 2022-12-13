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

        //Constructeur
        public UserRepository(WineContext wineContext)
        {
            this.wineContext = wineContext;
        }

        /// <summary>
        /// Permet de récuperer tout les utilisateurs
        /// </summary>
        /// <returns>Retourne une liste des utilisateurs existant</returns>
        public async Task<List<User>> GetAllUserAsync()
        {
            return await wineContext.Users.ToListAsync(); ;
        }

        /// <summary>
        /// Permet de se connecter
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pwd"></param>
        /// <returns>Retourne le nom de la personne connecté</returns>
        public async Task<User?> LoginAsync(string login, string pwd)
        {
            var userConnected = await wineContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == login && u.Password == pwd);

            return userConnected;
        }

        /// <summary>
        /// Permet de Créer un utilisateur
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Retourne l'utilisateur créer</returns>
        public async Task<User> CreateUserAsync(User user)
        {
            if(wineContext.Users.AsNoTracking().Any(e => e.Email == user.Email)) return null;
          
            wineContext.Users.Add(user);
            await wineContext.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Permet de modifier les informations d'un utilisateur 
        /// </summary>
        /// <param name="userView"></param>
        /// <returns>Retourne l'utilisateur avec ses données modifier</returns>
        public async Task<int> UpdateUserAsync(UpdateUserViewModel userView)
        {
            return await wineContext.Users.AsNoTracking().Where(u => u.UserId == userView.UserId)
                .ExecuteUpdateAsync(updates => updates
                .SetProperty(u => u.LastName, userView.LastName)
                .SetProperty(u => u.FirstName, userView.FirstName)
                .SetProperty(u => u.Email, userView.Email)
                .SetProperty(u => u.Password, userView.Password));
        }

        /// <summary>
        /// Permet de supprimer un utilisateur
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Retourne l'utilisateur supprimé</returns>
        public async Task<int> DeleteUserAsync(int userId)
        {
            await wineContext.Wines.AsNoTracking().Where(w => w.Drawer.Cellar.UserId == userId).ExecuteDeleteAsync();
            await wineContext.Drawers.AsNoTracking().Where(d => d.Cellar.UserId == userId).ExecuteDeleteAsync();
            await wineContext.Cellars.AsNoTracking().Where(c => c.UserId == userId).ExecuteDeleteAsync();

            return await wineContext.Users.Where(u => u.UserId == userId).ExecuteDeleteAsync();
        }
    }
}
