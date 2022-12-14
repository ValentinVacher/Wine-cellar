using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json.Nodes;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface ICellarRepository
    {
        //Permet de recuperer toute les caves
        Task<List<GetCellarViewModel>> GetAllCellarsAsync(int userId);
        //Permet ded recuperer toutes les caves avec tout ses elements
        Task<GetCellarViewModel> GetCellarByIdAsync(int id, int userId);
        Task<List<Cellar>> ExportJsonAsync(string name, int userId);
        //Permet d'ajouter une cave
        Task<Cellar> AddCellarAsync(Cellar cellar, int NbrButtleDrawer);
        Task<string> ImportJsonAsync(string form);
        //Permet de mettre a jour/modifier une cave
        Task<int> UpdateCellarAsync(UpdateCellarViewModel UpCellar, int userId);
        //Permet de supprimer une cave
        Task<int> DeleteCellarAsync(int cellarId, int userId);
    }
}
