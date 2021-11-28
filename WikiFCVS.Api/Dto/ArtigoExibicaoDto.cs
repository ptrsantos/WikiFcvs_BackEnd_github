using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class ArtigoExibicaoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataUltimaEdicao { get; set; }
        public string AutorUltimaEdicao { get; set; }
    }
}
