using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGD.Dominio.Entidade.Gerais;

namespace SGD.Dominio.Entidade.Gerais
{
    public class Paginacao
    {
        private int _paginaAtual;

        public int PaginaAtual
        { 
            get { return _paginaAtual <= 0 ? 1 : _paginaAtual; }
            set { _paginaAtual = value < 0 ? 1 : value; }
        }

        public int IndicePaginaAtual
        {
            get { return PaginaAtual - 1; }
        }

        private int _tamanhoPagina;

        public int TamanhoPagina
        {
            get { return _tamanhoPagina <= 0 ? 10 : _tamanhoPagina; }
            set { _tamanhoPagina = value <= 0 ? 10 : value; }
        }

        private string _ordenacao;

        public string Ordenacao 
        {
            get { return string.IsNullOrWhiteSpace(_ordenacao) ? "Id desc" : _ordenacao; }
            set { _ordenacao = value; }
        }

        private string _termoBusca;

        public string TermoBusca 
        {
            get { return _termoBusca?.ToLower(); }
            set { _termoBusca = value; }
        }
    }
}