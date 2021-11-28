using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class ArtigoDto
    {
        public int Id { get; set; }
        public EdicaoArtigoDto EdicaoArtigo { get; set; }
    }
}
