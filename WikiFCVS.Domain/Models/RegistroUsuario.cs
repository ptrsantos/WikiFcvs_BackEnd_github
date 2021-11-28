using System;
using System.Collections.Generic;
using System.Text;

namespace WikiFCVS.Domain.Models
{
    public class RegistroUsuario
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public DateTime DataRegistro { get; set; }



        public RegistroUsuario(string usuarioId)
        {
            UsuarioId = usuarioId;
            DataRegistro = DateTime.Now;
        }

    }
}
