using System;
using System.Collections.Generic;
using System.Text;
using WikiFCVS.Data.Context;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WikiFCVS.Data.Repository
{
    public class RegistroUsuarioRepository : RepositoryBase<RegistroUsuario>, IRegistroUsuarioRepository
    {
        public RegistroUsuarioRepository(WikiFCVSContext context) : base(context) { }

        public async Task<ICollection<RegistroUsuario>> ListarRegistrosUsuariosPorSemestre()
        {
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;
            //DateTime dataAtual = new DateTime(ano, mes, 1);
            DateTime dataAtual = DateTime.Now;
            DateTime dataAnterior = dataAtual.AddMonths(-6);

            ICollection<RegistroUsuario> listaRegistroUsuarios = await DbSet
                                                                .Where(r => r.DataRegistro <= dataAtual 
                                                                || r.DataRegistro >= dataAnterior).OrderBy(r => r.DataRegistro).ToListAsync();
            return listaRegistroUsuarios;
        }

        public async Task<RegistroUsuario> RetornaUsuarioPorUsuarioId(string id)
        {
            return await DbSet.FirstOrDefaultAsync(r => r.UsuarioId == id);
        }
    }
}
