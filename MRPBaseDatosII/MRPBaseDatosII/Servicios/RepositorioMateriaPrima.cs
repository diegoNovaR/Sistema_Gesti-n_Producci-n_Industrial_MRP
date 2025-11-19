using Dapper;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioMateriaPrima
    {
        Task<MateriaPrima> CrearMateriaPrima(MateriaPrima materiaPrima);
        Task<IEnumerable<MateriaPrima>> ObtenerListadoMateriaPrima();
        Task<IEnumerable<MateriaPrima>> ObtenerNombreId();
    }
    public class RepositorioMateriaPrima: IRepositorioMateriaPrima
    {
        private readonly string connectionString;
        public RepositorioMateriaPrima(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<MateriaPrima> CrearMateriaPrima(MateriaPrima materiaPrima)//AGREGAR NUEVA MATERIA PRIMA 
        {
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            var id = await connection.ExecuteScalarAsync<int>(
                    "SELECT sp_insertar_materia_prima(@nombre, @descripcion, @marca, tipo)", materiaPrima);
            materiaPrima.Id = id;
            return materiaPrima;
        }

        public async Task<IEnumerable<MateriaPrima>> ObtenerNombreId()
        {
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            var materiaPrima = await connection.QueryAsync<MateriaPrima>(
                "SELECT id, nombre FROM MateriaPrima ORDER BY id DESC");
            return materiaPrima;
        }

        public async Task<IEnumerable<MateriaPrima>> ObtenerListadoMateriaPrima()
        {
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            var materiaPrima = await connection.QueryAsync<MateriaPrima>("SELECT * FROM MateriaPrima");
            return materiaPrima;
        }

    }
}
