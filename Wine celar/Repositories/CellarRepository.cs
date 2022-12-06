using Microsoft.EntityFrameworkCore;
using Wine_cellar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.ViewModel;

namespace Wine_cellar.Repositories
{

    public class CellarRepository : ICellarRepository
    {
        WineContext winecontext;
        ILogger<CellarRepository> Logger;
        public CellarRepository(WineContext winecontext, ILogger<CellarRepository> Logger)
        {
            this.winecontext = winecontext;
            this.Logger = Logger;
        }
        public async Task<List<Cellar>> GetAllsAsync()
        {
            return await winecontext.Cellars.Include(c => c.Drawers).ThenInclude(d => d.Wines).ToListAsync();
        }
        public async Task<Cellar> GetCellarWithAllAsync(int id)
        {
            return await winecontext.Cellars.Include(c => c.Drawers).ThenInclude(d => d.Wines).FirstOrDefaultAsync(c => c.CellarId == id);
        }
        public async Task<Cellar> AddCellarAsync(Cellar cellar, int NbrButtleDrawer)
        {
            winecontext.Cellars.Add(cellar);
            await winecontext.SaveChangesAsync();
            for (int i = 1; i <= cellar.NbDrawerMax; i++)
            {
                winecontext.Drawers.Add(new Drawer { CellarId = cellar.CellarId, NbBottleMax = NbrButtleDrawer });
            }
            await winecontext.SaveChangesAsync();
            return cellar;

        }

        public async Task<bool> DeleteCellarAsync(int id)
        {
            var DelCellar = await GetCellarWithAllAsync(id);
            if (DelCellar == null) return false;
            foreach(var drawer in DelCellar.Drawers)
            {
                drawer.DeleteWines(winecontext);
                winecontext.Drawers.Remove(drawer);
            }
            winecontext.Cellars.Remove(DelCellar);
            await winecontext.SaveChangesAsync();
            return true;
        }



        public async Task<Cellar> UpdateCellarAsync(Cellar cellar)
        {
            var CellarUpdate = await winecontext.Cellars.FindAsync(cellar.CellarId);
            if (CellarUpdate == null) return null;
            CellarUpdate.Name = cellar.Name;
            await winecontext.SaveChangesAsync();
            return CellarUpdate;
        }
    }
}

