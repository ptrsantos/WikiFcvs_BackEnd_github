using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto
{
    public class MesTotalDto
    {
        public DateTime DataRegistro { get; set; }
        public int AnoMumerico { get; set; }
        public int MesNumerico { get; set; }
        public string MesLiteral { get; set; }
        public int Total { get; set; }
    }
}
