using Microsoft.EntityFrameworkCore;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

namespace Wine_cellar.Repositories
{
    public class DrawerRepository : IDrawerRepository
    {
        WineContext winecontext;
        ILogger<DrawerRepository> logger;
        public DrawerRepository(WineContext winecontext, ILogger<DrawerRepository> logger)
        {
            this.winecontext = winecontext;
            this.logger = logger;
        }
        public async Task<List<Drawer>> GetAllWithWineAsync()
        {
            return await winecontext.Drawers.Include(d => d.Wines).ToListAsync();
        }
        public async Task<Drawer> GetDrawerwithWineAsync(string cellarName, int index)
        {
            return await winecontext.Drawers.Include(d => d.Wines).Include(d=>d.Cellar).FirstOrDefaultAsync(d => d.Index == index && d.Cellar.Name == cellarName);
        }

        public async Task<Drawer> AddDrawerAsync(Drawer drawer)
        {
            var Cellar = await winecontext.Cellars.Include(d => d.Drawers).FirstOrDefaultAsync(d => d.CellarId == drawer.CellarId);
            try
            {
                if (Cellar.IsFull() )
                {
                    return drawer;
                }
                winecontext.Drawers.AddAsync(drawer);
                await winecontext.SaveChangesAsync();
            }
            catch (Exception e)
            {

                logger.LogError(e?.InnerException?.ToString());

                return null;
            }
            return drawer;
        }

        public async Task<Drawer> UpdateDrawerAsync(Drawer drawer)
        {
            var DrawerUpdate = await winecontext.Drawers.FindAsync(drawer.DrawerId);
            if (DrawerUpdate == null) return null;
            DrawerUpdate.NbBottleMax = drawer.NbBottleMax;
            DrawerUpdate.Index = drawer.Index;
            await winecontext.SaveChangesAsync();
            return DrawerUpdate;
        }

        public async Task<bool> DeleteDrawerAsync(int cellarId, int index)
        {
            var DelDrawer = await winecontext.Drawers.Include(w => w.Wines).FirstOrDefaultAsync(d => d.Index == index && d.CellarId == cellarId);
            if (DelDrawer == null) return false;
            winecontext.Drawers.Remove(DelDrawer);
            await winecontext.SaveChangesAsync();
            return true;
        }
    }
}
