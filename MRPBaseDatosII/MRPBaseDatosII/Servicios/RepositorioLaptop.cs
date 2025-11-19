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

    }
}
