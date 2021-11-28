using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WikiFCVS.Domain.Models
{
    public class Protocolo
    {
        public int Id { get; set; }
        public Guid EditadoPorId { get; set; }
        public string EditadoPorEmail { get; set; }
        public DateTime EditadoEm { get; set; }

        public EdicaoArtigo EdicaoArtigo { get; set; }
        public EdicaoTema EdicaoTema { get; set; }

        public Protocolo()
        {

        }

        public Protocolo(Guid usuarioId, string usuarioEmail)
        {
            EditadoPorId = usuarioId;
            EditadoPorEmail = usuarioEmail;
            EditadoEm = DateTime.Now;
        }
    }
}
