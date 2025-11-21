using Dapper;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioMateriaPrima
    {
        Task<MateriaPrima> CrearMateriaPrima(MateriaPrima materiaPrima);
        Task<IEnumerable<MateriaPrima>> ObtenerListadoMateriaPrima();
        Task<IEnumerable<MateriaPrima>> ObtenerNombreId();
        Task<IEnumerable<MateriaPrimaVM>> ObtenerStockCantidadTipo();
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
                    "SELECT sp_insertar_materia_prima(@nombre, @descripcion, @marca, @tipo)", materiaPrima);
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

        public async Task<IEnumerable<MateriaPrimaVM>> ObtenerStockCantidadTipo()
        {
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            var materiaPrimaVM = await connection.QueryAsync<MateriaPrimaVM>(
                "SELECT mp.id, mp.nombre, i.stock, mp.tipo " +
                "FROM materiaPrima AS mp " +
                "JOIN inventario AS i ON mp.id = i.id_materia_prima " +
                "WHERE mp.id = i.id_materia_prima AND i.tipo = 'Materia Prima'");

            return materiaPrimaVM;

        }

    }
}
