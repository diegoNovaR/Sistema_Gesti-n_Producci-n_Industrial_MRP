namespace MRPBaseDatosII.Models
{
    public class Receta
    {
        public int Id { get; set; }
        public int IdLaptop { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public decimal CostoTotalDeReceta { get; set; }
    }
}
