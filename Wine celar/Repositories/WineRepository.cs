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

        /// <summary>
        /// Permet de récuperer tout les vins dans une liste
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Retourne la listede tout les vins de l'utilisateur</returns>
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

        /// <summary>
        /// Permet de vérifier si le vin est à son apogée
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Retourne la liste des vins à leur apogée</returns>
        public async Task<List<GetWineViewModel>> GetWinesByApogeeAsync(int userId)
        {
            var wines = await wineContext.Wines.Include(w => w.Appelation).Include(d => d.Drawer).ThenInclude(c => c.Cellar).AsNoTracking()
                .Where(w => w.Drawer.Cellar.UserId == userId).ToListAsync();
            var winesView = new List<GetWineViewModel>();

            foreach (var w in wines)
            {
                var ToDay = DateTime.Now.Year;
                var max = w.Year + w.Appelation.KeepMax;
                var min = w.Year + w.Appelation.KeepMin;

                if (ToDay >= min && ToDay <= max)
                {
                    var Wine = Convertor.GetViewWine(w);
                    winesView.Add(Wine);
                }
            }

            if (winesView.Count == 0) return null;
            return winesView.OrderBy(w => w.Color).ToList();
        }

        /// <summary>
        /// Permet de voir un vin grace à son id
        /// </summary>
        /// <param name="wineId"></param>
        /// <param name="userId"></param>
        /// <returns>Retourne le vin correspondant à l'id saisi</returns>
        public async Task<Wine> GetWineByIdAsync(int wineId, int userId)
        {
            return await wineContext.Wines.Include(w => w.Appelation).Include(w => w.Drawer).ThenInclude(d => d.Cellar).AsNoTracking()
                .FirstOrDefaultAsync(p => p.WineId == wineId && p.Drawer.Cellar.UserId == userId);
        }

        /// <summary>
        /// Permet de faire une recherche par mot clé
        /// </summary>
        /// <param name="word"></param>
        /// <param name="userId"></param>
        /// <returns>Retourne une liste de vin composés du mot clé</returns>
        public async Task<List<Wine>> GetWinesByWordAsync(string word, int userId)
        {
            return await wineContext.Wines.Include(a => a.Appelation).Include(d => d.Drawer).ThenInclude(c => c.Cellar).AsNoTracking()
                .Where(w => (w.Appelation.Name.Contains(word) || w.Name.Contains(word)) && w.Drawer.Cellar.UserId == userId)
                .OrderBy(w => w.Color).ToListAsync();
        }

        public async Task<List<Wine>> GetWinesByColorAsync(WineColor color, int userId)
        {
            var WinesColor = await wineContext.Wines.Include(w => w.Appelation).Include(d => d.Drawer).ThenInclude(c => c.Cellar).AsNoTracking()
                .Where(w => w.Color == color && w.Drawer.Cellar.UserId == userId).ToListAsync();

            if (WinesColor.Count == 0) return null;
            return WinesColor;
        }

        /// <summary>
        /// Permet de créer un vin
        /// </summary>
        /// <param name="wineView"></param>
        /// <param name="userId"></param>
        /// <returns>Retourne le vin créer</returns>
        public async Task<int> AddWineAsync(CreateWineViewModel wineView, int userId)
        {
            var Drawer = await wineContext.Drawers.Include(d => d.Wines).AsNoTracking()
                .FirstOrDefaultAsync(d => d.DrawerId == wineView.DrawerId && d.Cellar.UserId == userId);

            if (Drawer == null) return 1;

            //Verifie si le tiroir est plein
            if (Drawer.IsFull() == true) return 2;

            var wine = Convertor.CreateWine(wineView);
            var appelation = await wineContext.Appelations.FindAsync(wine.AppelationId);

            //Verifie si l'appelation exist
            if (appelation == null) return 4;

            //Vérifie les couleurs du vin et de l'appelation
            if (wine.Color != appelation.Color) return 3;

            //Ajoute le vin 
            wineContext.Wines.Add(wine);
            await wineContext.SaveChangesAsync();
            return 0;
        }

        /// <summary>
        /// Permet de dupliquer un vin autant de fois qu'on le souhaite
        /// </summary>
        /// <param name="wineId"></param>
        /// <param name="nbrDuplicate"></param>
        /// <param name="userId"></param>
        /// <returns>Retourne le nombre d'elements créer</returns>
        public async Task<int> DuplicateAsync(int wineId, int nbrDuplicate, int userId)
        {
            var WineDuplicate = await wineContext.Wines.Include(d => d.Drawer).ThenInclude(w => w.Wines)
                .FirstOrDefaultAsync(p => p.WineId == wineId && p.Drawer.Cellar.UserId == userId);

            if(WineDuplicate == null) return-1;

            var nbWine = 0;
            var nbWineInDrawer = WineDuplicate.Drawer.Wines.Count();

            //Boucle pour le nombre de duplication 
            for (int i = 1; i <= nbrDuplicate; i++)
            {
                var wine = new Wine
                {
                    Color = WineDuplicate.Color,
                    AppelationId = WineDuplicate.AppelationId,
                    Name = WineDuplicate.Name,
                    Year = WineDuplicate.Year,
                    DrawerId = WineDuplicate.DrawerId,
                    PictureName = WineDuplicate.PictureName
                };

                //Verifie si le tiroir est plein
                if (nbWineInDrawer >= WineDuplicate.Drawer.NbBottleMax) break;

                wineContext.Wines.Add(wine);

                nbWine++;
                nbWineInDrawer++;
            }

            await wineContext.SaveChangesAsync();

            return nbWine;
        }  
        /// <summary>
        /// Permet de mettre à jour un vin
        /// </summary>
        /// <param name="updateWine"></param>
        /// <param name="UserId"></param>
        /// <returns>Retourne le vin modifier</returns>
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

        /// <summary>
        /// Permet de déplacer un vin dans un autre tiroir
        /// </summary>
        /// <param name="wineId"></param>
        /// <param name="drawerId"></param>
        /// <param name="userId"></param>
        /// <returns>Retourne le vin dans son nouvelle emplacement</returns>
        public async Task<int> MoveWineAsync(int wineId, int drawerId, int userId)
        {
            var drawer = await wineContext.Drawers.FirstOrDefaultAsync(d => d.DrawerId == drawerId);

            if (drawer == null) return -1;
            if (drawer.IsFull()) return -2;

            return await wineContext.Wines.Where(w => w.WineId == wineId && w.Drawer.Cellar.UserId == userId).AsNoTracking()
                    .ExecuteUpdateAsync(updates => updates.SetProperty(w => w.DrawerId, drawerId));
        }

        /// <summary>
        /// Permet de supprimer un vin 
        /// </summary>
        /// <param name="wineId"></param>
        /// <param name="userId"></param>
        /// <returns>Retourne l'element supprimer</returns>
        public async Task<int> DeleteWineAsync(int wineId, int userId)
        {
            return await wineContext.Wines.
               Where(w => w.WineId == wineId && w.Drawer.Cellar.UserId == userId).ExecuteDeleteAsync();
        }
    }
}
