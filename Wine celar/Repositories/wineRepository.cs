using Microsoft.EntityFrameworkCore;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

namespace Wine_cellar.Repositories
{
    public class WineRepository : IWineRepository
    {
        readonly WineContext wineContext;
        ILogger<WineRepository> logger;
        public WineRepository(WineContext wineContext, ILogger<WineRepository> logger)
        {
            this.wineContext = wineContext;
            this.logger = logger;
        }

        public async Task<List<Wine>> GetAllWinesAsync()
        {
            return await wineContext.Wines.ToListAsync();
        }
        public async Task<Wine> GetWineByIdAsync(int wineId)
        {
            return await wineContext.Wines.FirstOrDefaultAsync(p => p.WineId == wineId);
        }

        //public async Task<List<Wine>> GetWineByNameAsync(string name)
        //{
        //    return await wineContext.Wines.Where(p => p.Name == name).ToListAsync();
        //}
        //public async Task<List<Wine>> GetWineByColorAsync(string color)
        //{
        //    return await wineContext.Wines.Where(p => p.Color == color).ToListAsync();
        //}

        public async Task<List<Wine>> GetWineByWordAsync(string word)
        {
            return await wineContext.Wines.Where(w => w.Color.Contains(word) || w.Appelation.Contains(word) || w.Name.Contains(word)).ToListAsync();
        }
        public async Task<Wine> CreateWineAsync(Wine wine)
        {

            wineContext.Wines.Add(wine);
            await wineContext.SaveChangesAsync();
            return wine;
        }

        public async Task<Wine> UpdateWineAsync(Wine wine)
        {
            var WineUpdate = await GetWineByIdAsync(wine.WineId);
            if (WineUpdate == null) return null;
            WineUpdate.Name = wine.Name;
            WineUpdate.Color = wine.Color;
            WineUpdate.Appelation = wine.Appelation;
            await wineContext.SaveChangesAsync();
            return WineUpdate;
        }

        public async Task<Wine> DeleteWineAsync(int WineId)
        {
            var WineDelete = await GetWineByIdAsync(WineId);
            if (WineDelete == null)
                return WineDelete;
            wineContext.Wines.Remove(WineDelete);
            await wineContext.SaveChangesAsync();
            return WineDelete;

        }

    }
}
