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
        public int Cantidad { get; set; } 
        public string Tipo { get; set; }

    }
}
