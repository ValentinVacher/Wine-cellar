using Microsoft.EntityFrameworkCore;
using Wine_cellar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;


namespace Wine_cellar.Repositories
{

    public class CellarRepository : ICellarRepository
    {
        //Creation du context et du logger
        WineContext wineContext;
        ILogger<CellarRepository> Logger;

        //Constructeur
        public CellarRepository(WineContext winecontext, ILogger<CellarRepository> Logger)
        {
            this.wineContext = winecontext;
            this.Logger = Logger;
        }

        //Recupere une liste de toute les caves
        public async Task<List<Cellar>> GetAllsAsync(int userId)
        {
            return await wineContext.Cellars.Include(c => c.Drawers.OrderBy(d => d.Index)).ThenInclude(d => d.Wines).
                Where(c => c.UserId == userId).ToListAsync();
        }

        //Permet de recuperer une cave avec tout ses elements
        public async Task<Cellar> GetCellarById(int id, int userId)
        {
            return await wineContext.Cellars.Include(c => c.Drawers.OrderBy(d => d.Index)).ThenInclude(d => d.Wines).
                FirstOrDefaultAsync(c => c.CellarId == id && c.UserId == userId);
        }

        //Permet de rajouter une cave et lui donner un nombre de tiroirs
        public async Task<Cellar> AddCellarAsync(Cellar cellar, int NbrButtleDrawer)
        {
            //Ajoute la cave
            wineContext.Cellars.Add(cellar);
            await wineContext.SaveChangesAsync();
            //Ajoute les tiroirs
            for (int i = 1; i <= cellar.NbDrawerMax; i++)
                wineContext.Drawers.Add(new Drawer { CellarId = cellar.CellarId, Index = i, NbBottleMax = NbrButtleDrawer });

            await wineContext.SaveChangesAsync();
            return cellar;
        }

        //Permet de supprimer une cave et ses tiroirs
        public async Task<int> DeleteCellarAsync(int cellarId, int userId)
        {
            await wineContext.Wines.Where(w => w.Drawer.CellarId == cellarId && w.Drawer.Cellar.UserId == userId).ExecuteDeleteAsync();
            await wineContext.Drawers.Where(d => d.CellarId == cellarId && d.Cellar.UserId == userId).ExecuteDeleteAsync();

            return await wineContext.Cellars.Where(c => c.CellarId == cellarId && c.UserId == userId).ExecuteDeleteAsync();
        }

        //Permet de modifier une cave
        public async Task<int> UpdateCellarAsync(UpdateCellarViewModel updateCellar, int userId)
        {
            return await wineContext.Cellars.Where(c => c.CellarId == updateCellar.CellarId && c.UserId == userId).
                ExecuteUpdateAsync(updates => updates
                .SetProperty(c => c.UserId, updateCellar.UserId)
                .SetProperty(c => c.Name, updateCellar.Name)
                .SetProperty(c => c.Temperature, updateCellar.Temperature)
                .SetProperty(c => c.CellarType, updateCellar.CellarType)
                .SetProperty(c => c.Brand, updateCellar.Brand)
                .SetProperty(c => c.BrandOther, updateCellar.BrandOther));
        }

        public async Task<string> ImportJsonAsync(string form)
        {

            var deserializ = JsonSerializer.Deserialize<Cellar>(form + ".json");
            wineContext.Cellars.Add(deserializ);
            await wineContext.SaveChangesAsync();
            return form;


        }

        public async Task<List<Cellar>> ExportJsonAsync()
        {
            var result = await wineContext.Cellars
                .Include(c => c.Drawers
                .OrderBy(d => d.Index))
                .ThenInclude(d => d.Wines).ThenInclude(a => a.Appelation).ToListAsync();

            string fileName = "UserCellars.json";
            using FileStream createStream = File.Create("Json\\" + fileName);
            await JsonSerializer.SerializeAsync(createStream, result, new JsonSerializerOptions { WriteIndented = true, ReferenceHandler = ReferenceHandler.IgnoreCycles });
            await createStream.DisposeAsync();
            return result;
        }



    }
}

