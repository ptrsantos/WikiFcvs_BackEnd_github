using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class TemaInclusaoDto
    {
        //public int Id { get; set; }
        public EdicaoTemaDto Edicao { get; set; }
        public ArtigoDto Artigo { get; set; }

    }
}
