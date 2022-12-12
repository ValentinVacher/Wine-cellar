using System.Security.Claims;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface ICellarRepository
    {
        //Permet de recuperer toute les caves
        Task<List<Cellar>> GetAllsAsync(int userId);
        //Permet ded recuperer toutes les caves avec tout ses elements
        Task<Cellar> GetCellarById(int id, int userId);
        //Permet de supprimer une cave
        Task<int> DeleteCellarAsync(int cellarId, int userId);
        //Permet de mettre a jour/modifier une cave
        Task<Cellar> UpdateCellarAsync(Cellar cellar);
        //Permet d'ajouter une cave
        Task<Cellar> AddCellarAsync(Cellar cellar, int NbrButtleDrawer);
    }
}
