using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiFCVS.Api.Dto;

namespace WikiFCVS.Api.ViewModel
{
    public class EstatisticaViewModel
    {
        public ICollection<MesTotalDto> ListaEstatisticaUsuarios { get; set; }
        public ICollection<MesTotalDto> ListaEstatisticaArtigos { get; set; }
        public ICollection<MesTotalDto> ListaEstatisticaEdicoes { get; set; }

        public EstatisticaViewModel()
        {
            ListaEstatisticaUsuarios = new List<MesTotalDto>();
            ListaEstatisticaArtigos = new List<MesTotalDto>();
            ListaEstatisticaEdicoes = new List<MesTotalDto>();
        }
    }
}
