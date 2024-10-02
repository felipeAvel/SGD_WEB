using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominio.Entidade.Gerais
{
    public class AnexoDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public long Tamanho { get; set; }
        public string NomeArquivo { get; set; }
    }
}