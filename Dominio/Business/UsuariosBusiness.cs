using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Entidade.Gerais;
using SGD.Dominio.Entidade.Video;
using SGD.Dominio.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Dominio.Business
{
    public class UsuariosBusiness
    {
        public async Task<IList<Usuarios>> ListarUsuarios(int? equipeId = null, bool? isAtivo = null)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Usuarios.AsNoTracking()
                        .ToListAsync();
            }
        }

        public async Task<Usuarios> ObterUsuario(int id)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Usuarios
                    .Include(e => e.Equipe)
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<bool> MudarStatus(Usuarios usuario)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                if (usuario.IsAtivo)
                    usuario.IsAtivo = false;
                else
                    usuario.IsAtivo = true;

                ctx.Entry(usuario).State = EntityState.Modified;

                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> AtualizarUsuarioPerfil(Usuarios usuario)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.Entry(usuario).State = EntityState.Modified;

                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<Tuple<IList<Usuarios>, int>> ListarUsuarios(Paginacao paginacao)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var query = ctx.Usuarios
                    .Include(u => u.Equipe)
                    .OrderBy(t => t.Nome == paginacao.Ordenacao)
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(paginacao.TermoBusca))
                {
                    query = query.Where(t =>
                        t.Nome.Contains(paginacao.TermoBusca));
                }

                var total = await query.CountAsync();
                var itens = await query.Skip((paginacao.PaginaAtual - 1) * paginacao.TamanhoPagina)
                    .Take(paginacao.TamanhoPagina)
                    .ToListAsync();

                return new Tuple<IList<Usuarios>, int>(itens, total);
            }
        }

        public async Task<bool> InserirUsuario(Usuarios usuario)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.Usuarios.Add(usuario);

                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> ObterUsuario(Usuarios usuario)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var query = await ctx.Usuarios
                    .Where(u => u.Nome == usuario.Nome ||
                                u.Login == usuario.Login ||
                                u.Racf == usuario.Racf ||
                                u.Funcional == usuario.Funcional)
                    .FirstOrDefaultAsync();

                if (query != null)
                    return true;
                else
                    return false;
            }
        }

        public Usuarios ObterUsuario(string login)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return ctx.Usuarios
                    .Include(e => e.Equipe)
                    .Where(u => u.Login == login)
                    .FirstOrDefault();
            }
        }
    }
}
