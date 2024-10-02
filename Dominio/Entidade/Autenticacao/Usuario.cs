using SGD.Dominio.Entidade.Autenticacao;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Dominio.Entidade.Autenticacao
{
    public class Usuarios
    {
        // Keys
        public int Id { get; set; }

        // Properties
        public int EquipeId { get; set; }
        public string Nome { get; set; }
        public string Racf { get; set; }
        public string Funcional { get; set; }
        public string Login { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string Foto { get; set; }
        public bool IsAtivo { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? DataModificacao { get; set; }

        // Navigation Properties
        public Equipes Equipe { get; set; }
        public ICollection<Role> Roles { get; set; }
    }

    public class UsuarioConfig : EntityTypeConfiguration<Usuarios>
    {
        public UsuarioConfig()
        {
            ToTable("Usuarios");

            HasKey(u => u.Id);

            HasRequired(u => u.Equipe)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EquipeId);

            Property(u => u.Nome)
                .HasMaxLength(250)
                .IsRequired();

            Property(u => u.Login)
                .HasMaxLength(250)
                .IsRequired();

            Property(u => u.PassWord)
                .HasMaxLength(20)
                .IsRequired();

            Property(u => u.Racf)
                .HasMaxLength(7)
                .IsRequired();

            Property(u => u.Funcional)
                .HasMaxLength(9)
                .IsRequired();

            HasMany(u => u.Roles)
                .WithMany(r => r.Usuarios)
                .Map(c => c.ToTable("Roles"));
        }
    }
}