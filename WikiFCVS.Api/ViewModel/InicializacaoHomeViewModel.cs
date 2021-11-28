
using DevIO.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiFCVS.Api.Dto;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Api.ViewModel
{
    public class InicializacaoHomeViewModel
    {
        public LoginResponsavelViewModel LoginResponsavel { get; set; }

        public ArtigoEdicaoDto ArtigoEdicao { get; set; }

        public ICollection<TemaDto> Temas { get; set; }

        public InicializacaoHomeViewModel()
        {
            //TemasArtigos = new List<TemaArtigoDto>();
            Temas = new List<TemaDto>();
        }
    }
}
