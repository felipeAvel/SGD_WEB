using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Entidade.ClientePedidos;
using SGD.Dominio.Entidade.Gerais;
using SGD.Dominio.Entidade.Video;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace SGD.Dominio
{
    public class AppContext : DbContext
    {
        public AppContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        // DbSets para suas entidades
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Equipes> Equipes { get; set; }
        public DbSet<VideosYT> Videos { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Anexos> Anexos { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<ClientesPedido> Clientes { get; set; }
        public DbSet<Parametros> Parametros { get; set; }
        public DbSet<AnexoPedidos> AnexoPedidos { get; set; }

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                foreach (var entry in ChangeTracker.Entries().Where(t => t.Entity.GetType().GetProperty("DataRegistro") != null))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("DataRegistro").CurrentValue = DateTime.Now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Property("DataRegistro").IsModified = false;
                        if (entry.Entity.GetType().GetProperty("DataModificacao") != null)
                        {
                            entry.Property("DataModificacao").CurrentValue = DateTime.Now;
                        }
                    }
                }
                return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var erros = ex.EntityValidationErrors;
                throw;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // Remove a convenção de pluralização de nomes de tabela
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        //    // Aplicar outras configurações de entidades aqui, se necessário
        //    modelBuilder.Configurations.Add(new PedidoConfig());

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}



