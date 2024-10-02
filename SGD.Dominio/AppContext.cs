using SGD.Dominio.Entidade.Autenticacao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Dominio
{
    public class AppContext : DbContext
    {
        public AppContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        // DbSets para suas entidades
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Equipe> equipes { get; set; }
    }
}



