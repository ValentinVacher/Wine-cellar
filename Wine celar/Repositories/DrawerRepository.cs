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

        //Constructeur
        public DrawerRepository(WineContext winecontext)
        {
            this.wineContext = winecontext;
        }

        //Permet de recuperer tout les tiroirs avec leur bouteilles
        public async Task<List<GetDrawerViewModel>> GetAllDrawersAsync(int userId)
        {
            var drawers = await wineContext.Drawers.Include(d => d.Wines).ThenInclude(a => a.Appelation).Include(c => c.Cellar)
                .AsNoTracking().Where(c => c.Cellar.UserId == userId).OrderBy(d => d.Index).ToListAsync();
            var drawersView = new List<GetDrawerViewModel>();

            foreach (var drawer in drawers)
            {
                var winesView = new List<GetWineViewModel>();

                foreach (var wine in drawer.Wines)
                {
                    var Wine = Convertor.GetViewWine(wine);
                    winesView.Add(Wine);
                }

                var drawerView = Convertor.GetViewDrawer(drawer, winesView);
                drawersView.Add(drawerView);
            }

            return drawersView;
        }

        //Permet de récuperer un tiroir avec ses bouteilles
        public async Task<GetDrawerViewModel> GetDrawerByIdAsync(int id, int userId)
        {
            var drawer = await wineContext.Drawers.Include(d => d.Wines).ThenInclude(a => a.Appelation).Include(d => d.Cellar)
                .AsNoTracking().FirstOrDefaultAsync(d => d.DrawerId == id && d.Cellar.UserId == userId);

            if (drawer == null) return null;

            var winesView = new List<GetWineViewModel>();

            foreach (var wine in drawer.Wines)
            {
                var Wine = Convertor.GetViewWine(wine);
                winesView.Add(Wine);
            }

            return Convertor.GetViewDrawer(drawer, winesView);
        }

        //Permet de créer un tiroir si la cave n'est pas pleine
        public async Task<int> AddDrawerAsync(CreateDrawerViewModel createDrawer, int userId)
        {
            var Cellar = await wineContext.Cellars.Include(d => d.Drawers)
                .AsNoTracking().FirstOrDefaultAsync(d => d.CellarId == createDrawer.CellarId && d.UserId == userId);

            if (Cellar == null) return 3;

            //Verification cave pleine
            if (Cellar.IsFull()) return 2;

            foreach (Drawer e in Cellar.Drawers) if (e.Index >= createDrawer.Index) e.Index++;

            var drawer = Convertor.CreateDrawer(createDrawer);

            //Ajoute le tiroir
            wineContext.Drawers.AddAsync(drawer);
            await wineContext.SaveChangesAsync();

            return 1;
        }

        //Permet de modifier un tiroir
        public async Task<int> UpdateDrawerAsync(UpdateDrawerViewModel drawer, int userId)
        {
            var drawerToUp = await wineContext.Drawers.Include(d => d.Cellar)
                .AsNoTracking().FirstOrDefaultAsync(d => d.DrawerId == drawer.DrawerId && d.Cellar.UserId == userId); ;

            if (drawerToUp.Cellar.NbDrawerMax < drawer.Index) return 1;

            Drawer drawerReplace = drawerToUp.Cellar.Drawers.FirstOrDefault(d => d.Index == drawer.Index);

            if (drawerReplace != null)
                drawerReplace.Index = drawer.Index;

            return await wineContext.Drawers.Where(d => d.DrawerId == drawer.DrawerId && d.Cellar.UserId == userId).AsNoTracking()
                .ExecuteUpdateAsync(updates => updates
                .SetProperty(d => d.Index, drawer.Index)
                .SetProperty(d => d.NbBottleMax, drawer.NbBottleMax));
        }

        //Permet de supprimer un tiroir
        public async Task<int> DeleteDrawerAsync(int drawerId, int userId)
        {
            await wineContext.Wines.AsNoTracking().Where(w => w.DrawerId == drawerId && w.Drawer.Cellar.UserId == userId).ExecuteDeleteAsync();

            return await wineContext.Drawers.AsNoTracking()
               .Where(w => w.DrawerId == drawerId && w.Cellar.UserId == userId).ExecuteDeleteAsync();
        }
    }
}
