using Wine_celar.Entities;

namespace Wine_celar.IRepositories
{
    public interface ICelarRepository
    {
        Task<List<Celar>> GetAllsAsync();
        Task<Celar> GetCelarWithAllAsync(int id);
        Task<Celar> DeleteCelarAsync(int id);
        Task <Celar> UpdateCelarAsync(Celar celar);
    }
}
