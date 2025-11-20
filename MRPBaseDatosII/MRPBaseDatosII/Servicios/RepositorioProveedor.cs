using Dapper;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioProveedor
    {
        Task<Proveedor> Crear(Proveedor proveedor);
        Task<IEnumerable<Proveedor>> ObtenerNombreId();
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
                "INSERT INTO Proveedor (NombreEntidad, Direccion, Identificacion) " +
                "VALUES ('@NombreEntidad', '@Direccion', '@Identificacion')",
                new {proveedor.NombreEntidad, proveedor.Direccion, proveedor.Identificacion});
            
            proveedor.Id = idProveedor;
            return proveedor;
        }//comentario PRUEBA BORRAR LUEGO

        public async Task<IEnumerable<Proveedor>> ObtenerNombreId()
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var proveedores = await connection.QueryAsync<Proveedor>("SELECT id, NombreEntidad FROM Proveedor ORDER BY id DESC;");
            return proveedores;
        }

    }
}
