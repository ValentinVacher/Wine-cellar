using Microsoft.EntityFrameworkCore;
using Wine_cellar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Wine_cellar.Repositories
{
    public class WineRepository : IWineRepository
    {
        //Declaration du context et du logger
        readonly WineContext wineContext;
        ILogger<WineRepository> logger;

        //Constructeur
        public WineRepository(WineContext wineContext, ILogger<WineRepository> logger)
        {
            this.wineContext = wineContext;
            this.logger = logger;
        }

        //Permet de recuperer tout les vins dans une liste
        public async Task<List<Wine>> GetAllWinesAsync(ClaimsIdentity identity)
        {
            return await wineContext.Wines
                .Where(w => w.Drawer.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value))
                .OrderBy(w => w.Color).ToListAsync();
        }

        //Permet de recuperer tout les vins à leur apogée dans une liste
        public async Task<List<Wine>> GetApogeeAsync(ClaimsIdentity identity)
        {
            var wines = await wineContext.Wines.Include(w=>w.Appelation).Include(d => d.Drawer).ThenInclude(c => c.Cellar)
                .where(w => w.Drawer.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value)).ToListAsync();
            var winess = new List<Wine>();
            foreach (var wine in wines)
            {
                var ToDay = DateTime.Now.Year;
                var max = wine.Year + wine.Appelation.KeepMax;
                var min = wine.Year + wine.Appelation.KeepMin;
                if(ToDay>=min && ToDay <=max) 
                {
                winess.Add(wine);
                }   
            }
            if (winess.Count == 0) return null; 
            return winess.OrderBy(w => w.Color).ToList();
        }

        //Permet de recuperer un vin par son id 
        public async Task<Wine> GetWineByIdAsync(int wineId, ClaimsIdentity identity)
        {
            return await wineContext.Wines
                .FirstOrDefaultAsync(p => p.WineId == wineId && p.Drawer.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value));
        }

        //Permet de recuperer une liste de vin selon un terme choisi
        public async Task<List<Wine>> GetWineByWordAsync(string word, ClaimsIdentity identity)
        {
            return await wineContext.Wines
                .Where(w => (w.Color.Contains(word) || w.Appelation.Contains(word) || w.Name.Contains(word)) 
                && w.Drawer.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value))
                .OrderBy(w=>w.Color).ToListAsync();
        }

        //Permet de créer/Ajouter un vin si le tiroir n'est pas plein
        public async Task<int> CreateWineAsync(CreateWineViewModel wineView, ClaimsIdentity identity)
        {
            var Drawer = await wineContext.Drawers.Include(d => d.Wines)
                .FirstOrDefaultAsync(d => d.Index == wineView.DrawerIndex && d.Cellar.Name == wineView.CellarName 
                && d.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value));

            if (Drawer == null) return 1;

            //Verifie si le tiroir est plein
            if (Drawer.IsFull() == true) return 2;

            var wine = new Wine()
            {
                Name = WineViewModel.Name,
                Year = WineViewModel.Year,
                DrawerId = WineViewModel.DrawerId,
                PictureName = WineViewModel.Picture?.FileName ?? "",
                AppelationId = WineViewModel.AppelationId
            };

            //Ajoute le vin 
            wineContext.Wines.Add(wine);
            await wineContext.SaveChangesAsync();
            return 0;
        }

        //Permet de modifier un vin 
        public async Task<Wine> UpdateWineAsync(UpdateWineViewModel wine, ClaimsIdentity identity)
        {
            var WineUpdate = await GetWineByIdAsync(wine.WineId, identity);
            if (WineUpdate == null) return null;
            WineUpdate.Name = wine.Name;
            WineUpdate.Color = wine.Color;
            await wineContext.SaveChangesAsync();
            return WineUpdate;
        }

        //Permet de supprimer un vin 
        public async Task<bool> DeleteWineAsync(int WineId, ClaimsIdentity identity)
        {
            var WineDelete = await GetWineByIdAsync(WineId, identity);

            if (WineDelete == null) return false;

            wineContext.Wines.Remove(WineDelete);
            await wineContext.SaveChangesAsync();

            return true;

        }

        //Permet de deplacer un vin
        public async Task<int> MoveAsync(int WineId, int newDrawerIndex, string cellar, ClaimsIdentity identity)
        {
            var WineMove = await GetWineByIdAsync(WineId, identity);

            if (WineMove == null) return 1;

            var drawer = await wineContext.Drawers.Include(c => c.Cellar).Include(c => c.Wines)
                .Where(c => c.Cellar.Name == cellar && c.Index == newDrawerIndex 
                && c.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value)).FirstOrDefaultAsync();

            if (drawer == null) return 2;

            if (drawer.IsFull()) return 3;

            WineMove.DrawerId = drawer.CellarId;
            await wineContext.SaveChangesAsync();
            return 0;
        }

        //Permet de dupliquer un vin si le tiroir n'est pas plein
        public async Task<int> DuplicateAsync(int WineId, int NbrDuplicate, ClaimsIdentity identity)
        {
            var WineDuplicate = await wineContext.Wines.Include(d => d.Drawer)
                .FirstOrDefaultAsync(p => p.WineId == WineId && p.Drawer.Cellar.UserId == int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value));
            var nbWine = 0;
            var nbWinInDrawer = WineDuplicate.Drawer.Wines.Count();

            var wine = new Wine
            {
                Color = WineDuplicate.Color,
                Appelation = WineDuplicate.Appelation,
                Name = WineDuplicate.Name,
                Year = WineDuplicate.Year,
                KeepMax = WineDuplicate.KeepMax,
                KeepMin = WineDuplicate.KeepMin,
                DrawerId = WineDuplicate.DrawerId,
                PictureName = WineDuplicate.PictureName
            };

            //Boucle pour le nombre de duplication 
            for (int i = 1; i <= NbrDuplicate; i++)
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

        public async Task<List<Wine>> GetWineByColorAsync(WineColor color)
        {
            var WinesColor= await wineContext.Wines.Where(w=>w.Color== color).ToListAsync();
            if (WinesColor.Count == 0) return null;
            return WinesColor;
        }
    }
}
