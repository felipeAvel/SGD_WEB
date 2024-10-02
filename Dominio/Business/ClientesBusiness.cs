using Dominio.Entidade.Gerais;
using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Entidade.ClientePedidos;
using SGD.Dominio.Entidade.Gerais;
using SGD.Dominio.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SGD.Dominio.Business
{
    public class ClientesBusiness
    {
        public async Task<Tuple<IList<ClientesPedido>, int>> ListarClientes(Paginacao paginacao)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var query = ctx.Clientes
                    .OrderBy(c => c.Nome == paginacao.Ordenacao)
                    .Include(c => c.Pedidos)
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

                return new Tuple<IList<ClientesPedido>, int>(itens, total);
            }
        }

        public async Task<Pedidos> ObterPedidos(int idPedido)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Pedidos
                    .AsNoTracking()
                    .Where(p => p.Id == idPedido)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<bool> EditarPedidos(Pedidos pedidos)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.Entry(pedidos).State = EntityState.Modified;

                return await ctx.SaveChangesAsync() > 0;
            }
        }


        public async Task<bool> EditarCliente(ClientesPedido cliente)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.Entry(cliente).State = EntityState.Modified;

                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> FinalizarPedido(int idPedido)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var pedido = await ctx.Pedidos
                    .AsNoTracking()
                    .Where(p => p.Id == idPedido)
                    .FirstOrDefaultAsync();

                pedido.Status = "Finalizado";

                ctx.Entry(pedido).State = EntityState.Modified;

                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<ClientesPedido> ObterCliente(int id)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Clientes
                    .Include(e => e.Pedidos)
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IList<Pedidos>> ObterPedidos()
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Pedidos
                    .Include(c => c.Cliente)
                    .Where(p => p.Status == "Pendente")
                    .OrderBy(p => p.DataEntrega)
                    .Take(10)
                    .ToListAsync();
            }
        }

        public async Task<int> ObterTotalCliente()
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Clientes
                    .Where(c => c.IsAtivo == true)
                    .CountAsync();
            }
        }

        public async Task<int> ObterTotalPedidosFinalizados()
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Pedidos
                    .Where(c => c.Status == "Finalizado")
                    .CountAsync();
            }
        }

        public async Task<int> ObterTotalPedidosPendentes()
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Pedidos
                    .Where(c => c.Status == "Pendente")
                    .CountAsync();
            }
        }

        public IList<Pedidos> ObterTotalPedidosPendentesNotificacao()
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return ctx.Pedidos
                    .Include(p => p.Cliente)
                    .Where(c => c.Status == "Pendente")
                    .ToList();
            }
        }

        public async Task<decimal> ObterTotalVendas()
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var total = ctx.Pedidos
                    .AsNoTracking()
                    .FirstOrDefault();

                if (total != null)
                    return await ctx.Pedidos
                        .Where(p => p.Status == "Finalizado")
                        .SumAsync(p => p.Valor);
                else
                    return 0;
            }
        }

        public async Task<bool> ObterCliente(ClientesPedido cliente)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var query = await ctx.Clientes
                    .Where(u => u.Nome == cliente.Nome ||
                                u.Telefone == cliente.Telefone)
                    .FirstOrDefaultAsync();

                if (query != null)
                    return true;
                else
                    return false;
            }
        }

        public async Task<bool> InserirCliente(ClientesPedido cliente)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.Clientes.Add(cliente);

                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> InserirIdAnexo(AnexoPedidos anexo)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.AnexoPedidos.Add(anexo);

                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<int> InserirPedidoCliente(Pedidos pedidos)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.Pedidos.Add(pedidos);

                await ctx.SaveChangesAsync();

                return pedidos.Id;
            }
        }

        public async Task<Tuple<IList<AnexoDTO>, ClientesPedido>> ObterClientePedidos(int id)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var cliente = await ctx.Clientes
                    .Include(e => e.Pedidos)
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();

                List<int> idsPedidos = new List<int>();

                foreach (var item in cliente.Pedidos)
                {
                    idsPedidos.Add(item.Id);
                }

                var anexoPedidos = await ctx.AnexoPedidos
                    .Where(a => idsPedidos.Contains(a.PedidoId))
                    .ToListAsync();

                List<int> idsAnexos = new List<int>();

                foreach (var item in anexoPedidos)
                {
                    idsAnexos.Add(item.AnexoId);
                }

                var anexo = await ctx.AnexoPedidos
                    .AsNoTracking()
                    .Include(a => a.Anexo)
                    .Include(a => a.Pedido)
                    .Where(a => idsAnexos.Contains(a.AnexoId))
                    .Select(a => new AnexoDTO
                    {
                        Id = a.Anexo.Id,
                        Tamanho = a.Anexo.Tamanho,
                        NomeArquivo = a.Anexo.NomeArquivo,
                        PedidoId = a.Pedido.Id
                    })
                    .ToListAsync();

                return new Tuple<IList<AnexoDTO>, ClientesPedido>(anexo, cliente);
            }
        }

        public async Task<Tuple<IList<AnexoDTO>, ClientesPedido>> ObterClientePedidos(int id, int idPedido)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var cliente = await ctx.Clientes
                    .AsNoTracking()
                    .Include(e => e.Pedidos)
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();

                var anexoPedidos = await ctx.AnexoPedidos
                    .AsNoTracking()
                    .Where(a => a.PedidoId == idPedido)
                    .ToListAsync();

                List<int> idsAnexos = new List<int>();

                foreach (var item in anexoPedidos)
                {
                    idsAnexos.Add(item.AnexoId);
                }

                var anexo = await ctx.AnexoPedidos
                    .AsNoTracking()
                    .Include(a => a.Anexo)
                    .Include(a => a.Pedido)
                    .Where(a => idsAnexos.Contains(a.AnexoId))
                    .Select(a => new AnexoDTO
                    {
                        Id = a.Anexo.Id,
                        Tamanho = a.Anexo.Tamanho,
                        NomeArquivo = a.Anexo.NomeArquivo,
                        PedidoId = a.Pedido.Id
                    })
                    .ToListAsync();

                return new Tuple<IList<AnexoDTO>, ClientesPedido>(anexo, cliente);
            }
        }
    }
}