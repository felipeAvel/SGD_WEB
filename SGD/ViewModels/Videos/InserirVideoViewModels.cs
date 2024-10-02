using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SGD.ViewModels.Videos
{
    public class InserirVideoViewModels
    {
        [DisplayName("Nome do Vídeo")]
        public string Nome { get; set; }

        [DisplayName("Url do Vídeo")]
        public string Url { get; set; }

        [DisplayName("Url da Logo do Canal")]
        public string LogoUrl { get; set; }

        [DisplayName("Url da Capa do Vídeo")]
        public string CapaUrl { get; set; }

        [DisplayName("Categoria do Vídeo")]
        public string Categoria { get; set; }

    }
}