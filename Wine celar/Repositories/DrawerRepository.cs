using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Tools;
using Wine_cellar.ViewModel;

namespace Wine_cellar.Repositories
{
    public class DrawerRepository : IDrawerRepository
    {
        //Creation du context et du logger
        WineContext wineContext;
        ILogger<DrawerRepository> logger;

        //Constructeur
        public DrawerRepository(WineContext winecontext, ILogger<DrawerRepository> logger)
        {
            this.wineContext = winecontext;
            this.logger = logger;
        }

        //Permet de recuperer tout les tiroirs avec leur bouteilles
        public async Task<List<Drawer>> GetAllsAsync(int userId)
        {
            return await wineContext.Drawers.Include(d => d.Wines).
                Where(c => c.Cellar.UserId == userId).OrderBy(d => d.Index).ToListAsync();
        }

        //Permet de récuperer un tiroir avec ses bouteilles
        public async Task<Drawer> GetDrawerByIdAsync(int id, int userId)
        {
            return await wineContext.Drawers.Include(d => d.Wines).Include(d => d.Cellar).
                FirstOrDefaultAsync(d => d.DrawerId == id && d.Cellar.UserId == userId);
        }

        //Permet de créer un tiroir si la cave n'est pas pleine
        public async Task<int> AddDrawerAsync(CreateDrawerViewModel createDrawer, int userId)
        {
            var Cellar = await wineContext.Cellars.Include(d => d.Drawers)
                .FirstOrDefaultAsync(d => d.CellarId == createDrawer.CellarId && d.UserId == userId);

            if (Cellar == null) return 3;

            //Verification cave pleine
            if (Cellar.IsFull()) return 2;

            foreach (Drawer e in Cellar.Drawers) if (e.Index >= createDrawer.index) e.Index++;

            var drawer = Convertor.CreateDrawer(createDrawer);

            //Ajoute le tiroir
            wineContext.Drawers.AddAsync(drawer);
            await wineContext.SaveChangesAsync();

            return 1;
        }

        //Permet de modifier un tiroir
        public async Task<int> UpdateDrawerAsync(UpdateDrawerViewModel drawer, int userId)
        {
            Drawer drawerToUp = await GetDrawerByIdAsync(drawer.DrawerId, userId);

            if (drawerToUp.Cellar.NbDrawerMax < drawer.Index) return 1;

            Drawer drawerReplace = drawerToUp.Cellar.Drawers.FirstOrDefault(d => d.Index == drawer.Index);

            if (drawerReplace != null)
                drawerReplace.Index = drawer.Index;

            return await wineContext.Drawers.Where(d => d.DrawerId == drawer.DrawerId && d.Cellar.UserId == userId).
                ExecuteUpdateAsync(updates => updates
                .SetProperty(d => d.Index, drawer.Index)
                .SetProperty(d => d.NbBottleMax, drawer.NbBottleMax));
        }

        //Permet de supprimer un tiroir
        public async Task<int> DeleteDrawerAsync(int drawerId, int userId)
        {
            await wineContext.Wines.Where(w => w.DrawerId == drawerId && w.Drawer.Cellar.UserId == userId).ExecuteDeleteAsync();

            return await wineContext.Drawers.
               Where(w => w.DrawerId == drawerId && w.Cellar.UserId == userId).ExecuteDeleteAsync();
        }
    }
}
