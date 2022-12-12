using Wine_celar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface IAppelationRepository
    {
        Task<List<Appelation>> GetAllAppelationsAsync();
        Task<Appelation> GetAppelationAsync(int id);
        Task<Appelation> CreateAppelationAsync(Appelation appelation);
        Task<int> UpdateAppelationAsync(UpdateAppelationViewModel appelation);
        Task<int> DeleteAppelationAsync(int appelationName);
        Task<List<Appelation>> GetAppelationsByColoAsync(WineColor color);
    }
}
