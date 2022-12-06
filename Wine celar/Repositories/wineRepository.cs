using Microsoft.EntityFrameworkCore;
using Wine_cellar.ViewModel;
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
        public async Task<List<Wine>> GetApogeeAsync()
        {
            var wines = await GetAllWinesAsync();
            var winess = new List<Wine>();
            foreach (var wine in wines)
            {
                if (wine.IsApogee() == true)
                {
                    winess.Add(wine);

                }
                //else
                //yield return null;
            }
            return winess;

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
            var Drawer = await wineContext.Drawers.Include(d=>d.Wines).FirstOrDefaultAsync(d=>d.DrawerId==wine.DrawerId);
            if (Drawer.IsFull() == true) return null;
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

        public async Task<bool> DeleteWineAsync(int WineId)
        {
            var WineDelete = await GetWineByIdAsync(WineId);
            if (WineDelete == null)
                return false;
            wineContext.Wines.Remove(WineDelete);
            await wineContext.SaveChangesAsync();
            return true;

        }

        public async Task<Wine> MoveAsync(int WineId, int newDrawerId)
        {
            var WineMove = await GetWineByIdAsync(WineId);
            if (WineMove == null) return null;
            WineMove.DrawerId = newDrawerId;
            await wineContext.SaveChangesAsync();
            return WineMove;
        }
        public async Task<Wine> DuplicateAsync(int WineId, int NbrDuplicate)
        {
            var WineDuplicate = await GetWineByIdAsync(WineId);
            var wine = new Wine
            {
                Color = WineDuplicate.Color,
                Appelation = WineDuplicate.Appelation,
                Name = WineDuplicate.Name,
                Year = WineDuplicate.Year,
                Today = DateTime.Now,
                KeepMax = WineDuplicate.KeepMax,
                KeepMin = WineDuplicate.KeepMin,
                DrawerId = WineDuplicate.DrawerId
            };

            var Drawer = await wineContext.Drawers.Include(d => d.Wines).FirstOrDefaultAsync(d => d.DrawerId == wine.DrawerId);
            for (int i = 1; i <= NbrDuplicate; i++)
            {
                if (Drawer.IsFull() == true)
                {
                    return null;
                }
                wineContext.Wines.Add(wine);
                //    await wineContext.SaveChangesAsync();
            }
            await wineContext.SaveChangesAsync();
            return WineDuplicate;

        }

    }
}
