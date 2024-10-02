using SGD.Dominio.Entidade.Gerais;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;


namespace SGD.Dominio.Entidade.ClientePedidos
{
    public class Pedidos
    {
        // Keys
        public int Id { get; set; }
        public int ClienteId { get; set; }

        // Properties
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime? DataEntrega { get; set; }

        // Navigation Properties
        public ClientesPedido Cliente { get; set; }
        public ICollection<Anexos> Anexo;
        public ICollection<AnexoPedidos> AnexoPedido;
    }

    public class PedidoConfig : EntityTypeConfiguration<Pedidos>
    {
        public PedidoConfig()
        {
            // Define o nome da tabela
            ToTable("Pedidos");

            // Define a chave primária
            HasKey(p => p.Id);

            Property(c => c.Status)
                .HasMaxLength(20);

            // Configuração de relacionamento com Cliente (1:N)
            HasRequired(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId)
                .WillCascadeOnDelete(false);  // Desabilita exclusão em cascata

            // Configurações de propriedades
            Property(p => p.Descricao)
                .HasMaxLength(255)  // Define o tamanho máximo da descrição
                .IsOptional();  // Permite ser nulo

            Property(p => p.Valor)
                .IsRequired()  // Campo obrigatório
                .HasPrecision(10, 2);  // Precisão decimal (10 dígitos totais, 2 casas decimais)

            Property(p => p.DataPedido)
                .IsRequired()  // Campo obrigatório
                .HasColumnType("datetime");  // Define o tipo como `datetime`

            Property(p => p.DataEntrega)
                .IsOptional()  // Permite ser nulo
                .HasColumnType("datetime");  // Define o tipo como `datetime`
        }
    }
}