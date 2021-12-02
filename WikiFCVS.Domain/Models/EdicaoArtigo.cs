using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Http;
using WikiFCVS.Domain.Handle;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Linq;
using System.Text.Encodings.Web;
using HtmlAgilityPack;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using System.Linq;
using System.Net;

namespace WikiFCVS.Domain.Models
{
    public class EdicaoArtigo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }

        public Artigo Artigo { get; set; }
        public int ArtigoId { get; set; }

        public Protocolo EdicaoEfetuada { get; set; }
        public int EdicaoEfetuadaId { get; set; }

        public EdicaoArtigo()
        {

        }

        public EdicaoArtigo(EdicaoArtigo edicao, Guid usuarioiId, string usuarioEmail)
        {
            EdicaoEfetuada = new Protocolo(usuarioiId, usuarioEmail);
            EdicaoEfetuada.EdicaoArtigo = this;
            Titulo = edicao.Titulo;
            Conteudo = edicao.Conteudo;
         }

        public EdicaoArtigo(EdicaoArtigo edicaoArtigo)
        {
            Titulo = edicaoArtigo.Titulo;
            Conteudo = edicaoArtigo.Conteudo;
        }


        internal void IncluirArtigo(Artigo artigo)
        {
            Artigo = artigo;
        }



        public string RetornaResumoDoConteudo()
        {
            var agleSharpUtilies = new AngleSharpUtilies();
            var plainText = agleSharpUtilies.ConvertHtmlToText(Conteudo);

            int tamanho = plainText.Length;
            if (tamanho <= 60)
            {
                return $"{plainText.Substring(0, tamanho)}...";
            }
            return $"{plainText.Substring(0, 60)}...";
        }

        public string RetornaResumoDoConteudoComFiltro(string filtro)
        {
            var agleSharpUtilies = new AngleSharpUtilies();
            var plainText = agleSharpUtilies.ConvertHtmlToText(Conteudo);

            int tamanho = plainText.Length;
            if (tamanho <= 60)
            {
                return $"{plainText.Substring(0, tamanho)}...";
            }
            return $"{plainText.Substring(0, 60)}...";

        }

        public string RetornaResumoDoTituloComFiltro(string filtro)
        {
            var agleSharpUtilies = new AngleSharpUtilies();
            var plainText = agleSharpUtilies.ConvertHtmlToText(Titulo);

            int tamanho = plainText.Length;
            if (tamanho <= 60)
            {
                return $"{plainText.Substring(0, tamanho)}...";
            }
            return $"{plainText.Substring(0, 60)}...";
        }


        public string RetornaConteudoEmTextoPuro()
        {
            var agleSharpUtilies = new AngleSharpUtilies();
            var plainText = agleSharpUtilies.ConvertHtmlToText(Conteudo);
            string conteudoTratado = plainText; 
            return conteudoTratado;
        }


        internal string RetornaTituloEmTextoPuro()
        {
            var agleSharpUtilies = new AngleSharpUtilies();
            var plainText = agleSharpUtilies.ConvertHtmlToText(Titulo);
            return plainText;

        }

        internal void AtualizarConteudo(Guid usuarioiId, string usuarioEmail, string titulo, string conteudo)
        {
            EdicaoEfetuada = new Protocolo(usuarioiId, usuarioEmail);
            Titulo = titulo;
            Conteudo = conteudo;
        }

        private string GetPlainTextFromHtml(string htmlString)
        {

             Regex[] _htmlReplaces = new[] {
                            new Regex(@"<script\b[^<]*(?:(?!</script>)<[^<]*)*</script>", RegexOptions.Compiled | RegexOptions.Singleline, TimeSpan.FromSeconds(1)),
                            new Regex(@"<style\b[^<]*(?:(?!</style>)<[^<]*)*</style>", RegexOptions.Compiled | RegexOptions.Singleline, TimeSpan.FromSeconds(1)),
                            new Regex(@"<[^>]*>", RegexOptions.Compiled),
                            new Regex(@" +", RegexOptions.Compiled)
                        };

            foreach (var r in _htmlReplaces)
            {
                htmlString = r.Replace(htmlString, " ");
            }
            var lines = htmlString
                .Split(new[] { '\r', '\n' })
                .Select(_ => WebUtility.HtmlDecode(_.Trim()))
                .Where(_ => _.Length > 0)
                .ToArray();
            return string.Join("\n", lines);

        }

        internal void ConteudoTextoPuroParaHtml()
        {

            string text = "";
            this.Conteudo = this.Conteudo.Replace("&para;", "").Replace("&#182;", "");
            text = HttpUtility.HtmlDecode(this.Conteudo);
            text = text.Replace("\r\n", "<br>");
            //text = text.Replace("\n", "");
            //text = text.Replace("\r", "<br>");
            text = text.Replace("  ", "&nbsp;");
            text = text.Replace(Environment.NewLine, "<br/>");
            //text = text.Replace("  ", "&para;");
            //text = text.Replace("  ", "&#182;");
            this.Conteudo = "<div>" + text  + "</div>";

        }

       
    }
}
