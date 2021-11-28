using System;
using System.Collections.Generic;
using System.Text;

namespace WikiFCVS.Domain.Models
{
    public class TransporteDadosTabela
    {
        public ICollection<EdicaoArtigo> Edicoes { get; set; }
        public int TotalDeItens { get; set; }
    }
}
