using Microsoft.AspNetCore.Mvc;
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
        WineContext wineContext;
        ILogger<AppelationRepository> logger;

        //Constructeur
        public AppelationRepository(WineContext winecontext, ILogger<AppelationRepository> logger)
        {
            this.wineContext = winecontext;
            this.logger = logger;
        }

        public async Task<List<Appelation>> GetAllAppelationsAsync()
        {
            return await wineContext.Appelations.ToListAsync();
        }
        public async Task<Appelation> GetAppelationAsync(string appelationName)
        {
            return await wineContext.Appelations.FirstOrDefaultAsync(p => p.Name == appelationName);
        }
        public async Task<Appelation> CreateAppelationAsync(Appelation appelation)
        {
            if (await wineContext.Appelations.FirstOrDefaultAsync(a => a.Name == appelation.Name) == null) return null;

            wineContext.Appelations.Add(appelation);
            await wineContext.SaveChangesAsync();
            return appelation;
        }

        public async Task<Appelation> UpdateAppelationAsync(Appelation appelation)
        {
            var AppelationUpdate = await GetAppelationAsync(appelation.Name);
            if (AppelationUpdate == null) return null;
            AppelationUpdate.Name = appelation.Name;
            AppelationUpdate.KeepMin = appelation.KeepMin;
            AppelationUpdate.KeepMax = appelation.KeepMax;
            await wineContext.SaveChangesAsync();
            return AppelationUpdate;
        }

        public async Task<int> DeleteAppelationAsync(int appelationId)
        {
            return await wineContext.Appelations.Where(a => a.AppelationId== appelationId).ExecuteDeleteAsync();
        }

        public async Task<List<Appelation>> GetAppelationsByColoAsync(WineColor color)
        {
            var AppelationsColor = await wineContext.Appelations.Where(a => a.Color == color).ToListAsync();
            if (AppelationsColor.Count() == 0) return null;
            return AppelationsColor;
        }
    }
}
