using System;
using System.Collections.Generic;
using System.Linq;

namespace WikiFCVS.Domain.Models
{
    public class Artigo
    {
        public int Id { get; set; }

        /* EF Relations */
        public Tema Tema { get; set; }
        //public int TemaId { get; set; }
        public ICollection<EdicaoArtigo> Edicoes { get; set; }
        public virtual EdicaoArtigo Edicao { get; set; }

        public Artigo()
        {
            Edicoes = new List<EdicaoArtigo>();
        }

        public Artigo(Artigo artigo)
        {
            Tema = artigo.Tema;
            Edicoes = artigo.Edicoes;
        }

        public EdicaoArtigo RetornaUltimaEdicao()
        {
            return Edicoes.FirstOrDefault();
        }

        //public Artigo(Guid usuarioId, string usuarioEmail, Artigo artigo)
        //{
        //    EdicaoEfetuada = new Protocolo(usuarioId, usuarioEmail);
        //    //Titulo = artigo.Titulo;
        //    //Descricao = artigo.Descricao;
        //    Edicoes = new List<Edicao>();
        //}

        public void IncluirTema(Tema tema)
        {
            Tema = tema;
        }

        public Artigo(Guid usuarioId)
        {
            Edicoes = new List<EdicaoArtigo>();
        }

        //public Artigo(Guid usuarioId, string usuarioEmail, Edicao edicaoDomain)
        //{
        //    EdicaoEfetuada = new Protocolo(usuarioId, usuarioEmail);
        //    this.Edicoes = new List<Edicao>();
        //    this.Edicoes.Add(edicaoDomain);
        //    //this.usuarioEmail = usuarioEmail;
        //    edicaoDomain.IncluirArtigo(this);

        //}

        public void AdicionarEdicaoNaLista(EdicaoArtigo edicao)
        {
            Edicoes.Add(edicao);
        }


    }
}