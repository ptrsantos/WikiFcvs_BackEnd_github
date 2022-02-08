using System;
using System.Collections.Generic;
using System.Text;

namespace WikiFCVS.Domain.Models
{
    public class MesTotal
    {
        public DateTime DataRegistro { get; set; }
        public int AnoMumerico { get; set; }
        public int MesNumerico { get; set; }
        public string MesLiteral { get; set; }
        public int Total { get; set; }
    }
}
