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
        public async Task<Drawer> GetDrawerwithWineAsync(int id)
        {
            return await winecontext.Drawers.Include(d => d.Wines).FirstOrDefaultAsync(d => d.DrawerId == id);
        }

        public async Task<Drawer> AddDrawerAsync(Drawer drawer)
        {
            try
            {
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

        public async Task<Drawer> DeleteDrawerAsync(int id)
        {
            var DelDrawer = await winecontext.Drawers.FindAsync(id);
            winecontext.Drawers.Remove(DelDrawer);
            await winecontext.SaveChangesAsync();
            return DelDrawer;
        }



        public async Task<Drawer> UpdateDrawerAsync(Drawer drawer)
        {
            var DrawerUpdate = await winecontext.Drawers.FindAsync(drawer.DrawerId);
            if (DrawerUpdate != null) return null;
            DrawerUpdate.NbBottleMax=drawer.NbBottleMax;
            DrawerUpdate.Index=drawer.Index;
            await winecontext.SaveChangesAsync();
            return DrawerUpdate;
        }
    }
}
