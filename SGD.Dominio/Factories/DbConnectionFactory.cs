using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Dominio.Factories
{
    public class DbConnectionFactory
    {
        public AppContext CreateDbContext()
        {
            // Defina a string de conexão aqui ou obtenha de um arquivo de configuração
            var connectionString = ConfigurationManager.ConnectionStrings["MyDbContext"].ConnectionString;

            return new AppContext(connectionString);
        }
    }
}
