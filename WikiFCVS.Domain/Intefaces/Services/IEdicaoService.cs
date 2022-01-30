using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Intefaces.Services
{
    public interface IEdicaoService : IDisposable
    {

        //Task<Edicao> SalvarDados(Tema tema, Artigo artigo, Edicao edicao, Guid usuarioId, string usuarioEmail);
        Task<ICollection<EdicaoArtigo>> ListarArtigosEdicoesHistorico(int pagina, int tamanho);
        Task<EdicaoArtigo> RetornaEdicaoPaginaHome();
        Task<int> RetornaQuantidadeArtigoEdicoes();
        Task<EdicaoArtigo> RetornaArtigoEdicao(int edicaoId);
        Task<EdicaoArtigo> RetornaArtigoEdicaoHistorico(int edicaoId);
        Task<EdicaoArtigo> SalvarEdicao(Tema tema, Artigo artigo, EdicaoArtigo edicao, Guid usuarioId, string usuarioEmail);
        Task<EdicaoArtigo> RetornaArtigoEdicaoPorId(int edicaoId);
        Task<EdicaoArtigo> RetornaArtigoEdicaoPorArtigoId(int artigoId);
        Task<TransporteDadosTabela> ListarArtigosEdicoesHistoricoComFiltro(int pagina, int tamanho, string filtro);
        //Task<Edicao> SalvarDados(Tema tema, Artigo artigo, Guid usuarioId, string usuarioEmail);
        Task<EdicaoArtigo> SalvarInclusaoDados(EdicaoTema edicaoTema, EdicaoArtigo edicaoArtigo, Guid usuarioId, string usuarioEmail);
        Task<EdicaoArtigo> SalvarArtigoEdicao(Artigo artigo, EdicaoArtigo edicao, Guid usuarioId, string usuarioEmail);

    }
    
}