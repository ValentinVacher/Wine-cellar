using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.ViewModel;

namespace Wine_cellar.Repositories
{
    public class DrawerRepository : IDrawerRepository
    {
        //Creation du context et du logger
        WineContext winecontext;
        ILogger<DrawerRepository> logger;

        //Constructeur
        public DrawerRepository(WineContext winecontext, ILogger<DrawerRepository> logger)
        {
            this.winecontext = winecontext;
            this.logger = logger;
        }

        //Permet de recuperer tout les tiroirs avec leur bouteilles
        public async Task<List<Drawer>> GetAllWithWineAsync(ClaimsIdentity identity)
        {
            return await winecontext.Drawers.Include(d => d.Wines).
                Where(c => c.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value)).OrderBy(d => d.Index).ToListAsync();
        }

        //Permet de récuperer un tiroir avec ses bouteilles
        public async Task<Drawer> GetDrawerwithWineAsync(string cellarName, int index, ClaimsIdentity identity)
        {
            return await winecontext.Drawers.Include(d => d.Wines).Include(d=>d.Cellar).
                FirstOrDefaultAsync(d => d.Index == index && d.Cellar.Name.Contains(cellarName) 
                && d.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value));
        }

        //Permet de créer un tiroir si la cave n'est pas pleine
        public async Task<int> AddDrawerAsync(CreateDrawerViewModel createDrawer, ClaimsIdentity identity)
        {
            var Cellar = await winecontext.Cellars.Include(d => d.Drawers)
                .FirstOrDefaultAsync(d => d.Name == createDrawer.CellarName && d.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value));

            if (Cellar == null) return 3;

            //Verification cave pleine
            if (Cellar.IsFull() )
            {
                return 2;
            }

            foreach (Drawer e in Cellar.Drawers)
            {
                if (e.Index >= createDrawer.index)
                {
                    e.Index++;
                }
            }

            Drawer drawer = new()
            {
                Index = createDrawer.index,
                NbBottleMax = createDrawer.NbBottleMax,
                CellarId = Cellar.CellarId
            };

            //Ajoute le tiroir
            winecontext.Drawers.AddAsync(drawer);
            await winecontext.SaveChangesAsync();

            return 1;
        }

        //Permet de modifier un tiroir
        public async Task<Drawer> UpdateDrawerAsync(UpdateDrawerViewModel drawer, ClaimsIdentity identity)
        {
            Drawer DrawerUpdate = await winecontext.Drawers.
                FirstOrDefaultAsync(d => d.Index == drawer.Index && d.Cellar.Name == drawer.CellarName
                && d.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value));

            if (DrawerUpdate == null) return null;

            DrawerUpdate.NbBottleMax = drawer.NbBottleMax;
            DrawerUpdate.Index = drawer.Index;

            await winecontext.SaveChangesAsync();

            return DrawerUpdate;
        }

        //Permet de supprimer un tiroir
        public async Task<bool> DeleteDrawerAsync(string cellarName, int index, ClaimsIdentity identity)
        {
            var DelDrawer = await winecontext.Drawers.Include(w => w.Wines).
                FirstOrDefaultAsync(d => d.Index == index && d.Cellar.Name == cellarName 
                && d.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value));

            if (DelDrawer == null) return false;
            winecontext.Drawers.Remove(DelDrawer);
            await winecontext.SaveChangesAsync();
            var DrawersIndex = await winecontext.Drawers.Where(d => d.CellarId == DelDrawer.CellarId).ToListAsync();
            foreach (var drawerindex in DrawersIndex)
            {
                if (drawerindex.Index>DelDrawer.Index)
                {
                    drawerindex.Index--;
                }
            }
            await winecontext.SaveChangesAsync();
            return true;
        }
    }
}
