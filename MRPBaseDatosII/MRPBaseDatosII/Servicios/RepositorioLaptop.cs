using Dapper;
using MRPBaseDatosII.Models;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioLaptop
    {
        
    }
    public class RepositorioLaptop
    {
        private readonly string connectionString;
        public RepositorioLaptop(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //public async Task<bool> Crear(LaptopConMateriaPrimaViewModel laptopConMateriaPrima)
        //{
        //    var connection = new Npgsql.NpgsqlConnection(connectionString);
        //    var parametros = new 
        //    var query = await connection.ExecuteScalarAsync<bool>("SELECT sp_crear_laptop_con_receta(@p_nombre,@_tipo,@p_precio_venta,@_detalles)");
        //}

    }
}
