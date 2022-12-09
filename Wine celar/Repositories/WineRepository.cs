﻿using Microsoft.EntityFrameworkCore;
using Wine_cellar.ViewModel;
using Wine_cellar.Contexts;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Wine_celar.ViewModel;
using Microsoft.Identity.Client;

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
        public async Task<List<WineViewModel>> GetAllWinesAsync(int userId)
        {
            var Wines = await wineContext.Wines.Include(w => w.Appelation).Include(w => w.Drawer).ThenInclude(d => d.Cellar)
                .Where(w => w.Drawer.Cellar.UserId == userId).ToListAsync();
            var WinesView = new List<WineViewModel>();
            foreach (var w in Wines)
            {
                var Wine = new WineViewModel();
                Wine.WineId = w.WineId;
                Wine.WineName = w.Name;
                Wine.CellarName = w.Drawer.Cellar.Name;
                Wine.Year = w.Year;
                Wine.Color = w.Color;
                Wine.AppelationName = w.Appelation.AppelationName;
                Wine.DrawerIndex = w.Drawer.Index;
                WinesView.Add(Wine);
            }
            return WinesView.ToList();

        }

        //Permet de recuperer tout les vins à leur apogée dans une liste
        public async Task<List<WineViewModel>> GetApogeeAsync(int userId)
        {
            var wines = await wineContext.Wines.Include(w => w.Appelation).Include(d => d.Drawer).ThenInclude(c => c.Cellar)
                .Where(w => w.Drawer.Cellar.UserId == userId).ToListAsync();
            var winess = new List<WineViewModel>();
            foreach (var w in wines)
            {
                var ToDay = DateTime.Now.Year;
                var max = w.Year + w.Appelation.KeepMax;
                var min = w.Year + w.Appelation.KeepMin;
                if (ToDay >= min && ToDay <= max)
                {
                    var Wine = new WineViewModel();
                    Wine.WineId = w.WineId;
                    Wine.WineName = w.Name;
                    Wine.CellarName = w.Drawer.Cellar.Name;
                    Wine.Year = w.Year;
                    Wine.Color = w.Color;
                    Wine.AppelationName = w.Appelation.AppelationName;
                    Wine.DrawerIndex = w.Drawer.Index;
                    winess.Add(Wine);
                }
            }
            if (winess.Count == 0) return null;
            return winess.OrderBy(w => w.Color).ToList();
        }

        //Permet de recuperer un vin par son id 
        public async Task<Wine> GetWineByIdAsync(int wineId, int userId)
        {
            return await wineContext.Wines.Include(w => w.Appelation).Include(w => w.Drawer).ThenInclude(d => d.Cellar)
                .FirstOrDefaultAsync(p => p.WineId == wineId && p.Drawer.Cellar.UserId == userId);
        }

        //Permet de recuperer une liste de vin selon un terme choisi
        public async Task<List<Wine>> GetWineByWordAsync(string word, int userId)
        {
            return await wineContext.Wines.Include(a => a.Appelation)
                .Where(w => (w.Appelation.AppelationName.Contains(word) || w.Name.Contains(word))
                && w.Drawer.Cellar.UserId == userId).OrderBy(w => w.Color).ToListAsync();
        }

        //Permet de créer/Ajouter un vin si le tiroir n'est pas plein
        public async Task<int> CreateWineAsync(CreateWineViewModel WineView, int userId)
        {
            var Drawer = await wineContext.Drawers.Include(d => d.Wines)
                .FirstOrDefaultAsync(d => d.Index == WineView.DrawerIndex && d.Cellar.Name == WineView.CellarName && d.Cellar.UserId == userId);

            if (Drawer == null) return 1;

            //Verifie si le tiroir est plein
            if (Drawer.IsFull() == true) return 2;

            var wine = new Wine()
            {
                Name = WineView.Name,
                Year = WineView.Year,
                DrawerId = Drawer.DrawerId,
                PictureName = WineView.Picture?.FileName ?? "",
                AppelationId = WineView.AppelationId,
                Color = WineView.Color,
            };
            //Vérifie les couleurs du vin et de l'appelation
            var appelation = await wineContext.Appelations.FindAsync(wine.AppelationId);
            if (wine.Color != appelation.Color) return 3;

            //Ajoute le vin 
            wineContext.Wines.Add(wine);
            await wineContext.SaveChangesAsync();
            return 0;
        }

        //Permet de modifier un vin 
        public async Task<Wine> UpdateWineAsync(UpdateWineViewModel wine, int userId)
        {
            var WineUpdate = await GetWineByIdAsync(wine.WineId, userId);
            if (WineUpdate == null) return null;
            WineUpdate.Name = wine.Name;
            WineUpdate.Color = wine.Color;
            await wineContext.SaveChangesAsync();
            return WineUpdate;
        }

        //Permet de supprimer un vin 
        public async Task<bool> DeleteWineAsync(int WineId, int userId)
        {
            var WineDelete = await GetWineByIdAsync(WineId, userId);

            if (WineDelete == null) return false;

            wineContext.Wines.Remove(WineDelete);
            await wineContext.SaveChangesAsync();

            return true;

        }

        //Permet de deplacer un vin
        public async Task<int> MoveAsync(int WineId, int newDrawerIndex, string cellar, int userId)
        {
            var WineMove = await GetWineByIdAsync(WineId, userId);

            if (WineMove == null) return 1;

            var drawer = await wineContext.Drawers.Include(c => c.Cellar).Include(c => c.Wines)
                .Where(c => c.Cellar.Name == cellar && c.Index == newDrawerIndex
                && c.Cellar.UserId == userId).FirstOrDefaultAsync();

            if (drawer == null) return 2;

            if (drawer.IsFull()) return 3;

            WineMove.DrawerId = drawer.CellarId;
            await wineContext.SaveChangesAsync();
            return 0;
        }

        //Permet de dupliquer un vin si le tiroir n'est pas plein
        public async Task<int> DuplicateAsync(int WineId, int NbrDuplicate, int userId)
        {
            var WineDuplicate = await wineContext.Wines.Include(d => d.Drawer)
                .FirstOrDefaultAsync(p => p.WineId == WineId && p.Drawer.Cellar.UserId == userId);
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

        public async Task<List<Wine>> GetWineByColorAsync(WineColor color, int userId)
        {
            var WinesColor = await wineContext.Wines.Include(w=>w.Appelation).Include(d => d.Drawer).ThenInclude(c => c.Cellar)
                .Where(w => w.Color == color && w.Drawer.Cellar.UserId == userId).ToListAsync();

            if (WinesColor.Count == 0) return null;
            return WinesColor;
        }

        public async Task<int> DeleteEFbyIdAsync(int WineId, int userId)
        {
            
            return await wineContext.Wines.
               Where(w => w.WineId == WineId && w.Drawer.Cellar.UserId == userId).ExecuteDeleteAsync();
        }

        public async Task<int> UpdateEFbyidAsync(UpdateWineViewModel updateWine, int UserId)
        {
            return await wineContext.Wines.Where(w => w.WineId == updateWine.WineId && w.Drawer.Cellar.UserId == UserId).
                ExecuteUpdateAsync(updates=>updates
                .SetProperty(w=>w.Color,updateWine.Color)
                .SetProperty(w=>w.Name,updateWine.Name)
                .SetProperty(w=>w.AppelationId,updateWine.AppelationId));
        }
    }
}
