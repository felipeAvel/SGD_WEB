using Dominio.Business;
using PagedList;
using SGD.Dominio.Business;
using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Entidade.ClientePedidos;
using SGD.Dominio.Entidade.Gerais;
using SGD.ViewModels.Clientes;
using SGD.ViewModels.Videos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SGD.Controllers
{
    [AllowAnonymous]
    public class ClienteController : BaseController
    {
        private readonly ClientesBusiness clientesBusiness;
        private readonly AnexosBusiness anexosBusiness;

        public ClienteController()
        {
            this.clientesBusiness = new ClientesBusiness();
            this.anexosBusiness = new AnexosBusiness();
        }

        [HttpGet]
        public async Task<ActionResult> Index(Paginacao paginacao, string categoria)
        {
            var termo = paginacao.TermoBusca;
            var clientes = await clientesBusiness.ListarClientes(paginacao);

            ViewBag.ListaClientes = new StaticPagedList<ClientesPedido>(
                clientes.Item1, paginacao.PaginaAtual,
                paginacao.TamanhoPagina, clientes.Item2);

            return View(paginacao);
        }

        [HttpGet]
        public async Task<ActionResult> CadastrarCliente()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> EditarCliente(int id)
        {
            var cliente = await clientesBusiness.ObterClientePedidos(id);

            var vm = new ClienteViewModels();

            vm.LoadFromModel(cliente.Item2, cliente.Item1);

            return View(vm);
        }


        [HttpPost]
        public async Task<JsonResult> EditarCliente(ClienteViewModels vm)
        {
            var clienteExistente = await clientesBusiness.ObterClientePedidos(vm.Id);

            clienteExistente.Item2.Nome = vm.Nome;
            clienteExistente.Item2.Telefone = vm.Telefone;
            clienteExistente.Item2.Endereco = vm.Endereco;
            clienteExistente.Item2.CEP = vm.CEP;
            clienteExistente.Item2.Email = vm.Email;

            if (vm.FotoInput != null)
            {
                var binaryReader = new BinaryReader(vm.FotoInput.InputStream);

                byte[] fileBytes = binaryReader.ReadBytes(vm.FotoInput.ContentLength);
                string base64String = Convert.ToBase64String(fileBytes);

                clienteExistente.Item2.Foto = base64String;
            }

            if (await clientesBusiness.EditarCliente(clienteExistente.Item2))
                return Json("Sucesso", JsonRequestBehavior.AllowGet);
            else
                return Json("Erro ao Editar Cliente!", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ClientePedidos(int id, int? idPedido)
        {
            if (idPedido > 0)
            {
                var cliente = await clientesBusiness.ObterClientePedidos(id, idPedido.Value);

                var vm = new ClienteViewModels();

                vm.LoadFromModel(cliente.Item2, cliente.Item1);

                vm.Pedidos = vm.Pedidos.Where(p => p.Id == idPedido).ToList();

                return View(vm);
            }
            else
            {
                var cliente = await clientesBusiness.ObterClientePedidos(id);

                var vm = new ClienteViewModels();

                vm.LoadFromModel(cliente.Item2, cliente.Item1);

                return View(vm);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CadastrarPedidos(int id)
        {
            ViewBag.ClienteId = id;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> EditarPedidos(int pedidoId, int clienteId)
        {
            ViewBag.PedidoId = pedidoId;
            ViewBag.ClienteId = clienteId;

            var pedido = await clientesBusiness.ObterPedidos(pedidoId);

            var vm = new CadastrarPedidoViewModels();

            vm.Descricao = pedido.Descricao;
            vm.Valor = pedido.Valor;
            vm.DataPedido = pedido.DataPedido;
            vm.DataEntrega = pedido.DataEntrega;
            vm.Status = pedido.Status;

            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> EditarPedidos(CadastrarPedidoViewModels vm)
        {
            var pedido = await clientesBusiness.ObterPedidos(vm.Id);

            pedido.Descricao = vm.Descricao;
            pedido.Valor = vm.Valor;
            pedido.Status = vm.Status;
            pedido.DataPedido = vm.DataPedido;
            pedido.DataEntrega = vm.DataEntrega;

            if (await clientesBusiness.EditarPedidos(pedido))
                return Json("Sucesso", JsonRequestBehavior.AllowGet);
            else
                return Json("Erro ao Editar Pedido!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> EnviarArquivoPedido(HttpPostedFileBase arquivo, int pedidoId)
        {
            var binaryReader = new BinaryReader(arquivo.InputStream);

            byte[] fileBytes = binaryReader.ReadBytes(arquivo.ContentLength);
            string base64String = Convert.ToBase64String(fileBytes);

            var anexo = Anexos.FromValues(
                                usuarioId: Base_UsuarioId,
                                tamanho: arquivo.ContentLength,
                                tipoArquivo: "Pedido",
                                caminho: string.Empty,
                                nomeArquivo: arquivo.FileName,
                                b64: base64String);

            var anexoId = await anexosBusiness.AdicionarAnexo(anexo);

            var anexoPedido = new AnexoPedidos();
            anexoPedido.PedidoId = pedidoId;
            anexoPedido.AnexoId = anexoId;
            anexoPedido.IsAtivo = true;

            if (await clientesBusiness.InserirIdAnexo(anexoPedido))
                return Json("Sucesso", JsonRequestBehavior.AllowGet);
            else
                return Json("Erro ao Enviar o Arquivo!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> FinalizarPedido(int pedidoId)
        {
            if (await clientesBusiness.FinalizarPedido(pedidoId))
                return Json("Sucesso", JsonRequestBehavior.AllowGet);
            else
                return Json("Erro ao Finalizar Pedido!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> CadastrarPedidos(CadastrarPedidoViewModels vm)
        {
            var binaryReader = new BinaryReader(vm.Arquivos.InputStream);

            byte[] fileBytes = binaryReader.ReadBytes(vm.Arquivos.ContentLength);
            string base64String = Convert.ToBase64String(fileBytes);

            var anexo = Anexos.FromValues(
                                usuarioId: Base_UsuarioId,
                                tamanho: vm.Arquivos.ContentLength,
                                tipoArquivo: "Pedido",
                                caminho: string.Empty,
                                nomeArquivo: vm.Arquivos.FileName,
                                b64: base64String);

            var anexoId = await anexosBusiness.AdicionarAnexo(anexo);

            var pedido = new Pedidos();
            pedido.ClienteId = vm.ClienteId;
            pedido.Valor = vm.Valor;
            pedido.Status = vm.Status;
            pedido.Descricao = vm.Descricao;
            pedido.DataPedido = vm.DataPedido;
            pedido.DataEntrega = vm.DataEntrega;

            var PedidoId = await clientesBusiness.InserirPedidoCliente(pedido);

            var anexoPedido = new AnexoPedidos();
            anexoPedido.PedidoId = PedidoId;
            anexoPedido.AnexoId = anexoId;
            anexoPedido.IsAtivo = true;

            if (await clientesBusiness.InserirIdAnexo(anexoPedido))
                return Json("Sucesso", JsonRequestBehavior.AllowGet);
            else
                return Json("Erro ao Cadastrar o Pedido!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> InserirCliente(InserirClienteViewModels vm)
        {
            var binaryReader = new BinaryReader(vm.Foto.InputStream);

            byte[] fileBytes = binaryReader.ReadBytes(vm.Foto.ContentLength);
            string base64String = Convert.ToBase64String(fileBytes);

            var cliente = new ClientesPedido();

            cliente.Nome = vm.Nome;
            cliente.Telefone = vm.Telefone;
            cliente.Endereco = vm.Endereco;
            cliente.CEP = vm.CEP;
            cliente.Cidade = vm.Cidade;
            cliente.Email = vm.Email;
            cliente.Foto = base64String;
            cliente.IsAtivo = true;

            //Valida se o User ja existe
            if (await clientesBusiness.ObterCliente(cliente))
                return Json("Cliente ja Existente!", JsonRequestBehavior.AllowGet);
            else
            {
                if (await clientesBusiness.InserirCliente(cliente))
                    return Json("Sucesso", JsonRequestBehavior.AllowGet);
                else
                    return Json("Erro ao Inserir Usuário!", JsonRequestBehavior.AllowGet);
            }
        }
    }
}