using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiFCVS.Api.Dto;

namespace WikiFCVS.Api.ViewModel
{
    public class TemaEdicaoViewModel
    {
        public TemaDto Tema { get; set; }
        public ICollection<TemaDto> ListaTemas { get; set; }

        public TemaEdicaoViewModel()
        {
            ListaTemas = new List<TemaDto>();
        }
    }
}
