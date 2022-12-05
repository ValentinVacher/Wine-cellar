using Wine_celar.Entities;

namespace Wine_celar.IRepositories
{
    public interface IWineRepository
    {
        Task<List<Wine>> GetAllWinesAsync();
        Task<Wine> GetWineByIdAsync(int Wineid);
        Task<List<Wine>> GetWineByNameAsync(string name);
        Task<List<Wine>> GetWineByColorAsync(string color);
        Task<Wine> CreateWineAsync(Wine wine);
        Task<Wine> UpdateWineAsync(Wine wine);
        Task<Wine> DeleteWineAsync(int WineId);
    }
}
