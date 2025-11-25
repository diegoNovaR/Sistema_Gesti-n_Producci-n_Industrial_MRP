using Dapper;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioInventario
    {
        Task<IEnumerable<InventarioLaptopDTO>> LaptopConInventario();
    }
    public class RepositorioInventario: IRepositorioInventario
    {
        private readonly string connectionString;
        public RepositorioInventario(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<InventarioLaptopDTO>> LaptopConInventario()
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var query = await connection.QueryAsync<InventarioLaptopDTO>("SELECT l.nombre, l.tipo, l.precioventa, i.stock, i.stock_minimo, i.ubicacion_seccion, i.ubicacion_stand " +
                "FROM Laptop AS l " +
                "JOIN Inventario AS i ON l.id = i.id_Laptop"); 
            return query;
        }

    }
}
