namespace MRPBaseDatosII.Models
{
    public class LaptopConMateriaPrimaViewModel
    {
        public Laptop? Laptop { get; set; }
        public List<MateriaPrimaVM>? MateriasPrimas { get; set; }
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
