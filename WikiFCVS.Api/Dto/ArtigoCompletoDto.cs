using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class ArtigoCompletoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public Guid EditadoPorId { get; set; }
        public string EditadoPorEmail { get; set; }
        public DateTime EditadoEm { get; set; }

        public TemaCompletoDto Tema { get; set; }
        public ICollection<EdicaoCompletaDto> Edicoes { get; set; }
    }
}
