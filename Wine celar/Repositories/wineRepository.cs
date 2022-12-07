using Microsoft.EntityFrameworkCore;
using Wine_cellar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

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
        public async Task<List<Wine>> GetAllWinesAsync()
        {
            return await wineContext.Wines.ToListAsync();
        }

        //Permet de recuperer tout les vins à leur apogée dans une liste
        public async Task<List<Wine>> GetApogeeAsync()
        {
            var wines = await GetAllWinesAsync();
            var winess = new List<Wine>();
            foreach (var wine in wines)
            {
                if (wine.IsApogee() == true)
                {
                    winess.Add(wine);
                }            
            }
            return winess.OrderBy(w=>w.KeepMax).ToList();
        }

        //Permet de recuperer un vin par son id 
        public async Task<Wine> GetWineByIdAsync(int wineId)
        {
            return await wineContext.Wines.FirstOrDefaultAsync(p => p.WineId == wineId);
        }

        //Permet de recuperer une liste de vin selon un terme choisi
        public async Task<List<Wine>> GetWineByWordAsync(string word)
        {
            return await wineContext.Wines.Where(w => w.Color.Contains(word) || w.Appelation.Contains(word) || w.Name.Contains(word)).OrderBy(w=>w.Color).ToListAsync();
        }

        //Permet de créer/Ajouter un vin si le tiroir n'est pas plein
        public async Task<Wine> CreateWineAsync(Wine wine)
        {
            var Drawer = await wineContext.Drawers.Include(d => d.Wines).FirstOrDefaultAsync(d => d.DrawerId == wine.DrawerId);
            //Verifie si le tiroir est plein
            if (Drawer.IsFull() == true) return null;
            //Ajoute le vin 
            wineContext.Wines.Add(wine);
            await wineContext.SaveChangesAsync();
            return wine;
        }

        //Permet de modifier un vin 
        public async Task<Wine> UpdateWineAsync(Wine wine)
        {
            var WineUpdate = await GetWineByIdAsync(wine.WineId);
            if (WineUpdate == null) return null;
            WineUpdate.Name = wine.Name;
            WineUpdate.Color = wine.Color;
            WineUpdate.Appelation = wine.Appelation;
            await wineContext.SaveChangesAsync();
            return WineUpdate;
        }

        //Permet de supprimer un vin 
        public async Task<bool> DeleteWineAsync(int WineId)
        {
            var WineDelete = await GetWineByIdAsync(WineId);
            if (WineDelete == null)
                return false;
            wineContext.Wines.Remove(WineDelete);
            await wineContext.SaveChangesAsync();
            return true;

        }

        //Permet de deplacer un vin
        public async Task<Wine> MoveAsync(int WineId, int newDrawerId)
        {
            var WineMove = await GetWineByIdAsync(WineId);
            if (WineMove == null) return null;
            WineMove.DrawerId = newDrawerId;
            await wineContext.SaveChangesAsync();
            return WineMove;
        }

        //Permet de dupliquer un vin si le tiroir n'est pas plein
        public async Task<Wine> DuplicateAsync(int WineId, int NbrDuplicate)
        {
            var WineDuplicate = await GetWineByIdAsync(WineId);

            var Drawer = await wineContext.Drawers.Include(d => d.Wines).FirstOrDefaultAsync(d => d.DrawerId == WineDuplicate.DrawerId);
            //Verifie si le tiroir est plein
            if (Drawer.IsFull() == true)
            {
                return null;
            }
            //Boucle pour le nombre de duplication 
            for (int i = 1; i <= NbrDuplicate; i++)
            {
                var wine = new Wine
                {
                    Color = WineDuplicate.Color,
                    Appelation = WineDuplicate.Appelation,
                    Name = WineDuplicate.Name,
                    Year = WineDuplicate.Year,
                    KeepMax = WineDuplicate.KeepMax,
                    KeepMin = WineDuplicate.KeepMin,
                    DrawerId = WineDuplicate.DrawerId
                };
                wineContext.Wines.Add(wine);
                
            }
            await wineContext.SaveChangesAsync();
            return WineDuplicate;

        }

    }
}
