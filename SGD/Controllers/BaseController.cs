using Dominio.Business;
using SGD.Dominio.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SGD.Controllers
{
    [AllowAnonymous]
    public class BaseController : Controller
    {
        private readonly UsuariosBusiness usuariosBusiness;
        private readonly ClientesBusiness clientesBusiness;

        public BaseController()
        {
            this.usuariosBusiness = new UsuariosBusiness();
            this.clientesBusiness = new ClientesBusiness();
        }

        protected int Base_UsuarioId { get; set; }
        protected string Base_NomeUsuario { get; set; }
        protected string Base_LoginUsuario { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            try
            {
                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket;

                    ticket = FormsAuthentication.Decrypt(cookie.Value);

                    var usuario = usuariosBusiness.ObterUsuario(ticket.Name);

                    Base_UsuarioId = usuario.Id;
                    Base_NomeUsuario = usuario.Nome;
                    Base_LoginUsuario = ticket.Name;

                    ViewBag.Base_UsuarioId = usuario.Id;
                    ViewBag.Base_NomeUsuario = usuario.Nome;
                    ViewBag.Base_EquipeUsuario = usuario.Equipe.Nome;
                    ViewBag.Base_LoginUsuario = ticket.Name;
                    ViewBag.Base_FotoUsuario = "data:image/png;base64," + usuario.Foto;

                    ViewBag.PedidosPendentesCliente = clientesBusiness.ObterTotalPedidosPendentesNotificacao();
                    ViewBag.TotalNotificacao = clientesBusiness.ObterTotalPedidosPendentesNotificacao().Count();
                }
            }
            catch (Exception ex)
            {
                filterContext.Result = RedirectToAction("Index", "Login");
            }
        }
    }
}