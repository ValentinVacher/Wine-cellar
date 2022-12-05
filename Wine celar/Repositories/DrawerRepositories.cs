using Microsoft.EntityFrameworkCore;
using Wine_celar.Contexts;
using Wine_celar.Entities;
using Wine_celar.IRepositories;

namespace Wine_celar.Repositories
{
    public class DrawerRepositories : IDrawerRepository
    {
        WineContext winecontext;
        ILogger<DrawerRepositories> logger;
        public DrawerRepositories(WineContext winecontext, ILogger<DrawerRepositories> logger)
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



        public Task<Drawer> UpdateDrawerAsync(Drawer drawer)
        {
            throw new NotImplementedException();
        }
    }
}
