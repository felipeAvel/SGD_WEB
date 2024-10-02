using Dominio.Business;
using SGD.Dominio.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SGD.Controllers
{
    public class HomeController : BaseController
    {

        private readonly ClientesBusiness clientesBusiness;

        public HomeController()
        {
            this.clientesBusiness = new ClientesBusiness();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            ViewBag.TotalCliente = await clientesBusiness.ObterTotalCliente();
            ViewBag.PedidosFinalizados = await clientesBusiness.ObterTotalPedidosFinalizados();
            ViewBag.PedidosPendentes = await clientesBusiness.ObterTotalPedidosPendentes();
            ViewBag.TotalVendas = await clientesBusiness.ObterTotalVendas();
            ViewBag.Top10Pedidos = await clientesBusiness.ObterPedidos();

            return View();
        }
    }
}