namespace MRPBaseDatosII.Models
{
    public class OrdenCompraViewModel
    {
        public OrdenCompra ordenCompra { get; set; }
        public IEnumerable<MateriaPrima> materiasPrimas { get; set; }
        public IEnumerable<Proveedor> proveedores { get; set; }
    }
}
