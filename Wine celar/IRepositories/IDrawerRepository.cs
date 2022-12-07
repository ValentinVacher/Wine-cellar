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
        //Permet d'ajouter un tiroir 
        Task<int> AddDrawerAsync(CreateDrawerViewModel createDrawer, ClaimsIdentity identity);
        //Permet de mettre a jour un tiroir 
        Task<Drawer> UpdateDrawerAsync(Drawer drawer);
        //Permet de supprimer un tiroir
        Task<bool> DeleteDrawerAsync(int cellarId,int index);
    }
}
