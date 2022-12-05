using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IWineRepository
    {
        Task<List<Wine>> GetAllWinesAsync();
        Task<Wine> GetWineByIdAsync(int Wineid);
        //Task<List<Wine>> GetWineByNameAsync(string name);
        //Task<List<Wine>> GetWineByColorAsync(string color);
        Task<List<Wine>> GetWineByWordAsync(string word);
        Task<Wine> CreateWineAsync(Wine wine);
        Task<Wine> UpdateWineAsync(Wine wine);
        Task<Wine> DeleteWineAsync(int WineId);
    }
}
