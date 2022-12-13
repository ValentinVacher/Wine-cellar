using Microsoft.EntityFrameworkCore;
using Wine_cellar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Wine_cellar.Tools;

namespace Wine_cellar.Repositories
{

    public class CellarRepository : ICellarRepository
    {
        //Creation du context 
        WineContext wineContext;

        //Constructeur
        public CellarRepository(WineContext winecontext)
        {
            this.wineContext = winecontext;
        }

        /// <summary>
        /// Permet de récuperer toutes les caves de l'utilisateur
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Retorune une liste contenant toutes les caves de l'utilisateur</returns>
        public async Task<List<GetCellarViewModel>> GetAllCellarsAsync(int userId)
        {
            var cellars = await wineContext.Cellars.Include(c => c.Drawers.OrderBy(d => d.Index)).ThenInclude(d => d.Wines).ThenInclude(a => a.Appelation)
                .AsNoTracking().Where(c => c.UserId == userId).AsNoTracking().ToListAsync();
            var cellarsView = new List<GetCellarViewModel>();   
            foreach (Cellar celar in cellars)
            {
                var drawersView = new List<GetDrawerViewModel>();
                foreach (var drawer in celar.Drawers)
                {
                    var winesView = new List<GetWineViewModel>();
                    foreach (var wine in drawer.Wines)
                    {
                        var wineView = Convertor.GetViewWine(wine);
                        winesView.Add(wineView);
                    }
                    var drawerView = Convertor.GetViewDrawer(drawer, winesView);
                    drawersView.Add(drawerView);
                }
                var cellarView = Convertor.GetViewCellar(celar, drawersView);
                cellarsView.Add(cellarView);
            }
            return cellarsView;
        }

        /// <summary>
        /// Permet de Récuperer une cave par son Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns>Retourne la cave corresspondant à l'id saisi</returns>
        public async Task<GetCellarViewModel> GetCellarByIdAsync(int id, int userId)
        {
            var cellar = await wineContext.Cellars.Include(c => c.Drawers.OrderBy(d => d.Index)).ThenInclude(d => d.Wines).ThenInclude(a => a.Appelation)
                .AsNoTracking().FirstOrDefaultAsync(c => c.CellarId == id && c.UserId == userId);

            if (cellar == null) return null;
            var drawersView = new List<GetDrawerViewModel>();
            foreach (Drawer drawer in cellar.Drawers)
            {
                var winesView = new List<GetWineViewModel>();
                foreach (var wine in drawer.Wines)
                {
                    var wineView = Convertor.GetViewWine(wine);
                    winesView.Add(wineView);
                }
                var drawerView = Convertor.GetViewDrawer(drawer, winesView);
                drawersView.Add(drawerView);
            }
            return Convertor.GetViewCellar(cellar, drawersView);
        }

        /// <summary>
        /// Permet de créer un fichier Json et de le sauvegarder dans un dossier du projet
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Retourne les données sauvegarder</returns>
        public async Task<List<Cellar>> ExportJsonAsync(string name)
        {
            var result = await wineContext.Cellars
                .Include(c => c.Drawers
                .OrderBy(d => d.Index))
                .ThenInclude(d => d.Wines).ThenInclude(a => a.Appelation).AsNoTracking().ToListAsync();

            string fileName = $"{name}.json";
            using FileStream createStream = File.Create("Json\\" + fileName);
            await System.Text.Json.JsonSerializer.SerializeAsync(createStream, result, new JsonSerializerOptions { WriteIndented = true, ReferenceHandler = ReferenceHandler.IgnoreCycles });
            await createStream.DisposeAsync();
            return result;
        }

        /// <summary>
        /// Permet d'ajouter une cave
        /// </summary>
        /// <param name="cellar"></param>
        /// <param name="nbrButtleDrawer">Nombre de bouteille par tiroir</param>
        /// <returns>Retourne la cave ajouter</returns>
        public async Task<Cellar> AddCellarAsync(Cellar cellar, int nbrButtleDrawer)
        {
            //Ajoute la cave
            wineContext.Cellars.Add(cellar);
            await wineContext.SaveChangesAsync();
            //Ajoute les tiroirs
            for (int i = 1; i <= cellar.NbDrawerMax; i++)
                wineContext.Drawers.Add(new Drawer { CellarId = cellar.CellarId, Index = i, NbBottleMax = nbrButtleDrawer });

            await wineContext.SaveChangesAsync();
            return cellar;
        }

        //Importe un fichier Json
        public async Task<string> ImportJsonAsync(string form)
        {
            var deserializ = System.Text.Json.JsonSerializer.Deserialize<List<Cellar>>(form);

            foreach (var item in deserializ)
            {
                item.CellarId = 0;
                foreach (var val in item.Drawers)
                {
                    val.DrawerId = 0;
                    val.CellarId = 0;
                    foreach (var value in val.Wines)
                    {
                        value.WineId = 0;
                        value.DrawerId = 0;
                        value.Appelation = null;
                    }
                }
            }

            wineContext.Cellars.AddRange(deserializ);
            await wineContext.SaveChangesAsync();
            return form;
        }

        //Permet de modifier une cave
        public async Task<int> UpdateCellarAsync(UpdateCellarViewModel updateCellar, int userId)
        {
            return await wineContext.Cellars.AsNoTracking().Where(c => c.CellarId == updateCellar.CellarId && c.UserId == userId).
                ExecuteUpdateAsync(updates => updates
                .SetProperty(c => c.UserId, updateCellar.UserId)
                .SetProperty(c => c.Name, updateCellar.Name)
                .SetProperty(c => c.Temperature, updateCellar.Temperature)
                .SetProperty(c => c.CellarType, updateCellar.CellarType)
                .SetProperty(c => c.Brand, updateCellar.Brand)
                .SetProperty(c => c.BrandOther, updateCellar.BrandOther));
        }

        //Permet de supprimer une cave et ses tiroirs
        public async Task<int> DeleteCellarAsync(int cellarId, int userId)
        {
            await wineContext.Wines.AsNoTracking().Where(w => w.Drawer.CellarId == cellarId && w.Drawer.Cellar.UserId == userId).ExecuteDeleteAsync();
            await wineContext.Drawers.AsNoTracking().Where(d => d.CellarId == cellarId && d.Cellar.UserId == userId).ExecuteDeleteAsync();

            return await wineContext.Cellars.AsNoTracking().Where(c => c.CellarId == cellarId && c.UserId == userId).ExecuteDeleteAsync();
        }
    }
}