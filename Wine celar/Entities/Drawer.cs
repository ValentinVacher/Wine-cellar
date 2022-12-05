namespace Wine_celar.Entities
{
    public class Drawer
    {
        public int DrawerId { get; set; }
        public int Index { get; set; }
        public int NbBottleMax { get; set; }
        public Celar Celar { get; set; }
        public int CelarId { get; set; }
        public List<Wine> Wines { get; set; }


       
    }
}
