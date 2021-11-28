using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Services
{
    public class RegistroUsuarioService : IRegistroUsuarioService
    {
        private readonly IRegistroUsuarioRepository RegistroUsuarioRepository;

        public RegistroUsuarioService(IRegistroUsuarioRepository registroUsuarioRepository)
        {
            RegistroUsuarioRepository = registroUsuarioRepository;
        }


        public async Task SalvarRegistroDaInclusaoDoUsuario(string id)
        {
            try
            {
                RegistroUsuario registroUsuario;
                registroUsuario = await RegistroUsuarioRepository.RetornaUsuarioPorUsuarioId(id);
                if(registroUsuario == null)
                {
                    registroUsuario = new RegistroUsuario(id);
                    await RegistroUsuarioRepository .Salvar(registroUsuario);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Dispose()
        {
            RegistroUsuarioRepository?.Dispose();
        }
    }
}
