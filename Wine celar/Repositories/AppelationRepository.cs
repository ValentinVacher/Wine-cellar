using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wine_celar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;
using Wine_cellar.ViewModel;

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
        public async Task<Appelation> GetAppelationAsync(int id)
        {
            return await wineContext.Appelations.FirstOrDefaultAsync(p => p.AppelationId == id);
        }
        public async Task<Appelation> CreateAppelationAsync(Appelation appelation)
        {
            if (await wineContext.Appelations.FirstOrDefaultAsync(a => a.AppelationId == appelation.AppelationId) == null) return null;

            wineContext.Appelations.Add(appelation);
            await wineContext.SaveChangesAsync();
            return appelation;
        }

        public async Task<int> UpdateAppelationAsync(UpdateAppelationViewModel appelation)
        {
            return await wineContext.Appelations.Where(a => a.AppelationId == appelation.AppelationId).
                ExecuteUpdateAsync(updates => updates
                .SetProperty(a => a.Name, appelation.Name)
                .SetProperty(a => a.KeepMin, appelation.KeepMin)
                .SetProperty(a => a.KeepMax, appelation.KeepMax)
                .SetProperty(a => a.Color, appelation.Color));
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
