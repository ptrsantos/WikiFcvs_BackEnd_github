using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class ArtigoEdicaoDto
    {
        public int EdicaoId { get; set; }

        public string EdicaoConteudo { get; set; }

        public DateTime EdicaoData { get; set; }

        public string ReponsavelEmail { get; set; }

        public string ArtigoId { get; set; }

        public string ArtigoTitulo { get; set; }

        public string TemaTitulo { get; set; }

        public string TemaId { get; set; }
    }
}
