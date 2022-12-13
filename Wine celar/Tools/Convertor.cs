using Wine_celar.Entities;
using Wine_celar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.ViewModel;

namespace Wine_cellar.Tools
{
    public class Convertor
    {
        public static GetAppelationViewModel GetAppelation(Appelation appelation, List<GetWineViewModel> getWineViewModel)
        {
            return new GetAppelationViewModel()
            {
                AppelationId = appelation.AppelationId,
                Color = appelation.Color,
                KeepMax = appelation.KeepMax,
                KeepMin = appelation.KeepMin,
                Name = appelation.Name,
                Wines = getWineViewModel
            };
        }

        public static GetWineViewModel GetViewWine(Wine wine)
        {
            return new GetWineViewModel()
            {
                WineId = wine.WineId,
                WineName = wine.Name,
                CellarName = wine.Drawer.Cellar.Name,
                Year = wine.Year,
                Color = wine.Color,
                AppelationName = wine.Appelation.Name,
                DrawerIndex = wine.Drawer.Index,
                PictureName = wine.PictureName,
            };
        }

        public static GetDrawerViewModel GetViewDrawer(Drawer drawer, List<GetWineViewModel> wines)
        {
            return new GetDrawerViewModel()
            {
                DrawerId = drawer.DrawerId,
                Index = drawer.Index,
                NbBottleMax = drawer.NbBottleMax,
                CellarName = drawer.Cellar.Name,
                Wines = wines
            };
        }

        public static GetCellarViewModel GetViewCellar(Cellar cellar, List<GetDrawerViewModel> drawers)
        {
            return new GetCellarViewModel()
            {
                CellarId = cellar.CellarId,
                Name = cellar.Name,
                NbDrawerMax = cellar.NbDrawerMax,
                UserId = cellar.UserId,
                CellarType = cellar.CellarType,
                Brand = cellar.Brand,
                BrandOther = cellar.BrandOther,
                Temperature = cellar.Temperature,
                Drawers = drawers,
            };
        }

        public static Cellar CreateCellar(CreateCellarViewModel viewModel)
        {
            return new Cellar()
            {
                Name = viewModel.Name,
                NbDrawerMax = viewModel.NbDrawerMax,
                CellarType = viewModel.CellarType,
                Brand = viewModel.Brand,
                BrandOther = viewModel.BrandOther,
                Temperature = viewModel.Temperature
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
    }
}
