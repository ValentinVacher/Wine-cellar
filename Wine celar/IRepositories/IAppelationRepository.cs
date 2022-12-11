using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IAppelationRepository
    {
        Task<List<Appelation>> GetAllAppelationsAsync();
        Task<Appelation> GetAppelationAsync(string appelationName);
        Task<Appelation> CreateAppelationAsync(Appelation appelation);
        Task<Appelation> UpdateAppelationAsync(Appelation appelation);
        Task<int> DeleteAppelationAsync(int appelationName);
        Task<List<Appelation>> GetAppelationsByColoAsync(WineColor color);
    }
}
