using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IDrawerRepository
    {
        Task<List<Drawer>> GetAllWithWineAsync();
        Task<Drawer> GetDrawerwithWineAsync(string cellarName,int index);
        Task<Drawer> AddDrawerAsync(Drawer drawer);
        Task<Drawer> UpdateDrawerAsync(Drawer drawer);
        Task<Drawer> DeleteDrawerAsync(string cellarName,int index);
    }
}
