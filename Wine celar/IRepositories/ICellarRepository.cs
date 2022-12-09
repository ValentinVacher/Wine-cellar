using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface ICellarRepository
    {
        //Permet de recuperer toute les caves
        Task<List<Cellar>> GetAllsAsync(int userId);
        //Permet ded recuperer toutes les caves avec tout ses elements
        Task<List<Cellar>> GetCellarByName(string name, int userId);
        //Permet de supprimer une cave
        Task<bool> DeleteCellarAsync(Cellar cellar);
        //Permet de mettre a jour/modifier une cave
        Task<Cellar> UpdateCellarAsync(Cellar cellar);
        //Permet d'ajouter une cave
        Task<Cellar> AddCellarAsync(Cellar cellar, int NbrButtleDrawer);
        Task<string> ImportJsonAsync(string form);
    }
}
