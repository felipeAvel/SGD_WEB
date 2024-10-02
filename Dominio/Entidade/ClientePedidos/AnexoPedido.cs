using SGD.Dominio.Entidade.Gerais;
using System;
using System.Data.Entity.ModelConfiguration;

namespace SGD.Dominio.Entidade.ClientePedidos
{
    public class AnexoPedidos
    {
        // Keys
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int AnexoId { get; set; }

        // Properties
        public bool IsAtivo { get; set; }
        public DateTime DataRegistro { get; set; }
        public Pedidos Pedido { get; set; }
        public Anexos Anexo { get; set; }
    }

    public class AnexoPedidoConfig : EntityTypeConfiguration<AnexoPedidos>
    {
        public AnexoPedidoConfig()
        {
            HasRequired(pa => pa.Anexo)
                .WithMany()
                .HasForeignKey(pa => pa.AnexoId);

            HasRequired(pa => pa.Pedido)
                .WithMany(pa => pa.AnexoPedido)
                .HasForeignKey(pa => pa.PedidoId);

        }
    }
}