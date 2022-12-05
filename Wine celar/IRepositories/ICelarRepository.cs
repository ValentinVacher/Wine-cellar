using Wine_celar.Entities;

namespace Wine_celar.IRepositories
{
    public interface ICelarRepository
    {
        Task<List<Celar>> GetAlls();
        Task<Celar> GetCelarWithAll(int id);
        Task<Celar> DeleteCelar(int id);
        Task <Celar> UpdateCelar(Celar celar);
    }
}
