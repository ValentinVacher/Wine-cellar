using Wine_celar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IAppelationRepository
    {
        Task<List<Appelation>> GetAllAppelationsAsync();
        Task<Appelation> CreateAppelationAsync(Appelation appelation);
        Task<Appelation> UpdateAppelationAsync(string appelationName);
        Task<Appelation> DeleteAppelationAsync(string appelationName);
    }
}
