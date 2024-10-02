using Dominio.Business;
using SGD.Dominio.Entidade.Gerais;
using PagedList;
using SGD.Dominio.Business;
using SGD.Dominio.Entidade.Video;
using SGD.ViewModels.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using SGD.Dominio.Entidade.Autenticacao;
using System.IO;
using System.Text;

namespace SGD.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : BaseController
    {
        private readonly UsuariosBusiness usuariosBusiness;
        private readonly EquipesBusiness equipesBusiness;

        public UsuarioController()
        {
            this.usuariosBusiness = new UsuariosBusiness();
            this.equipesBusiness = new EquipesBusiness();
        }

        [HttpGet]
        public async Task<ActionResult> Index(Paginacao paginacao, string categoria)
        {
            var termo = paginacao.TermoBusca;
            var usuarios = await usuariosBusiness.ListarUsuarios(paginacao);

            ViewBag.ListaUsuarios = new StaticPagedList<Usuarios>(
                usuarios.Item1, paginacao.PaginaAtual,
                paginacao.TamanhoPagina, usuarios.Item2);

            return View(paginacao);
        }

        [HttpGet]
        public async Task<ActionResult> InserirUsuario()
        {
            await PreencherDados();

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> InserirUsuario(UsuarioViewModels vm)
        {
            var binaryReader = new BinaryReader(vm.Foto.InputStream);

            byte[] bytesSenha = Encoding.UTF8.GetBytes(vm.Senha);
            byte[] fileBytes = binaryReader.ReadBytes(vm.Foto.ContentLength);
            string base64String = Convert.ToBase64String(fileBytes);

            var usuario = new Usuarios();

            usuario.Nome = vm.Nome;
            usuario.Login = vm.Login;
            usuario.Racf = vm.Racf;
            usuario.Funcional = vm.Funcional;
            usuario.Email = vm.Email;
            usuario.EquipeId = vm.EquipeId;
            usuario.Login = vm.Login;
            usuario.Foto = base64String;
            usuario.PassWord = Convert.ToBase64String(bytesSenha);
            usuario.IsAtivo = true;

            //Valida se o User ja existe
            if (await usuariosBusiness.ObterUsuario(usuario))
                return Json("Usuário ja Existente!", JsonRequestBehavior.AllowGet);
            else
            {
                if (await usuariosBusiness.InserirUsuario(usuario))
                    return Json("Sucesso", JsonRequestBehavior.AllowGet);
                else
                    return Json("Erro ao Inserir Usuário!", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AtualizarUsuarioPerfil(UsuarioViewModels vm)
        {
            var binaryReader = new BinaryReader(vm.Foto.InputStream);
            byte[] fileBytes = binaryReader.ReadBytes(vm.Foto.ContentLength);
            string base64String = Convert.ToBase64String(fileBytes);

            var usuario = await usuariosBusiness.ObterUsuario(vm.Id);

            usuario.Foto = base64String;

            if (await usuariosBusiness.AtualizarUsuarioPerfil(usuario))
                return Json("Sucesso", JsonRequestBehavior.AllowGet);
            else
                return Json("Erro ao Inserir Usuário!", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<JsonResult> AtualizarUsuario(UsuarioViewModels vm)
        {
            var binaryReader = new BinaryReader(vm.Foto.InputStream);
            byte[] fileBytes = binaryReader.ReadBytes(vm.Foto.ContentLength);
            string base64String = Convert.ToBase64String(fileBytes);

            var usuario = await usuariosBusiness.ObterUsuario(vm.Id);

            usuario.Nome = vm.Nome;
            usuario.Login = vm.Login;
            usuario.Racf = vm.Racf;
            usuario.Funcional = vm.Funcional;
            usuario.Email = vm.Email;
            usuario.EquipeId = vm.EquipeId;
            usuario.Login = vm.Login;
            usuario.Foto = base64String;
            usuario.IsAtivo = true;

            if (await usuariosBusiness.AtualizarUsuarioPerfil(usuario))
                return Json("Sucesso", JsonRequestBehavior.AllowGet);
            else
                return Json("Erro ao Inserir Usuário!", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<ActionResult> VerPerfilUsuario()
        {
            var usuario = await usuariosBusiness.ObterUsuario(Base_UsuarioId);
            var vm = new UsuarioViewModels();
            vm.LoadFromModel(usuario);

            return View(vm);
        }

        [HttpGet]
        public async Task<ActionResult> EditarUsuario(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index", "Usuario");

            await PreencherDados();

            var usuario = await usuariosBusiness.ObterUsuario(id.Value);
            var vm = new UsuarioViewModels();
            vm.LoadFromModel(usuario);

            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> MudarStatus(int id)
        {
            var usuario = await usuariosBusiness.ObterUsuario(id);

            if (usuario == null)
                return Json("Erro ao Excluir Usuário!", JsonRequestBehavior.AllowGet);
            else
            {
                if (await usuariosBusiness.MudarStatus(usuario))
                    return Json("Sucesso", JsonRequestBehavior.AllowGet);
                else
                    return Json("Erro ao Excluir Usuário!", JsonRequestBehavior.AllowGet);
            }
        }

        private async Task PreencherDados(int? equipeId = null)
        {
            var equipes = await equipesBusiness.ListarEquipes();

            ViewBag.Equipes = new SelectList(equipes, "Id", "Nome", equipeId);

            ViewBag.ItemSelect = equipeId;
        }


    }
}