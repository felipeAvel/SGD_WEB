using Dominio.Business;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SGD.Controllers
{
    [AllowAnonymous]
    public class AnexosController : BaseController
    {
        private readonly AnexosBusiness anexosBusiness;

        public AnexosController()
        { 
         anexosBusiness = new AnexosBusiness();
        }

        [HttpGet]
        public async Task<FileResult> Download(int? id)
        {
            if (!id.HasValue)
                return null;

            var anexo = await anexosBusiness.ObterAnexo(id.Value);

            if (anexo == null)
                return null;

            var b = Convert.FromBase64String(anexo.B64.ToString());

            string textEncode = System.Web.HttpUtility.UrlEncode(anexo.NomeArquivo, Encoding.GetEncoding("iso-8859-7"));
            string textDecode = System.Web.HttpUtility.UrlDecode(textEncode);

            var cd = new ContentDisposition
            {
                FileName = textDecode,
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(b, anexo.TipoArquivo);
        }
    }
}