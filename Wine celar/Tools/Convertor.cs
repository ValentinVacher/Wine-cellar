using Wine_celar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.Tools
{
    public class Convertor
    {
        public static Cellar CreateCellar(CreateCellarViewModel viewModel)
        {
            return new Cellar()
            {
                Name = viewModel.Name,
                NbDrawerMax = viewModel.NbDrawerMax
            };
        }
        public static Drawer CreateDrawer(CreateDrawerViewModel viewModel)
        {
            return new Drawer()
            {
                Index = viewModel.index,
                NbBottleMax = viewModel.NbBottleMax,
                CellarId = viewModel.CellarId,
            };
        }
        public static Appelation CreateAppelation(CreateAppelationViewModel viewModel)
        {
            return new Appelation()
            {
                Name = viewModel.Name,
                KeepMin = viewModel.KeepMin,
                KeepMax = viewModel.KeepMax,
                Color = viewModel.Color,
            };
        }
        public static Wine CreateWine(CreateWineViewModel wineViewModel)
        {
            return new Wine()
            {
                Name = wineViewModel.Name,
                Year = wineViewModel.Year,
                PictureName = wineViewModel.Picture?.FileName ?? "",
                Color = wineViewModel.Color,
                AppelationId = wineViewModel.AppelationId,
                DrawerId = wineViewModel.DrawerId,
            };
        }
        public static WineViewModel ViewWine(Wine wine)
        {
            return new WineViewModel()
            {
                WineId = wine.WineId,
                WineName = wine.Name,
                CellarName = wine.Drawer.Cellar.Name,
                Year = wine.Year,
                Color = wine.Color,
                AppelationName = wine.Appelation.Name,
                DrawerIndex = wine.Drawer.Index
            };

        }
    }
}
