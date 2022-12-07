using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IColorRepository
    {
        Task<List<ColorWine>> GetColorswithAppelationAsync();
        Task<List<ColorWine>> GetColorwithAppelationByNameAsync(string ColorName);
    }
}
