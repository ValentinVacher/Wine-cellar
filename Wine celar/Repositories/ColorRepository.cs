using Microsoft.EntityFrameworkCore;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

namespace Wine_cellar.Repositories
{
    public class ColorRepository : IColorRepository
    {
        WineContext winecontext;
        ILogger<ColorRepository> logger;
        public ColorRepository(WineContext winecontext, ILogger<ColorRepository> logger)
        {
            this.winecontext = winecontext;
            this.logger = logger;
        }

        public async Task<List<ColorWine>> GetColorswithAppelationAsync()
        {
            return await winecontext.ColorWines.Include(c => c.Appelations).ToListAsync();
        }

        public async Task<ColorWine> GetColorwithAppelationByNameAsync(string ColorName)
        {
            return await winecontext.ColorWines.Include(c => c.Appelations).FirstOrDefaultAsync(c => c.Name == ColorName);
        }
    }
}
