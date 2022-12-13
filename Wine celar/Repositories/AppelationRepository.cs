using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wine_celar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;
using Wine_cellar.Tools;
using Wine_cellar.ViewModel;

namespace Wine_celar.Repositories
{
    public class AppelationRepository : IAppelationRepository
    {
        WineContext wineContext;

        //Constructeur
        public AppelationRepository(WineContext winecontext)
        {
            this.wineContext = winecontext;
        }

        public async Task<List<Appelation>> GetAllAppelationsAsync()
        {
            return await wineContext.Appelations.AsNoTracking().ToListAsync();
        }
        public async Task<GetAppelationViewModel> GetAppelationByIdAsync(int id, int userid)
        {
            var appel= await wineContext.Appelations.Include(c => c.Wines.Where(w=>w.Drawer.Cellar.UserId==userid)).ThenInclude(w=>w.Drawer).ThenInclude(d=>d.Cellar).
                AsNoTracking().FirstOrDefaultAsync(a => a.AppelationId == id);
            var wines = new List<GetWineViewModel>();
            foreach (var wine in appel.Wines)
            {
                var winemodel = Convertor.GetViewWine(wine);
                wines.Add(winemodel);
            }
            return Convertor.GetAppelation(appel, wines);
        }

        public async Task<List<Appelation>> GetAppelationsByColoAsync(WineColor color)
        {
            var AppelationsColor = await wineContext.Appelations.AsNoTracking().Where(a => a.Color == color).ToListAsync();
            if (AppelationsColor.Count() == 0) return null;
            return AppelationsColor;
        }

        public async Task<Appelation> CreateAppelationAsync(Appelation appelation)
        {
            if (await wineContext.Appelations.AsNoTracking().FirstOrDefaultAsync(a => a.AppelationId == appelation.AppelationId) == null) return null;

            wineContext.Appelations.Add(appelation);
            await wineContext.SaveChangesAsync();

            return appelation;
        }

        public async Task<int> UpdateAppelationAsync(UpdateAppelationViewModel appelation)
        {
            return await wineContext.Appelations.AsNoTracking().Where(a => a.AppelationId == appelation.AppelationId).
                ExecuteUpdateAsync(updates => updates
                .SetProperty(a => a.Name, appelation.Name)
                .SetProperty(a => a.KeepMin, appelation.KeepMin)
                .SetProperty(a => a.KeepMax, appelation.KeepMax)
                .SetProperty(a => a.Color, appelation.Color));
        }

        public async Task<int> DeleteAppelationAsync(int appelationId)
        {
            return await wineContext.Appelations.AsNoTracking().Where(a => a.AppelationId == appelationId).ExecuteDeleteAsync();
        }
    }
}
