using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using System.Security.Claims;
using Wine_celar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface IWineRepository
    {
        //Permet de recuperer tout les vins 
        Task<List<GetWineViewModel>> GetAllWinesAsync(int userId);
        //Permet de recuperer un vin par son id
        Task<Wine> GetWineByIdAsync(int Wineid, int userId);
        //Permet de recuperer un vin contenant le mot saisi
        Task<List<Wine>> GetWinesByWordAsync(string word, int userId);
        //Permet de récuperer tout les vins qui sont à leur apogée
        Task<List<GetWineViewModel>> GetWinesByApogeeAsync(int userId);
        Task<List<Wine>> GetWinesByColorAsync(WineColor color, int UserId);
        //Permet de rajouter un vin 
        Task<int> AddWineAsync(CreateWineViewModel wine, int userId);
        Task<int> DuplicateAsync(int WineId, int NbrDuplicate, int UserId);
        //Permet de mettre à jour un vin
        //Task<Wine> UpdateWineAsync(UpdateWineViewModel wine, Drawer userId);
        //Permet de supprimer un vin
        //Task<bool> DeleteWineAsync(Drawer WineId, Drawer userId);
        Task<int> UpdateWineAsync(UpdateWineViewModel updateWine, int UserId);
        //Permet de deplacer un vin dans un autre tiroir
        Task<int> MoveWineAsync(int wineId,int drawerId, int userId);
        //Permet de dupliquer un vin
        Task<int> DeleteWineAsync(int WineId, int UserId);
    }
}
