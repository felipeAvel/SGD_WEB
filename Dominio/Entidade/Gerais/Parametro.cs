using SGD.Dominio.Entidade.Autenticacao;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SGD.Dominio.Entidade.Gerais
{
    public class Parametros
    {
        // Keys
        public int Id { get; set; }

        // Properties
        public int Hash { get; set; }
        public int UsuarioRegistroId { get; set; }
        public string IsAtivo { get; set; }
        public string Valor { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? DataModificacao { get; set; }

        // Navigation Properties
        public Usuarios UsuarioRegistro { get; set; }
    }

    public class ParametroConfig : EntityTypeConfiguration<Parametros>
    {
        public ParametroConfig()
        {
            ToTable("Parametros");

            HasKey(p => p.Id);

            Property(p => p.Valor)
                .IsMaxLength()
                .IsRequired();
        }
    }
}