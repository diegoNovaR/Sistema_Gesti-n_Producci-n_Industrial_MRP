using Dapper;
using MRPBaseDatosII.Models;
using Npgsql;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioLaptop
    {
        Task<bool> Crear(Laptop laptop, string jsonDetalles);
    }
    public class RepositorioLaptop: IRepositorioLaptop
    {
        private readonly string connectionString;
        public RepositorioLaptop(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> Crear(Laptop laptop, string jsonDetalles)
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);

                var result = await connection.ExecuteScalarAsync<bool>(
                    "SELECT sp_crear_laptop_con_receta(@p_nombre, @p_tipo, @p_precio_venta, @p_detalles::json)",
                    new
                    {
                        p_nombre = laptop.Nombre,
                        p_tipo = laptop.Tipo,
                        p_precio_venta = laptop.PrecioVenta,
                        p_detalles = jsonDetalles
                    });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR BD: " + ex.Message);
                throw; // controlador rexibe el error
            }
        }

    }
}
