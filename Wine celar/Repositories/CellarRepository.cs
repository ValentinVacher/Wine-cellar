using Microsoft.EntityFrameworkCore;
using Wine_cellar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using System.Security.Claims;

namespace Wine_cellar.Repositories
{

    public class CellarRepository : ICellarRepository
    {
        //Creation du context et du logger
        WineContext winecontext;
        ILogger<CellarRepository> Logger;

        //Constructeur
        public CellarRepository(WineContext winecontext, ILogger<CellarRepository> Logger)
        {
            this.winecontext = winecontext;
            this.Logger = Logger;
        }

        //Recupere une liste de toute les caves
        public async Task<List<Cellar>> GetAllsAsync(ClaimsIdentity identity)
        {
            return await winecontext.Cellars.Include(c => c.Drawers).ThenInclude(d => d.Wines).
                Where(c => c.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value)).ToListAsync();
        }

        //Permet de recuperer une cave avec tout ses elements
        public async Task<List<Cellar>> GetCellarByName(string name, ClaimsIdentity identity)
        {
            return await winecontext.Cellars.Include(c => c.Drawers).ThenInclude(d => d.Wines).
                Where(c => c.Name.Contains(name) && c.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value)).ToListAsync();
        }

        //Permet de rajouter une cave et lui donner un nombre de tiroirs
        public async Task<Cellar> AddCellarAsync(Cellar cellar, int NbrButtleDrawer)
        {
            //Ajoute la cave
            winecontext.Cellars.Add(cellar);
            await winecontext.SaveChangesAsync();
            //Ajoute les tiroirs
            for (int i = 1; i <= cellar.NbDrawerMax; i++)
            {
                winecontext.Drawers.Add(new Drawer { CellarId = cellar.CellarId, Index = i, NbBottleMax = NbrButtleDrawer });
            }
            await winecontext.SaveChangesAsync();
            return cellar;
        }

        //Permet de supprimer une cave et ses tiroirs
        public async Task<bool> DeleteCellarAsync(Cellar cellar)
        {
            if (cellar == null) return false;
            //Supprime les tiroirs
            foreach(var drawer in cellar.Drawers)
            {
                drawer.DeleteWines(winecontext);
                winecontext.Drawers.Remove(drawer);
            }
            //Supprime la cave
            winecontext.Cellars.Remove(cellar);
            await winecontext.SaveChangesAsync();
            return true;
        }

        //Permet de modifier une cave
        public async Task<Cellar> UpdateCellarAsync(Cellar cellar)
        {
            var CellarUpdate = await winecontext.Cellars.FindAsync(cellar.CellarId);
            if (CellarUpdate == null) return null;
            CellarUpdate.Name = cellar.Name;
            CellarUpdate.UserId = cellar.UserId;
            await winecontext.SaveChangesAsync();
            return CellarUpdate;
        }
    }
}

