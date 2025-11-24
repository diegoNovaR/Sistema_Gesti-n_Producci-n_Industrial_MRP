using Dapper;
using Microsoft.AspNetCore.Mvc;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioOrdenProduccion
    {
        Task<CrearOrdenProduccionDTO> Crear(CrearOrdenProduccionViewModel ordenProduccion);
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

        public async Task<CrearOrdenProduccionDTO> Crear(CrearOrdenProduccionViewModel ordenProduccion)
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionString);
            var parametros = new
            {
                p_id_receta = ordenProduccion.LaptopId,
                p_cantidad_fabricar = ordenProduccion.Cantidad
            };
            var ordenProduccionCreada = await connection.QuerySingleAsync<CrearOrdenProduccionDTO>(
                    "SELECT * FROM generar_orden_produccion(@p_id_receta,@p_cantidad_fabricar);",parametros);
            return ordenProduccionCreada;
        }

        //public async Task<IEnumerable<CrearOrdenProduccionDTO>>

    }
}
