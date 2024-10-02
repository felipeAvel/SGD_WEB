using System;
using System.Web;
using SGD.Dominio.Entidade.ClientePedidos;

namespace SGD.ViewModels.Clientes
{
    public class InserirClienteViewModels
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Email { get; set; }
        public string IsAtivo { get; set; }
        public HttpPostedFileBase Foto { get; set; }
        public string FotoCaminho { get; set; }
        public DateTime DataRegistro { get; set; }

        public void LoadFromModel(ClientesPedido cliente)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            Telefone = cliente.Telefone;
            Endereco = cliente.Endereco;
            CEP = cliente.CEP;
            Cidade = cliente.Cidade;
            Email = cliente.Email;
            FotoCaminho = cliente.Foto;
            IsAtivo = (cliente.IsAtivo) != false ? "Ativo" : "Desativado";
            DataRegistro = cliente.DataRegistro;
        }
    }
}