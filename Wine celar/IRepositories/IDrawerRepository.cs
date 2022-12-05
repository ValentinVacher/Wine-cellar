using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IDrawerRepository
    {
        Task<List<Drawer>> GetAllWithWineAsync();
        Task<Drawer> GetDrawerwithWineAsync(int id);
        Task<Drawer> AddDrawerAsync(Drawer drawer);
        Task<Drawer> UpdateDrawerAsync(Drawer drawer);
        Task<Drawer> DeleteDrawerAsync(int id);
    }
}
