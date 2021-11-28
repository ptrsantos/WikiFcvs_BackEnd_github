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
            //string conteudoSemTagLink = Regex.Replace(Conteudo, @"<a[^>]*>(.*?)</a>", String.Empty, RegexOptions.ECMAScript);
            //string plainText;
            //plainText = ConvertToPlain2(Conteudo);
            //plainText = GetPlainTextFromHtml(Conteudo);


            Chilkat.HtmlToText h2t = new Chilkat.HtmlToText();
            h2t.RightMargin = 300;
            h2t.DecodeHtmlEntities = true;
            h2t.SuppressLinks = true;
            string plainText = h2t.ToText(Conteudo);

            int tamanho = plainText.Length;
            if (tamanho <= 60)
            {
                return $"{plainText.Substring(0, tamanho)}...";
            }
            return $"{plainText.Substring(0, 60)}...";
        }

        public string RetornaResumoDoConteudoComFiltro(string filtro)
        {
            //string conteudoSemTagLink = Regex.Replace(Conteudo, @"<a[^>]*>(.*?)</a>", String.Empty, RegexOptions.ECMAScript);
            //string plainText;
            //plainText = ConvertToPlain2(Conteudo);
            //plainText = GetPlainTextFromHtml(Conteudo);


            Chilkat.HtmlToText h2t = new Chilkat.HtmlToText();
            h2t.RightMargin = 300;
            h2t.SuppressLinks = true;
            h2t.DecodeHtmlEntities = true;
            string plainText = h2t.ToText(Conteudo);

            int tamanho = plainText.Length;
            if (tamanho <= 60)
            {
                return $"{plainText.Substring(0, tamanho)}...";
            }
            return $"{plainText.Substring(0, 60)}...";

        }

        public string RetornaResumoDoTituloComFiltro(string filtro)
        {
            //string conteudoSemTagLink = Regex.Replace(Conteudo, @"<a[^>]*>(.*?)</a>", String.Empty, RegexOptions.ECMAScript);
            ////plainText = ConvertToPlain2(Conteudo);
            //plainText = GetPlainTextFromHtml(Conteudo);

            string plainText;
            Chilkat.HtmlToText h2t = new Chilkat.HtmlToText();
            h2t.RightMargin = 300;
            h2t.SuppressLinks = true;
            h2t.DecodeHtmlEntities = true;
            plainText = h2t.ToText(Conteudo);

            int tamanho = plainText.Length;
            if (tamanho <= 60)
            {
                return $"{plainText.Substring(0, tamanho)}...";
            }
            return $"{plainText.Substring(0, 60)}...";
        }


        public string RetornaConteudoEmTextoPuro()
        {

            //string plainText;
            //plainText = ConvertToPlain(Conteudo);
            //plainText = GetPlainTextFromHtml(Conteudo);
            //return conteudoTratado;
            //return plainText;

            Chilkat.HtmlToText h2t = new Chilkat.HtmlToText();
            h2t.RightMargin = 300;
            h2t.SuppressLinks = true;
            h2t.DecodeHtmlEntities = true;
            string plainText = h2t.ToText(Conteudo);
            string conteudoTratado = plainText.Substring(4, plainText.Length - 4); 
            //return plainText;
            return conteudoTratado;
        }


        internal string RetornaTituloEmTextoPuro()
        {
            //string plainText;
            //plainText = ConvertToPlain2(Titulo);
            //plainText = GetPlainTextFromHtml(Titulo);

            //string tituloTratado = plainText; //.Replace("&para;", "").Replace("&#182;", ""); 
            ////return tituloTratado;
            //string tituloTratado2 = plainText2;
            //return tituloTratado2;

            Chilkat.HtmlToText h2t = new Chilkat.HtmlToText();
            h2t.RightMargin = 300;
            h2t.SuppressLinks = true;
            string plainText = h2t.ToText(Titulo);
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

            //HtmlUtilitiesHandle
            var text = HTML.FormatLineBreaks(htmlString);//Razoável
            var textoPlano = HtmlUtilitiesHandle.ConvertToPlainText(htmlString);//Precisa de ajustes


            //string htmlTagPattern = "<.*?>";
            //var regexCss = new Regex("(\\<script(.+?)\\)|(\\<style(.+?)\\)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //htmlString = regexCss.Replace(htmlString, string.Empty);
            //htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            //htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            //htmlString = htmlString.Replace(" ", string.Empty);

            //return textoPlano;
            return text;
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



        //private string StripHtml(string source)
        //{
        //    string output;

        //    //get rid of HTML tags
        //    output = Regex.Replace(source, "<[^>]*>", string.Empty);

        //    //get rid of multiple blank lines
        //    //output = Regex.Replace(output, @"^\s*$\n", string.Empty, RegexOptions.Multiline);

        //    return output;
        //}

        //internal void ConteudoTextoPuroParaHtml()
        //{
        //    string text;
        //    text = HttpUtility.HtmlDecode(this.Conteudo);
        //    //text = HttpUtility.HtmlEncode(this.Conteudo);
        //    text = text.Replace("\r\n", "");
        //    text = text.Replace("\n", "");
        //    text = text.Replace("\r", "<br>");
        //    text = text.Replace("  ", " &nbsp;");
        //    text = "<div>" + text + "</div>";
        //    this.Conteudo = text;
        //}


        //internal static string RemoveTag(string source)
        //{
        //    //var retornoAux = Regex.Replace(source, @"<[^>]*>", String.Empty, RegexOptions.ECMAScript);
        //    //var retornoAux = Regex.Replace(source, @"<(.*?)>", String.Empty, RegexOptions.ECMAScript);
        //    var retornoAux = Regex.Replace(source, @"&(.*?);", String.Empty, RegexOptions.ECMAScript);
        //    Regex _notOkCharacter_ = new Regex(@"[^\w;&#@.:/\\?=|%!() -]", RegexOptions.Compiled);
        //    string retorno = _notOkCharacter_.Replace(source, " ");
        //    //return Regex.Replace(source, @"<[^>]*>", string.Empty);

        //    string conteudoSemTagLink = Regex.Replace(source, @"<a[^>]*>(.*?)</a>", String.Empty, RegexOptions.ECMAScript);
        //    string conteudoSemCaracteresEspeciais = Regex.Replace(conteudoSemTagLink, @"&(.*?);", String.Empty, RegexOptions.ECMAScript);
        //    string conteudoSemTags = Regex.Replace(conteudoSemCaracteresEspeciais, @"<[^>]*>", String.Empty, RegexOptions.ECMAScript);

        //    return conteudoSemTags;
        //}


        ///*Teste*/

        //private String UnHtml(String html)
        //{
        //   Regex _tags_ = new Regex(@"<[^>]+?>", RegexOptions.Multiline | RegexOptions.Compiled);

        //   Regex _notOkCharacter_ = new Regex(@"[^\w;&#@.:/\\?=|%!() -]", RegexOptions.Compiled);

        //html = HttpUtility.UrlDecode(html);
        //    html = HttpUtility.HtmlDecode(html);

        //    html = RemoveTag(html, "<!--", "-->");
        //    html = RemoveTag(html, "<script", "</script>");
        //    html = RemoveTag(html, "<style", "</style>");

        //    //replace matches of these regexes with space
        //    //html = _tags_.Replace(html, " ");
        //    html = _notOkCharacter_.Replace(html, " ");
        //    //html = SingleSpacedTrim(html);

        //    return html;
        //}

        //private String RemoveTag(String html, String startTag, String endTag)
        //{
        //    Boolean bAgain;
        //    do
        //    {
        //        bAgain = false;
        //        Int32 startTagPos = html.IndexOf(startTag, 0, StringComparison.CurrentCultureIgnoreCase);
        //        if (startTagPos < 0)
        //            continue;
        //        Int32 endTagPos = html.IndexOf(endTag, startTagPos + 1, StringComparison.CurrentCultureIgnoreCase);
        //        if (endTagPos <= startTagPos)
        //            continue;
        //        html = html.Remove(startTagPos, endTagPos - startTagPos + endTag.Length);
        //        bAgain = true;
        //    } while (bAgain);
        //    return html;
        //}

        //private String SingleSpacedTrim(String inString)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    Boolean inBlanks = false;
        //    foreach (Char c in inString)
        //    {
        //        switch (c)
        //        {
        //            case '\r':
        //            case '\n':
        //            case '\t':
        //            case ' ':
        //                if (!inBlanks)
        //                {
        //                    inBlanks = true;
        //                    sb.Append(' ');
        //                }
        //                continue;
        //            default:
        //                inBlanks = false;
        //                sb.Append(c);
        //                break;
        //        }
        //    }
        //    return sb.ToString().Trim();
        //}


        //private string ConvertToPlain2(string html)
        //{
        //    //Regex quebraDeLinhaRegex = new Regex("</p>|<br/|<br>", RegexOptions.Compiled | RegexOptions.Singleline);
        //Regex inlineTagRegex = new Regex("<\\/?(a|span|sub|sup|b|i|strong|small|big|em|label|q)[^>]*>", RegexOptions.Compiled | RegexOptions.Singleline);
        //Regex scriptRegex = new Regex("<(script|style)[^>]*?>.*?</\\1>", RegexOptions.Compiled | RegexOptions.Singleline);
        //Regex tagRegex = new Regex("<[^>]+>", RegexOptions.Compiled | RegexOptions.Singleline);
        //Regex multiWhitespaceRegex = new Regex("\\s+", RegexOptions.Compiled | RegexOptions.Singleline);

        //if (html == null)
        //{
        //    return html;
        //}

        //html = quebraDeLinhaRegex.Replace(html, @"\r\n");
        //html = scriptRegex.Replace(html, string.Empty);
        //html = inlineTagRegex.Replace(html, string.Empty);
        //html = tagRegex.Replace(html, " ");
        //html = HttpUtility.HtmlDecode(html);
        //html = multiWhitespaceRegex.Replace(html, " ");

        //string sText = HTML.HTML_to_Text(html);

        //string value = System.Net.WebUtility.HtmlDecode(html);
        //string value = System.Net.WebUtility.HtmlEncode(html);
        //return value;

        //var html_saida = XDocument.Parse(html);
        //var sText = Html2Text(html_saida);

        ////return html.Trim();
        //return sText;

        //}

        //private string Html2Text(XDocument source)
        //{
        //    var writer = new StringWriter();
        //    Html2Text(source, writer);
        //    return writer.ToString();
        //}

        //private void Html2Text(XDocument source, StringWriter output)
        //{
        //    Transformer.Transform(source.CreateReader(), null, output);
        //}

        //public static XslCompiledTransform _transformer;
        //public static XslCompiledTransform Transformer
        //{
        //    get
        //    {
        //        if (_transformer == null)
        //        {
        //            _transformer = new XslCompiledTransform();
        //            var xsl = XDocument.Parse(@"<?xml version='1.0'?><xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" exclude-result-prefixes=""xsl""><xsl:output method=""html"" indent=""yes"" version=""4.0"" omit-xml-declaration=""yes"" encoding=""UTF-8"" /><xsl:template match=""/""><xsl:value-of select=""."" /></xsl:template></xsl:stylesheet>");
        //            _transformer.Load(xsl.CreateNavigator());
        //        }
        //        return _transformer;
        //    }
        //}

        //private string ConvertToPlain(string source)
        ////private string StripHTML(string source)
        //{
        //    try
        //    {
        //        string result;

        //        // Remove HTML Development formatting
        //        // Replace line breaks with space
        //        // because browsers inserts space
        //        result = source.Replace("\r", " ");
        //        // Replace line breaks with space
        //        // because browsers inserts space
        //        result = result.Replace("\n", " ");
        //        // Remove step-formatting
        //        result = result.Replace("\t", string.Empty);
        //        // Remove repeating spaces because browsers ignore them
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                                                              @"( )+", " ");

        //        // Remove the header (prepare first by clearing attributes)
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<( )*head([^>])*>", "<head>",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"(<( )*(/)( )*head( )*>)", "</head>",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 "(<head>).*(</head>)", string.Empty,
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // remove all scripts (prepare first by clearing attributes)
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<( )*script([^>])*>", "<script>",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"(<( )*(/)( )*script( )*>)", "</script>",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        //result = System.Text.RegularExpressions.Regex.Replace(result,
        //        //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
        //        //         string.Empty,
        //        //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"(<script>).*(</script>)", string.Empty,
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // remove all styles (prepare first by clearing attributes)
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<( )*style([^>])*>", "<style>",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"(<( )*(/)( )*style( )*>)", "</style>",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 "(<style>).*(</style>)", string.Empty,
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // insert tabs in spaces of <td> tags
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<( )*td([^>])*>", "\t",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // insert line breaks in places of <BR> and <LI> tags
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<( )*br( )*>", "\r",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<( )*li( )*>", "\r",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // insert line paragraphs (double line breaks) in place
        //        // if <P>, <DIV> and <TR> tags
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<( )*div([^>])*>", "\r\r",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<( )*tr([^>])*>", "\r\r",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<( )*p([^>])*>", "\r\r",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // Remove remaining tags like <a>, links, images,
        //        // comments etc - anything that's enclosed inside < >
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"<[^>]*>", string.Empty,
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // replace special characters:
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @" ", " ",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&bull;", " * ",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&lsaquo;", "<",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&rsaquo;", ">",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&trade;", "(tm)",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&frasl;", "/",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&lt;", "<",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&gt;", ">",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&copy;", "(c)",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&reg;", "(r)",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        // Remove all others. More can be added, see
        //        // http://hotwired.lycos.com/webmonkey/reference/special_characters/
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 @"&(.{2,6});", string.Empty,
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // for testing
        //        //System.Text.RegularExpressions.Regex.Replace(result,
        //        //       this.txtRegex.Text,string.Empty,
        //        //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //        // make line breaking consistent
        //        result = result.Replace("\n", "\r");

        //        // Remove extra line breaks and tabs:
        //        // replace over 2 breaks with 2 and over 4 tabs with 4.
        //        // Prepare first to remove any whitespaces in between
        //        // the escaped characters and remove redundant tabs in between line breaks
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 "(\r)( )+(\r)", "\r\r",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 "(\t)( )+(\t)", "\t\t",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 "(\t)( )+(\r)", "\t\r",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 "(\r)( )+(\t)", "\r\t",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        // Remove redundant tabs
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 "(\r)(\t)+(\r)", "\r\r",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        // Remove multiple tabs following a line break with just one tab
        //        result = System.Text.RegularExpressions.Regex.Replace(result,
        //                 "(\r)(\t)+", "\r\t",
        //                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        // Initial replacement target string for line breaks
        //        string breaks = "\r\r\r";
        //        // Initial replacement target string for tabs
        //        string tabs = "\t\t\t\t\t";
        //        for (int index = 0; index < result.Length; index++)
        //        {
        //            result = result.Replace(breaks, "\r\r");
        //            result = result.Replace(tabs, "\t\t\t\t");
        //            breaks = breaks + "\r";
        //            tabs = tabs + "\t";
        //        }

        //        // That's it.
        //        return result;
        //    }
        //    catch(Exception ex)
        //    {
        //       throw ex;
        //    }
        //}


        //private string HtmlEncode(string html)
        //{
        //    //String TestString = "This is a <Test String>.";
        //    StringWriter writer = new StringWriter();
        //    //Server.HtmlEncode(TestString, writer);
        //    HttpContext. .Current.Server.HtmlEncode();
        //    System.Web.HttpUtility.
        //    HttpUtility.HtmlEncode(html, writer);
        //    String EncodedString = writer.ToString();
        //    return EncodedString;
        //}

    }
}
