using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class TemaArtigoDto
    {
        public int TemaId { get; set; }
        public string TemaTitulo { get; set; }
        public string ArtigoId { get; set; }
        public string ArtigoTitulo { get; set; }
        public string ArtigoDescricao { get; set; }
    }
}
