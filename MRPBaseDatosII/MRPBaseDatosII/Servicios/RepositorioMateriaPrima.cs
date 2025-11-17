using Dapper;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioMateriaPrima
    {
        Task Crear(OrdenCompra ordenCompra);
    }
    public class RepositorioMateriaPrima: IRepositorioMateriaPrima
    {
        private readonly string connectionString;
        public RepositorioMateriaPrima(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(OrdenCompra ordenCompra)//GENERAR ORDEN DE COMPRA para materia prima
        {
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            var id = await connection.ExecuteScalarAsync<int>(
                    "SELECT sp_insertar_orden_compra(@IdMateriaPrima, @IdProveedor, " +
                    "@CantidadUnidades, @PrecioUnitario)",ordenCompra);
            ordenCompra.Id = id;
        }
    }
}
