using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface ICellarRepository
    {
        Task<List<Cellar>> GetAllsAsync();
        Task<Cellar> GetCellarWithAllAsync(int id);
        Task<Cellar> DeleteCellarAsync(int id);
        Task<Cellar> UpdateCellarAsync(Cellar cellar);
        Task<Cellar> AddCellarAsync(Cellar cellar, int NbrButtleDrawer);
    }
}
