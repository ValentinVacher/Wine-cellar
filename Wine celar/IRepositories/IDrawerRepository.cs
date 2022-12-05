using Wine_celar.Entities;

namespace Wine_celar.IRepositories
{
    public interface IDrawerRepository
    {
        Task<List<Drawer>> GetAllWithWine();
        Task<Drawer> GetDrawerwithWine(int id);
        Task<Drawer> AddDrawer(Drawer drawer);
        Task<Drawer> UpdateDrawer(Drawer drawer);
        Task<Drawer> DeleteDrawer(int id);
    }
}
