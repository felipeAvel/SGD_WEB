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

namespace SGD.Controllers
{
    public class VideoController : Controller
    {
        private readonly VideosBusiness videosBusiness;

        public VideoController()
        {
            this.videosBusiness = new VideosBusiness();
        }

        [HttpGet]
        public async Task<ActionResult> Index(Paginacao paginacao, string categoria)
        {
            var termo = paginacao.TermoBusca;
            var videos = await videosBusiness.ListarVideos(paginacao, categoria);

            ViewBag.ListaVideos = new StaticPagedList<VideosYT>(
                videos.Item1, paginacao.PaginaAtual,
                paginacao.TamanhoPagina, videos.Item2);
            
            return View(paginacao);
        }

        [HttpGet]
        public async Task<ActionResult> ExibirVideo(int? id = null)
        {
            if (!id.HasValue)
                return RedirectToAction("Index", "Video");

            var video = await videosBusiness.ObterVideo(id.Value);
            var vm = new VideoViewModels();
            vm.LoadFromModel(video);

            var videos = await videosBusiness.ListarVideos(video.Categoria);

            ViewBag.Video = videos;

            return View(vm);
        }

        [HttpGet]
        public async Task<ActionResult> EditarVideo(int? id = null)
        {
            if (!id.HasValue)
                return RedirectToAction("Index", "Home");

            var videos = await videosBusiness.ObterVideo(id.Value);
            var vm = new VideoViewModels();
            vm.LoadFromModel(videos);

            return View();
        }

        [HttpGet]
        public ActionResult InserirVideo()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> InserirVideo(InserirVideoViewModels vm)
        {
            var video = new VideosYT();

            video.Nome = vm.Nome;
            video.Url = vm.Url;
            video.CapaUrl = vm.CapaUrl;
            video.Categoria = vm.Categoria;
            video.LogoUrl = vm.LogoUrl;
            video.IsAtivo = true;

            if (await videosBusiness.InserirVideo(video))
                return Json("Sucesso", JsonRequestBehavior.AllowGet);
            else
                return Json("Erro ao Inserir Vídeo!", JsonRequestBehavior.AllowGet);
        }
    }
}