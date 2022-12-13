using Microsoft.EntityFrameworkCore;
using Wine_cellar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Wine_celar.ViewModel;
using Microsoft.Identity.Client;
using Wine_cellar.Tools;

namespace Wine_cellar.Repositories
{
    public class WineRepository : IWineRepository
    {
        //Declaration du context et du logger
        readonly WineContext wineContext;

        //Constructeur
        public WineRepository(WineContext wineContext)
        {
            this.wineContext = wineContext;
        }

        //Permet de recuperer tout les vins dans une liste
        public async Task<List<GetWineViewModel>> GetAllWinesAsync(int userId)
        {
            var wines = await wineContext.Wines.Include(w => w.Appelation).Include(w => w.Drawer).ThenInclude(d => d.Cellar).AsNoTracking()
                .Where(w => w.Drawer.Cellar.UserId == userId).ToListAsync();
            var winesView = new List<GetWineViewModel>();

            foreach (var w in wines)
            {
                var Wine = Convertor.GetViewWine(w);
                winesView.Add(Wine);
            }

            return winesView.ToList();
        }

        //Permet de recuperer tout les vins à leur apogée dans une liste
        public async Task<List<GetWineViewModel>> GetWinesByApogeeAsync(int userId)
        {
            var wines = await wineContext.Wines.Include(w => w.Appelation).Include(d => d.Drawer).ThenInclude(c => c.Cellar).AsNoTracking()
                .Where(w => w.Drawer.Cellar.UserId == userId).ToListAsync();
            var winess = new List<GetWineViewModel>();

            foreach (var w in wines)
            {
                var ToDay = DateTime.Now.Year;
                var max = w.Year + w.Appelation.KeepMax;
                var min = w.Year + w.Appelation.KeepMin;

                if (ToDay >= min && ToDay <= max)
                {
                    var Wine = Convertor.GetViewWine(w);
                    winess.Add(Wine);
                }
            }

            if (winess.Count == 0) return null;
            return winess.OrderBy(w => w.Color).ToList();
        }

        //Permet de recuperer un vin par son id 
        public async Task<Wine> GetWineByIdAsync(int wineId, int userId)
        {
            return await wineContext.Wines.Include(w => w.Appelation).Include(w => w.Drawer).ThenInclude(d => d.Cellar).AsNoTracking()
                .FirstOrDefaultAsync(p => p.WineId == wineId && p.Drawer.Cellar.UserId == userId);
        }

        //Permet de recuperer une liste de vin selon un terme choisi
        public async Task<List<Wine>> GetWinesByWordAsync(string word, int userId)
        {
            return await wineContext.Wines.Include(a => a.Appelation).AsNoTracking()
                .Where(w => (w.Appelation.Name.Contains(word) || w.Name.Contains(word))
                && w.Drawer.Cellar.UserId == userId).OrderBy(w => w.Color).ToListAsync();
        }

        public async Task<List<Wine>> GetWinesByColorAsync(WineColor color, int userId)
        {
            var WinesColor = await wineContext.Wines.Include(w => w.Appelation).Include(d => d.Drawer).ThenInclude(c => c.Cellar).AsNoTracking()
                .Where(w => w.Color == color && w.Drawer.Cellar.UserId == userId).ToListAsync();

            if (WinesColor.Count == 0) return null;
            return WinesColor;
        }

        //Permet de créer/Ajouter un vin si le tiroir n'est pas plein
        public async Task<int> CreateWineAsync(CreateWineViewModel wineView, int userId)
        {
            var Drawer = await wineContext.Drawers.Include(d => d.Wines).AsNoTracking()
                .FirstOrDefaultAsync(d => d.Index == wineView.DrawerId && d.Cellar.UserId == userId);

            if (Drawer == null) return 1;

            //Verifie si le tiroir est plein
            if (Drawer.IsFull() == true) return 2;

            var wine = Convertor.CreateWine(wineView);

            //Vérifie les couleurs du vin et de l'appelation
            if (wine.Color != (await wineContext.Appelations.FindAsync(wine.AppelationId)).Color) return 3;

            //Ajoute le vin 
            wineContext.Wines.Add(wine);
            await wineContext.SaveChangesAsync();
            return 0;
        }

        //Permet de dupliquer un vin si le tiroir n'est pas plein
        public async Task<int> DuplicateAsync(int wineId, int nbrDuplicate, int userId)
        {
            var WineDuplicate = await wineContext.Wines.Include(d => d.Drawer).AsNoTracking()
                .FirstOrDefaultAsync(p => p.WineId == wineId && p.Drawer.Cellar.UserId == userId);
            var nbWine = 0;
            var nbWinInDrawer = WineDuplicate.Drawer.Wines.Count();

            var wine = new Wine
            {
                Color = WineDuplicate.Color,
                AppelationId = WineDuplicate.AppelationId,
                Name = WineDuplicate.Name,
                Year = WineDuplicate.Year,
                DrawerId = WineDuplicate.DrawerId,
                PictureName = WineDuplicate.PictureName
            };

            //Boucle pour le nombre de duplication 
            for (int i = 1; i <= nbrDuplicate; i++)
            {
                //Verifie si le tiroir est plein
                if (nbWinInDrawer == WineDuplicate.Drawer.NbBottleMax) break;

                wineContext.Wines.Add(wine);

                nbWine++;
                nbWinInDrawer++;
            }

            await wineContext.SaveChangesAsync();

            return nbWine;
        }

        //Permet de modifier un vin 
        //public async Task<Wine> UpdateWineAsync(UpdateWineViewModel wine, Drawer userId)
        //{
        //    var WineUpdate = await GetWineByIdAsync(wine.wineId, userId);
        //    if (WineUpdate == null) return null;
        //    WineUpdate.Name = wine.Name;
        //    WineUpdate.Color = wine.Color;
        //    await wineContext.SaveChangesAsync();
        //    return WineUpdate;
        //}

        //Permet de supprimer un vin 
        //public async Task<bool> DeleteWineAsync(Drawer wineId, Drawer userId)
        //{
        //    var WineDelete = await GetWineByIdAsync(wineId, userId);

        //    if (WineDelete == null) return false;

        //    wineContext.wines.Remove(WineDelete);
        //    await wineContext.SaveChangesAsync();

        //    return true;

        //}

        public async Task<int> UpdateWineAsync(UpdateWineViewModel updateWine, int UserId)
        {
            //Vérifie les couleurs du vin et de l'appelation
            if (updateWine.Color != (await wineContext.Appelations.FindAsync(updateWine.AppelationId)).Color) return 0;

            return await wineContext.Wines.Where(w => w.WineId == updateWine.WineId && w.Drawer.Cellar.UserId == UserId).AsNoTracking()
                .ExecuteUpdateAsync(updates => updates
                .SetProperty(w => w.Color, updateWine.Color)
                .SetProperty(w => w.Name, updateWine.Name)
                .SetProperty(w => w.AppelationId, updateWine.AppelationId));
        }

        //Permet de deplacer un vin
        public async Task<int> MoveWineAsync(int wineId, int drawerId, int userId)
        {
            return await wineContext.Wines.Where(w => w.WineId == wineId && w.Drawer.Cellar.UserId == userId).AsNoTracking()
                .ExecuteUpdateAsync(updates => updates.SetProperty(w => w.DrawerId, drawerId));
        }

        public async Task<int> DeleteWineAsync(int wineId, int userId)
        {
            return await wineContext.Wines.
               Where(w => w.WineId == wineId && w.Drawer.Cellar.UserId == userId).ExecuteDeleteAsync();
        }
    }
}
