using Dapper;

namespace MRPBaseDatosII.Servicios
{
    public interface IRepositorioMateriaPrima
    {

    }
    public class RepositorioMateriaPrima: IRepositorioMateriaPrima
    {
        private readonly string connectionString;
        public RepositorioMateriaPrima(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


    }
}
