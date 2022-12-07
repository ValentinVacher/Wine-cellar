namespace Wine_cellar.ViewModel
{
    public class CreateWineViewModel
    {
        public string Color { get; set; }
        public string Appelation { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int KeepMin { get; set; }
        public int KeepMax { get; set; }
        public int DrawerIndex { get; set; }
        public string CellarName { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
