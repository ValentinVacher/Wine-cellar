using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;

namespace Wine_celar.Repositories
{
    public class AppelationRepository : IAppelationRepository
    {
        WineContext winecontext;
        ILogger<AppelationRepository> logger;

        //Constructeur
        public AppelationRepository(WineContext winecontext, ILogger<AppelationRepository> logger)
        {
            this.winecontext = winecontext;
            this.logger = logger;
        }

        public async Task<List<Appelation>> GetAllAppelationsAsync()
        {
            return await winecontext.Appelations.ToListAsync();
        }
        public async Task<Appelation> GetAppelationAsync(string appelationName)
        {
            return await winecontext.Appelations.FirstOrDefaultAsync(p => p.AppelationName == appelationName);
        }
        public async Task<Appelation> CreateAppelationAsync(Appelation appelation)
        {
            winecontext.Appelations.Add(appelation);
            await winecontext.SaveChangesAsync();
            return appelation;
        }


        public async Task<Appelation> UpdateAppelationAsync(Appelation appelation)
        {
            var AppelationUpdate = await GetAppelationAsync(appelation.AppelationName);
            if (AppelationUpdate == null) return null;
            AppelationUpdate.AppelationName = appelation.AppelationName;
            AppelationUpdate.KeepMin = appelation.KeepMin;
            AppelationUpdate.KeepMax = appelation.KeepMax;
            await winecontext.SaveChangesAsync();
            return AppelationUpdate;
        }

    public async Task<Appelation> DeleteAppelationAsync(string appelationName)
        {
            var AppelationDelete = await GetAppelationAsync(appelationName);
            if (AppelationDelete == null)
                return null;
            winecontext.Appelations.Remove(AppelationDelete);
            await winecontext.SaveChangesAsync();
            return AppelationDelete;
        }

      

        public async Task<List<Appelation>> GetAppelationsByColoAsync(WineColor color)
        {
            return await winecontext.Appelations.Where(a => a.Color == color).ToListAsync();
        }
    }
}
