namespace MRPBaseDatosII.Models
{
    public class LaptopConMateriaPrimaViewModel
    {
        public Laptop? Laptop { get; set; }
        public List<MateriaPrimaVM>? MateriasPrimas { get; set; }
        public int ProcesadorId { get; set; }
        public int RamId { get; set; }
        public int AlmacenamientoId { get; set; }
        public int PlacaMadreId { get; set; }
        public int PantallaId { get; set; }
        public int TarjetaVideoId { get; set; }
        public int ChasisId { get; set; }
    }
    public class MateriaPrimaVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CantidadStock { get; set; }
        public int CantidadRequerida { get; set; } 
        public string Tipo { get; set; }

    }
}
