using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Dominio.Entidade.Autenticacao
{
    public class Equipe
    {
        // Keys
        public int Id { get; set; }

        // Properties
        public string Nome { get; set; }
        public bool IsAtivo { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime DataModificacao { get; set; }

        // Navigation Properties
        public ICollection<Usuario> Usuarios { get; set; }
    }
    public class EquipeConfig : EntityTypeConfiguration<Equipe>
    {
        public EquipeConfig()
        {
            ToTable("Equipe");

            HasKey(e => e.Id);

            Property(e => e.Nome)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}
