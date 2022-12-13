using System.Security.Claims;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface IDrawerRepository
    {
        //Permet de recuperer tout les tiroirs avec les vins 
        Task<List<GetDrawerViewModel>> GetAllDrawersAsync(int userId);
        //Permet de recuperer un tiroir avec ses vins 
        Task<GetDrawerViewModel> GetDrawerByIdAsync(int id, int userId);
        //Task<Drawer> GetDrawerAsync(string cellarName, Drawer index, ClaimsIdentity identity);
        //Permet d'ajouter un tiroir 
        Task<int> AddDrawerAsync(CreateDrawerViewModel createDrawer, int userId);
        //Permet de mettre a jour un tiroir 
        Task<int> UpdateDrawerAsync(UpdateDrawerViewModel drawer, int userId);
        //Permet de supprimer un tiroir
        Task<int> DeleteDrawerAsync(int drawerId, int userId);
    }
}
