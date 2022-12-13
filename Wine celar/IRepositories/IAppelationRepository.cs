using Wine_celar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface IAppelationRepository
    {
        Task<List<Appelation>> GetAllAppelationsAsync();
        Task<GetAppelationViewModel> GetAppelationByIdAsync(int id, int userid);
        Task<List<Appelation>> GetAppelationsByColorAsync(WineColor color);
        Task<Appelation> CreateAppelationAsync(Appelation appelation);
        Task<int> UpdateAppelationAsync(UpdateAppelationViewModel appelation);
        Task<int> DeleteAppelationAsync(int appelationName);
    }
}
