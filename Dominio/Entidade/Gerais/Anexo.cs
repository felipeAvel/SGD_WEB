using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Entidade.ClientePedidos;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SGD.Dominio.Entidade.Gerais
{
    public class Anexos
    {
        // Keys
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        // Properties
        public long Tamanho { get; set; }
        public string TipoArquivo { get; set; }
        public string Caminho { get; set; }
        public string NomeArquivo { get; set; }
        public string B64 { get; set; }
        public DateTime DataRegistro { get; set; }

        // Navigation Properties
        public Usuarios Usuario { get; set; }

        public static Anexos FromValues(
            int usuarioId,
            long tamanho,
            string tipoArquivo,
            string caminho,
            string nomeArquivo,
            string b64)
        {
            return new Anexos
            {
                UsuarioId = usuarioId,
                Tamanho = tamanho,
                TipoArquivo = tipoArquivo,
                Caminho = caminho,
                NomeArquivo = nomeArquivo,
                B64 = b64,
                DataRegistro = DateTime.Now
            };
        }
    }

    public class AnexoConfig : EntityTypeConfiguration<Anexos>
    { 
        public AnexoConfig() 
        {
            ToTable("Anexos");

            HasKey(a => a.Id);

            HasRequired(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.UsuarioId);

            Property(a => a.B64)
                .IsMaxLength()
                .HasColumnType("nvarchar(max)");

            Property(a => a.Caminho)
                .IsOptional();
        }
    }
}