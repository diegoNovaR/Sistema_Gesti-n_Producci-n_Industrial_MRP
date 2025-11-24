namespace MRPBaseDatosII.Models
{
    public class IndexProduccionDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public decimal costo_produccion { get; set; }
        public string codigo_lote { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int cantidad_producto_fabricado { get; set; }
    }
}
