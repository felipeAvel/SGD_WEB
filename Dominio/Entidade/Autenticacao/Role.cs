using SGD.Dominio.Entidade.Autenticacao;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SGD.Dominio.Entidade.Autenticacao
{
    public class Role
    {
        // Keys
        public int Id { get; set; }

        // Properties
        public string RoleName { get; set; }
        public string DisplayRoleName { get; set; }

        // Navigation Properties
        public IList<Usuarios> Usuarios { get; set; }
    }

    public class RoleConfig : EntityTypeConfiguration<Role>
    {
        public RoleConfig()
        {
            ToTable("Role");

            HasKey(r => r.Id);

            Property(r => r.RoleName)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}