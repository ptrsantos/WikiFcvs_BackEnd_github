using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WikiFCVS.Domain.Models
{
    public class EdicaoTema
    {
        //private EdicaoTema edicaoTema;
        //private Guid usuarioId;
        //private string usuarioEmail;

        public int Id { get; set; }
        public string Titulo { get; set; }

        public Tema Tema { get; set; }
        public int TemaId { get; set; }

        public Protocolo EdicaoEfetuada { get; set; }
        public int EdicaoEfetuadaId { get; set; }

        public EdicaoTema()
        {

        }

        public EdicaoTema(string temaTitulo, Guid usuarioId, string usuarioEmail)
        {
            EdicaoEfetuada = new Protocolo(usuarioId, usuarioEmail);
            Titulo = temaTitulo;
            //this.usuarioId = usuarioId;
            //this.usuarioEmail = usuarioEmail;
        }

        public EdicaoTema(EdicaoTema edicaoTema, Guid usuarioId, string usuarioEmail)
        {
            EdicaoEfetuada = new Protocolo(usuarioId, usuarioEmail);
            Titulo = edicaoTema.Titulo;
            //this.edicaoTema = edicaoTema;
            //this.usuarioId = usuarioId;
            //this.usuarioEmail = usuarioEmail;
        }

        internal void IncluirTema(Tema tema)
        {
            Tema = tema;
        }
    }
}
