using SGD.Dominio.Entidade.ClientePedidos;
using SGD.Dominio.Entidade.Gerais;
using SGD.Dominio.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Dominio.Business
{
    public class AnexosBusiness
    {
        public async Task<Anexos> ObterAnexo(int id)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Anexos
                    .Where(a => a.Id == id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<int> AdicionarAnexo(Anexos anexos)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.Anexos.Add(anexos);
                await ctx.SaveChangesAsync();

                return anexos.Id;
            }
        }
    }
}