using SGD.Dominio.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;
using System.Web.Security;
using System.Runtime.Remoting.Contexts;
using Dominio.Business;
using System.Security.Claims;
using System.Net;
using System.Net.Sockets;


namespace SGD.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        private readonly UsuariosBusiness usuariosBusiness;
        private readonly LoginBusiness loginBusiness;

        public LoginController()
        {
            this.usuariosBusiness = new UsuariosBusiness();
            this.loginBusiness = new LoginBusiness();
        }

        // GET: Login
        [HttpGet]
        public ActionResult Index(string msg)
        {
            ViewBag.ErrorUser = msg;

            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<ActionResult> Index(string login, string senha)
        {
            var usuario = await loginBusiness.Login(login, senha);

            if (usuario != null && usuario.IsAtivo == true)
            {
                var ticket = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, usuario.Login, DateTime.Now, DateTime.Now.AddHours(12), true, usuario.Roles.FirstOrDefault().RoleName));
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket);
                Response.Cookies.Add(cookie);
            }
            else if (usuario != null && usuario.IsAtivo == false)
                return RedirectToAction("Index", new { msg = "Usuário Desativado!" });
            else
                return RedirectToAction("Index", new { msg = "Login ou Senha Incorreta!" });

            return Redirect("Home");
        }

        // GET: logoff
        [HttpGet]
        public ActionResult logoff()
        {
            // Remove o cookie de autenticação
            FormsAuthentication.SignOut();

            // Limpa todos os cookies
            ClearAllCookies();

            // Redireciona para a página de login ou inicial
            return RedirectToAction("Index", "Login");
        }

        // Método para limpar todos os cookies
        private void ClearAllCookies()
        {
            var cookies = Request.Cookies.AllKeys;
            foreach (var cookie in cookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
        }

        [HttpGet]
        public JsonResult BuscarDadosUsuario()
        {
            var dados = usuariosBusiness.ObterUsuario(Base_LoginUsuario);
            return Json(dados, JsonRequestBehavior.AllowGet);
        }
    }
}
