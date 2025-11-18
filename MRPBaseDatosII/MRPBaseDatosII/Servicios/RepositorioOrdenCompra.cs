using Dapper;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioOrdenCompra
    {
        Task Crear(OrdenCompra ordenCompra);
        Task<IEnumerable<OrdenCompraListadoDTO>> ObtenerOrdenCompras();
    }
    public class RepositorioOrdenCompra: IRepositorioOrdenCompra
    {
        private readonly string connectionString;
        public RepositorioOrdenCompra(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(OrdenCompra ordenCompra)//GENERAR ORDEN DE COMPRA para materia prima
        {
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            var id = await connection.ExecuteScalarAsync<int>(
                    "SELECT sp_insertar_orden_compra(@IdMateriaPrima, @IdProveedor, " +
                    "@CantidadUnidades, @PrecioUnitario)", ordenCompra);
            ordenCompra.Id = id;
        }

        public async Task<IEnumerable<OrdenCompraListadoDTO>> ObtenerOrdenCompras()//OBTENER TODAS LAS ORDENES DE COMPRA
        {
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            var ordenes = await connection.QueryAsync<OrdenCompraListadoDTO>("SELECT * FROM fn_Lista_Ordenes_Compra()");
            return ordenes;
        }
    }
}
