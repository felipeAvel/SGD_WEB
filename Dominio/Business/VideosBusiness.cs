using SGD.Dominio.Entidade.Gerais;
using SGD.Dominio.Entidade.Video;
using SGD.Dominio.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Business
{
    public class VideosBusiness
    {
        public async Task<VideosYT> ObterVideo(int id)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Videos.AsNoTracking()
                        .FirstOrDefaultAsync(v => v.Id == id);
            }
        }

        public async Task<Tuple<IList<VideosYT>, int>> ListarVideos(Paginacao paginacao, string categoria)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var query = ctx.Videos
                    .OrderBy(t => t.Nome == paginacao.Ordenacao)
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(paginacao.TermoBusca))
                {
                    query = query.Where(t =>
                        t.Categoria.Contains(paginacao.TermoBusca));
                }

                if (!string.IsNullOrEmpty(categoria))
                {
                    query = query.Where(t =>
                        t.Categoria.Contains(categoria));
                }

                var total = await query.CountAsync();
                var itens = await query.Skip((paginacao.PaginaAtual - 1) * paginacao.TamanhoPagina)
                    .Take(paginacao.TamanhoPagina)
                    .ToListAsync();

                return new Tuple<IList<VideosYT>, int>(itens, total);
            }
        }

        public async Task<IList<VideosYT>> ListarVideos(string categoria)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Videos.Where(v => v.Categoria
                                        .Contains(categoria))
                                        .Take(20)
                                        .ToListAsync();
            }
        }

        public async Task<bool> InserirVideo(VideosYT video)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.Videos.Add(video);

                return await ctx.SaveChangesAsync() > 0;
            }
        }
    }
}