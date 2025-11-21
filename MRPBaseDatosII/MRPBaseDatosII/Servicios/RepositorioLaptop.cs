using Dapper;
using MRPBaseDatosII.Models;

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
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            //var parametros = new
            //{
            //    p_nombre = laptop.Nombre,
            //    p_tipo = laptop.Tipo,
            //    p_precio_venta = laptop.PrecioVenta,
            //    p_detalles = jsonDetalles
            //};
            var query = await connection.ExecuteScalarAsync<bool>("SELECT sp_crear_laptop_con_receta(@p_nombre, @p_tipo, @p_precio_venta, @p_detalles::json)", 
                    new
                    {
                        nombre = laptop.Nombre,
                        tipo = laptop.Tipo,
                        precio = laptop.PrecioVenta,
                        detalles = jsonDetalles
                    });
            return query;
        }

    }
}
