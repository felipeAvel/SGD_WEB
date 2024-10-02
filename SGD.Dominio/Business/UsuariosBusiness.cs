using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Dominio.Business
{
    public class UsuariosBusiness
    {
        public async Task<IList<Usuario>> ListarUsuarios(int? equipeId = null, bool? isAtivo = null) 
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext()) 
            {
                var query = ctx.usuarios.Where(a => a.IsAtivo == true);

                return await query.ToListAsync();
            }
        }
    }
}
