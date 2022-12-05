using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface ICelarRepository
    {
        Task<List<Celar>> GetAllsAsync();
        Task<Celar> GetCelarWithAllAsync(int id);
        Task<Celar> DeleteCelarAsync(int id);
        Task <Celar> UpdateCelarAsync(Celar celar);
    }
}
