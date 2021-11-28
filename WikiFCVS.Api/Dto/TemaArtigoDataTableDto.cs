using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class TemaArtigoDataTableDto
    {
        public int EdicaoId { get; set; }
        public int ArtigoId { get; set; }
        public string EdicaoConteudoResumo { get; set; }
        public DateTime EdicaoData { get; set; }
        public string ResponsavelEmail { get; set; }
        public string ArtigoTitulo { get; set; }
        public string TemaTitulo { get; set; }
    }
}
