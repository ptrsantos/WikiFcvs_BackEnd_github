using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class EdicaoCompletaDto
    {
        public int Id { get; set; }
        public string Conteudo { get; set; }
        public Guid EditadoPorId { get; set; }
        public string EditadoPorEmail { get; set; }
        public DateTime EditadoEm { get; set; }

    }
}
