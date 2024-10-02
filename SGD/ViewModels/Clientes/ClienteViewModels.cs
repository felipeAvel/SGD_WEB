using Dominio.Entidade.Gerais;
using SGD.Dominio.Entidade.ClientePedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SGD.ViewModels.Clientes
{
    public class ClienteViewModels
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Email { get; set; }
        public string Foto { get; set; }
        public HttpPostedFileBase FotoInput { get; set; }
        public string IsAtivo { get; set; }
        public DateTime DataRegistro { get; set; }
        public IList<Pedidos> Pedidos { get; set; }
        public IList<AnexoDTO> Anexos { get; set; }

        public void LoadFromModel(ClientesPedido cliente, IList<AnexoDTO> anexo)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            Telefone = cliente.Telefone;
            Endereco = cliente.Endereco;
            CEP = cliente.CEP;
            Cidade = cliente.Cidade;
            Email = cliente.Email;
            Foto = cliente.Foto;
            IsAtivo = (cliente.IsAtivo) != false ? "Ativo" : "Desativado";
            DataRegistro = cliente.DataRegistro;
            Pedidos = cliente.Pedidos.ToList();
            Anexos = anexo.ToList();
        }
    }
}