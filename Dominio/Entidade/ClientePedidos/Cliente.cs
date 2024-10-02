using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace SGD.Dominio.Entidade.ClientePedidos
{
    public class ClientesPedido
    {
        // Keys
        public int Id { get; set; }

        // Properties
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Email { get; set; }
        public string Foto { get; set; }
        public bool IsAtivo { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? DataModificacao { get; set; }

        // Navigation Properties
        public IList<Pedidos> Pedidos { get; set; }
    }

    public class ClienteConfig : EntityTypeConfiguration<ClientesPedido>
    {
        public ClienteConfig()
        {
            // Define o nome da tabela
            ToTable("ClientesPedidoes");

            // Define a chave primária
            HasKey(c => c.Id);

            // Configurações de propriedades
            Property(c => c.Nome)
                .IsRequired()  // Campo obrigatório
                .HasMaxLength(255);  // Tamanho máximo

            Property(c => c.Telefone)
                .HasMaxLength(20);

            Property(c => c.Endereco)
                .HasMaxLength(255);

            Property(c => c.CEP)
                .HasMaxLength(10);

            Property(c => c.Cidade)
                .HasMaxLength(100);

            Property(c => c.Email)
                .HasMaxLength(255);

            Property(c => c.Foto)
                .HasColumnType("nvarchar(max)");  // Sem limite de tamanho

            Property(c => c.IsAtivo)
                .IsRequired();  // Campo obrigatório

            Property(c => c.DataRegistro)
                .IsRequired();  // Campo obrigatório

            Property(c => c.DataModificacao)
                .IsOptional();  // Pode ser nulo

            // Configuração de relacionamento 1:N (Cliente -> Pedidos)
            HasMany(c => c.Pedidos)
                .WithRequired(p => p.Cliente)  // Relacionamento obrigatório
                .HasForeignKey(p => p.ClienteId)
                .WillCascadeOnDelete(false);  // Sem exclusão em cascata
        }
    }
}