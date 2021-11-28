using System;
using System.Collections.Generic;
using System.Linq;

namespace WikiFCVS.Domain.Models
{
    public class Tema
    {
        public int Id { get; set; }

        /* EF Relation */

        public ICollection<Artigo> Artigos { get; set; }
        public ICollection<EdicaoTema> Edicoes { get; set; }

        public Tema()
        {
            Artigos = new List<Artigo>();
            Edicoes = new List<EdicaoTema>();
        }

        public Tema(Tema tema)
        {
            //Titulo = tema.Titulo;
            Artigos = new List<Artigo>();
        }

        public Tema(Artigo artigoDomain, Tema tema)
        {
            //Titulo = tema.Titulo;
            Artigos = new List<Artigo>();
            Artigos.Add(artigoDomain);
        }

        public Tema(Artigo artigoDomain, EdicaoTema edicaoTemaDomain) 
        {
            Artigos = new List<Artigo>();
            Edicoes = new List<EdicaoTema>();
            //Titulo = edicaoTemaDomain.Titulo;
            Artigos.Add(artigoDomain);
            Edicoes.Add(edicaoTemaDomain);
            edicaoTemaDomain.IncluirTema(this);
        }

        internal void AdicionarArtigoNaLista(Artigo artigoDomain)
        {
            Artigos.Add(artigoDomain);
        }


        internal void AdicionarEdicaoNaLista(EdicaoTema edicaoTemaDomain)
        {
            Edicoes.Add(edicaoTemaDomain);
        }

        public Artigo RetornaUltimoArtigo()
        {
            Artigo artigo = Artigos.LastOrDefault();
            return artigo;
            //return Artigos.LastOrDefault();
        }

        public EdicaoArtigo RetornaUltimaEdicaoDoUltimoArtigo()
        {
            EdicaoArtigo edicao = Artigos.LastOrDefault().Edicoes.LastOrDefault();
            return edicao;
            //return Artigos.LastOrDefault().Edicoes.LastOrDefault();
        }


        public EdicaoTema RetornaUltimaEdicaoDoTema()
        {
            return Edicoes.LastOrDefault();
        }

        public DateTime RetornaDataUltimaEdicao()
        {
            return Edicoes.LastOrDefault().EdicaoEfetuada.EditadoEm;
        }

        public string RetornaAutorUltimaEdicao() {
            return Edicoes.LastOrDefault().EdicaoEfetuada.EditadoPorEmail;
        }

        public string RetornaTituloAtual()
        {
            return this.Edicoes.LastOrDefault().Titulo;
        }
    }


}