using Microsoft.EntityFrameworkCore;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

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
        public async Task<List<Drawer>> GetAllWithWineAsync()
        {
            return await winecontext.Drawers.Include(d => d.Wines).OrderBy(d=>d.Index).ToListAsync();
        }

        //Permet de récuperer un tiroir avec ses bouteilles
        public async Task<Drawer> GetDrawerwithWineAsync(string cellarName, int index)
        {
            return await winecontext.Drawers.Include(d => d.Wines).Include(d=>d.Cellar).FirstOrDefaultAsync(d => d.Index == index && d.Cellar.Name == cellarName);
        }

        //Permet de créer un tiroir si la cave n'est pas pleine
        public async Task<Drawer> AddDrawerAsync(Drawer drawer)
        {
            var Cellar = await winecontext.Cellars.Include(d => d.Drawers).FirstOrDefaultAsync(d => d.CellarId == drawer.CellarId);
            try
            {
                //Verification cave pleine
                if (Cellar.IsFull() )
                {
                    return drawer;
                }
                //Ajoute le tiroir
                winecontext.Drawers.AddAsync(drawer);
                await winecontext.SaveChangesAsync();
            }
            //En cas d'erreur
            catch (Exception e)
            {

                logger.LogError(e?.InnerException?.ToString());

                return null;
            }
            return drawer;
        }

        //Permet de modifier un tiroir
        public async Task<Drawer> UpdateDrawerAsync(Drawer drawer)
        {
            var DrawerUpdate = await winecontext.Drawers.FindAsync(drawer.DrawerId);
            if (DrawerUpdate == null) return null;
            DrawerUpdate.NbBottleMax = drawer.NbBottleMax;
            DrawerUpdate.Index = drawer.Index;
            var DrawersCheck= await winecontext.Drawers.Where(d=>d.CellarId==DrawerUpdate.CellarId).ToListAsync();
            foreach (var drawercheck in DrawersCheck)
            {
                if (drawercheck.Index==drawer.Index)
                {
                    return null;
                }
            }
            await winecontext.SaveChangesAsync();
            return DrawerUpdate;
        }

        //Permet de supprimer un tiroir
        public async Task<bool> DeleteDrawerAsync(int cellarId, int index)
        {
            var DelDrawer = await winecontext.Drawers.Include(w => w.Wines).FirstOrDefaultAsync(d => d.Index == index && d.CellarId == cellarId);
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
