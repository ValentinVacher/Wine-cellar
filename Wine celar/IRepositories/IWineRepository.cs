using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using System.Security.Claims;
using Wine_celar.ViewModel;

namespace Wine_cellar.IRepositories
{
    public interface IWineRepository
    {
        //Permet de recuperer tout les vins 
        Task<List<WineViewModel>> GetAllWinesAsync(ClaimsIdentity identity);
        //Permet de recuperer un vin par son id
        Task<Wine> GetWineByIdAsync(int Wineid, ClaimsIdentity identity);
        //Permet de recuperer un vin contenant le mot saisi
        Task<List<Wine>> GetWineByWordAsync(string word, ClaimsIdentity identity);
        //Permet de rajouter un vin 
        Task<int> CreateWineAsync(CreateWineViewModel wine, ClaimsIdentity identity);
        //Permet de mettre à jour un vin
        Task<Wine> UpdateWineAsync(UpdateWineViewModel wine, ClaimsIdentity identity);
        //Permet de supprimer un vin
        Task<bool> DeleteWineAsync(int WineId, ClaimsIdentity identity);
        //Permet de deplacer un vin dans un autre tiroir
        Task<int> MoveAsync(int WineId,int newDrawerId, string cellar, ClaimsIdentity identity);
        //Permet de récuperer tout les vins qui sont à leur apogée
        Task<List<WineViewModel>> GetApogeeAsync(ClaimsIdentity identity);
        //Permet de dupliquer un vin
        Task<int> DuplicateAsync(int WineId,int NbrDuplicate, ClaimsIdentity identity);
        Task<List<Wine>> GetWineByColorAsync(WineColor color, ClaimsIdentity identity);
        Task<int> DeleteEFbyIdAsync(int WineId, ClaimsIdentity identity);
        Task<int> UpdateEFbyidAsync(UpdateWineViewModel updateWine, int UserId);
    }
}
