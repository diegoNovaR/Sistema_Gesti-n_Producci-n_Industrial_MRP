namespace MRPBaseDatosII.Models
{
    public class CrearOrdenProduccionDTO
    {
        public int orden_id { get; set; }
        public int lote_id { get; set; }
        public string codigo_lote { get; set; }
        public decimal costo_total { get; set; }
    }
}
