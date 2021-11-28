using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiFCVS.Domain.Intefaces.Services
{
    public interface IRegistroUsuarioService : IDisposable
    {
        Task SalvarRegistroDaInclusaoDoUsuario(string id);
    }
}
