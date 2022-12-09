using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using System.Security.Claims;
using Wine_celar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface IWineRepository
    {
        //Permet de recuperer tout les vins 
        Task<List<WineViewModel>> GetAllWinesAsync(int userId);
        //Permet de recuperer un vin par son id
        Task<Wine> GetWineByIdAsync(int Wineid, int userId);
        //Permet de recuperer un vin contenant le mot saisi
        Task<List<Wine>> GetWineByWordAsync(string word, int userId);
        //Permet de rajouter un vin 
        Task<int> CreateWineAsync(CreateWineViewModel wine, int userId);
        //Permet de mettre à jour un vin
        Task<Wine> UpdateWineAsync(UpdateWineViewModel wine, int userId);
        //Permet de supprimer un vin
        Task<bool> DeleteWineAsync(int WineId, int userId);
        //Permet de deplacer un vin dans un autre tiroir
        Task<int> MoveAsync(int WineId,int newDrawerId, string cellar, int userId);
        //Permet de récuperer tout les vins qui sont à leur apogée
        Task<List<WineViewModel>> GetApogeeAsync(int userId);
        //Permet de dupliquer un vin
        Task<int> DuplicateAsync(int WineId,int NbrDuplicate, int userId);
        Task<List<Wine>> GetWineByColorAsync(WineColor color, int userId);
        Task<int> DeleteEFbyIdAsync(int WineId, int userId);
    }
}
