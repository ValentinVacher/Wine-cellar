using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface ICellarRepository
    {
        //Permet de recuperer toute les caves
        Task<List<Cellar>> GetAllsAsync();
        //Permet ded recuperer toutes les caves avec tout ses elements
        Task<Cellar> GetCellarWithAllAsync(int id);
        //Permet de supprimer une cave
        Task<bool> DeleteCellarAsync(int id);
        //Permet de mettre a jour/modifier une cave
        Task<Cellar> UpdateCellarAsync(Cellar cellar);
        //Permet d'ajouter une cave
        Task<Cellar> AddCellarAsync(Cellar cellar, int NbrButtleDrawer);
    }
}
