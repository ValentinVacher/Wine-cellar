using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IColorRepository
    {
        Task<List<ColorWine>> GetColorswithAppelationAsync();
        Task<ColorWine> GetColorwithAppelationByNameAsync(string ColorName);
    }
}
