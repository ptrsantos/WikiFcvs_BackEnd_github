using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class SalvarEdicaoVinculoTema
    {
        public TemaDto TemaInicial { get; set; }
        public TemaDto TemaSecundario { get; set; }
    }
}
