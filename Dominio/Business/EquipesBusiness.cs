using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Entidade.Gerais;
using SGD.Dominio.Entidade.Video;
using SGD.Dominio.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Business
{
    public class EquipesBusiness
    {
        public async Task<IList<Equipes>> ListarEquipes()
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.Equipes.AsNoTracking()
                        .ToListAsync();
            }
        }
    }
}