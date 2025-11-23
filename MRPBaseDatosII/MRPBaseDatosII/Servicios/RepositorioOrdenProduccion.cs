using Dapper;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioOrdenProduccion
    {
        Task<IEnumerable<OrdenProduccionViewModel>> ObtenerOrdenProduccion();
    }
    public class RepositorioOrdenProduccion: IRepositorioOrdenProduccion
    {
        private readonly string connectionString;
        public RepositorioOrdenProduccion(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<OrdenProduccionViewModel>> ObtenerOrdenProduccion()
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var ordenesProduccion = await connection.QueryAsync<OrdenProduccionViewModel>("SELECT r.id, l.nombre as nombreLaptop, r.costototaldereceta " +
                "FROM receta AS r " +
                "JOIN laptop AS l ON r.idlaptop = l.id");
            return ordenesProduccion;
        }

    }
}
