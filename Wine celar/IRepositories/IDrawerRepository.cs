using System.Security.Claims;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface IDrawerRepository
    {
        //Permet de recuperer tout les tiroirs avec les vins 
        Task<List<Drawer>> GetAllWithWineAsync(ClaimsIdentity identity);
        //Permet de recuperer un tiroir avec ses vins 
        Task<Drawer> GetDrawerwithWineAsync(string cellarName,int index, ClaimsIdentity identity);
        //Task<Drawer> GetDrawerAsync(string cellarName, int index, ClaimsIdentity identity);
        //Permet d'ajouter un tiroir 
        Task<int> AddDrawerAsync(CreateDrawerViewModel createDrawer, ClaimsIdentity identity);
        //Permet de mettre a jour un tiroir 
        Task<int> UpdateDrawerAsync(UpdateDrawerViewModel drawer, ClaimsIdentity identity);
        //Permet de supprimer un tiroir
        Task<bool> DeleteDrawerAsync(string cellarName, int index, ClaimsIdentity identity);
    }
}
