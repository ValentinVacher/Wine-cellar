using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IDrawerRepository
    {
        //Permet de recuperer tout les tiroirs avec les vins 
        Task<List<Drawer>> GetAllWithWineAsync();
        //Permet de recuperer un tiroir avec ses vins 
        Task<Drawer> GetDrawerwithWineAsync(string cellarName,int index);
        //Permet d'ajouter un tiroir 
        Task<Drawer> AddDrawerAsync(Drawer drawer);
        //Permet de mettre a jour un tiroir 
        Task<Drawer> UpdateDrawerAsync(Drawer drawer);
        //Permet de supprimer un tiroir
        Task<bool> DeleteDrawerAsync(int cellarId,int index);
    }
}
