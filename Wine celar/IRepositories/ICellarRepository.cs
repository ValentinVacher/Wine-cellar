using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface ICellarRepository
    {
        Task<List<Cellar>> GetAllsAsync();
        Task<Cellar> GetCellarWithAllAsync(int id);
        Task<Cellar> DeleteCellarAsync(int id);
        Task <Cellar> UpdateCellarAsync(Cellar cellar);
    }
}
