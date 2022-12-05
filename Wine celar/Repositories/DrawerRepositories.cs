using Microsoft.EntityFrameworkCore;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

namespace Wine_cellar.Repositories
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
            return await winecontext.Drawers.Include(d=>d.Wines).ToListAsync();
        }
        public async Task<Drawer> GetDrawerwithWineAsync(int id)
        {
            return await winecontext.Drawers.Include(d => d.Wines).FirstOrDefaultAsync(d=>d.DrawerId==id);
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

        public Task<Drawer> DeleteDrawerAsync(int id)
        {
           var DelDrawer= winecontext.Drawers.FindAsync(id);
            winecontext.Drawers.Remove(DelDrawer);
            winecontext.SaveChanges();
            return DelDrawer;
        }



        public Task<Drawer> UpdateDrawerAsync(Drawer drawer)
        {
            throw new NotImplementedException();
        }
    }
}
