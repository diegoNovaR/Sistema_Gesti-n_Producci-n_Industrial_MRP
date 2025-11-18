using Dapper;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioProveedor
    {
        Task<Proveedor> Crear(Proveedor proveedor);
    }
    public class RepositorioProveedor: IRepositorioProveedor
    {
        private readonly string connectionString;
        public RepositorioProveedor(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Proveedor> Crear(Proveedor proveedor)
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var idProveedor = await connection.ExecuteScalarAsync<int>(
                "SELECT sp_insertar_proveedor(@nombreEntidad, @direccion, @identificacion_)",
                new {proveedor.NombreEntidad, proveedor.Direccion, proveedor.Identificacion});
            
            proveedor.Id = idProveedor;
            return proveedor;
        }

    }
}
