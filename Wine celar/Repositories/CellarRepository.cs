using Wine_cellar.Entities;
using Wine_cellar.IRepositories;

namespace Wine_celar.Repositories
{
    public class CellarRepository : ICellarRepository
    {
        public Task<Celar> DeleteCellarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cellar>> GetAllsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Cellar> GetCellarWithAllAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Cellar> UpdateCellarAsync(Cellar cellar)
        {
            throw new NotImplementedException();
        }
    }
}
