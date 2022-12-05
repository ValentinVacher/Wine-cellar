using Microsoft.EntityFrameworkCore;
using Wine_celar.Contexts;
using Wine_celar.Entities;
using Wine_celar.IRepositories;

namespace Wine_celar.Repositories
{
    public class wineRepository : IWineRepository
    {
        readonly WineContext wineContext;
        ILogger logger;
        public wineRepository(WineContext wineContext, ILogger logger)
        {
            this.wineContext = wineContext;
            this.logger = logger;
        }

        public async Task<List<Wine>> GetAllWineAsync()
        {
            return await wineContext.Wines.ToListAsync();
        }
        public async Task<Wine> GetWineByIdAsync(int wineId)
        {
            return await wineContext.Wines.Include(p => p.Name).FirstOrDefaultAsync(p => p.WineId == wineId);
        }

        public

    }
}
