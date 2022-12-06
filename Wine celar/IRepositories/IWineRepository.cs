using Wine_cellar.ViewModel;
using Wine_cellar.Entities;

namespace Wine_cellar.IRepositories
{
    public interface IWineRepository
    {
        //Permet de recuperer tout les vins 
        Task<List<Wine>> GetAllWinesAsync();
        //Permet de recuperer un vin par son id
        Task<Wine> GetWineByIdAsync(int Wineid);
        //Permet de recuperer un vin contenant le mot saisi
        Task<List<Wine>> GetWineByWordAsync(string word);
        //Permet de rajouter un vin 
        Task<Wine> CreateWineAsync(Wine wine);
        //Permet de mettre à jour un vin
        Task<Wine> UpdateWineAsync(Wine wine);
        //Permet de supprimer un vin
        Task<bool> DeleteWineAsync(int WineId);
        //Permet de deplacer un vin dans un autre tiroir
        Task<Wine> MoveAsync(int WineId,int newDrawerId);
        //Permet de récuperer tout les vins qui sont à leur apogée
        Task<List<Wine>> GetApogeeAsync();
        //Permet de dupliquer un vin
        Task<Wine> DuplicateAsync(int WineId,int NbrDuplicate);
    }
}
