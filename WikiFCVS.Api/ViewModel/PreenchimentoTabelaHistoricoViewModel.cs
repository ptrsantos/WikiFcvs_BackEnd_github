using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiFCVS.Api.Dto;

namespace WikiFCVS.Api.ViewModel
{
    public class PreenchimentoTabelaHistoricoViewModel
    {
        public ICollection<TemaArtigoDataTableDto> ListaArtigosEdicoes { get; set; }
        public int QuantidadeItens { get; set; }

        public PreenchimentoTabelaHistoricoViewModel()
        {
            ListaArtigosEdicoes = new List<TemaArtigoDataTableDto>();
        }
    }
}
