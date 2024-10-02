using SGD.Dominio.Entidade.ClientePedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGD.ViewModels.Clientes
{
    public class CadastrarPedidoViewModels
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime? DataEntrega { get; set; }
        public HttpPostedFileBase Arquivos { get; set; }
    }
}