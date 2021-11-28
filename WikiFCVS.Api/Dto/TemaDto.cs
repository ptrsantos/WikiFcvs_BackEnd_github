using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class TemaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string AutorId { get; set; }
        public string AutorUltimaEdicao { get; set; }
        public DateTime DataUltimaEdicao { get; set; }
        public ICollection<ArtigoExibicaoDto> Artigos { get; set; }



        /* EF Relation */
        public int ArtigoId { get; set; }
    }
}
