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

        /// <summary>
        /// Permet de récuperer toutes les appellations
        /// </summary>
        /// <returns>Retourne une liste contenant toutes les appellations existante</returns>
        public async Task<List<Appelation>> GetAllAppelationsAsync()
        {
            return await wineContext.Appelations.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Permet de récuperer une appellation par son Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns>Retourne l'appellation correspondante à l'id saisi</returns>
        public async Task<GetAppelationViewModel> GetAppelationByIdAsync(int id, int userid)
        {
            var appel= await wineContext.Appelations
                .Include(c => c.Wines.Where(w=>w.Drawer.Cellar.UserId==userid)).ThenInclude(w=>w.Drawer).ThenInclude(d=>d.Cellar).
                AsNoTracking().FirstOrDefaultAsync(a => a.AppelationId == id);

            if (appel == null) return null;

            var wines = new List<GetWineViewModel>();

            foreach (var wine in appel.Wines)
            {
                var winemodel = Convertor.GetViewWine(wine);
                wines.Add(winemodel);
            }

            return Convertor.GetAppelation(appel, wines);
        }

        /// <summary>
        /// Permet de récuperer les appellations ayant une couleur correspondante à la couelur saisi
        /// </summary>
        /// <param name="color"></param>
        /// <returns>Retourne une liste d'appellation ayant la couleur saisi</returns>
        public async Task<List<Appelation>> GetAppelationsByColorAsync(WineColor color)
        {
            var AppelationsColor = await wineContext.Appelations.AsNoTracking().Where(a => a.Color == color).ToListAsync();

            if (AppelationsColor.Count() == 0) return null;
            return AppelationsColor;
        }


        /// <summary>
        /// Permet de créer une nouvelle appellation
        /// </summary>
        /// <param name="appelation"></param>
        /// <returns>Retourne l'appellation créer</returns>
        public async Task<Appelation> CreateAppelationAsync(Appelation appelation)
        {
            if (await wineContext.Appelations.AsNoTracking().FirstOrDefaultAsync(a => a.Name == appelation.Name) == null) return null;

            wineContext.Appelations.Add(appelation);
            await wineContext.SaveChangesAsync();

            return appelation;
        }


        /// <summary>
        /// Permet de modifier les infos d'une appellation
        /// </summary>
        /// <param name="appelation"></param>
        /// <returns>Retourne l'appellation créer</returns>
        public async Task<int> UpdateAppelationAsync(UpdateAppelationViewModel appelation)
        {
            return await wineContext.Appelations.AsNoTracking().Where(a => a.AppelationId == appelation.AppelationId).
                ExecuteUpdateAsync(updates => updates
                .SetProperty(a => a.Name, appelation.Name)
                .SetProperty(a => a.KeepMin, appelation.KeepMin)
                .SetProperty(a => a.KeepMax, appelation.KeepMax)
                .SetProperty(a => a.Color, appelation.Color));
        }


        /// <summary>
        /// Permet de supprimer une appellation
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retourne l'appellation supprimer</returns>
        public async Task<int> DeleteAppelationAsync(int id)
        {
            return await wineContext.Appelations.AsNoTracking().Where(a => a.AppelationId == id).ExecuteDeleteAsync();
        }
    }
}
