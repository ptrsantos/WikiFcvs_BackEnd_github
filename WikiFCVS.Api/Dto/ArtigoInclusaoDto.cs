using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class ArtigoInclusaoDto
    {
        public int Id { get; set; }
        public EdicaoArtigoDto Edicao { get; set; }
    }
}
