using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Data.Mappings
{
    public class ArtigoMapping : IEntityTypeConfiguration<Artigo>
    {
        public void Configure(EntityTypeBuilder<Artigo> builder)
        {
            builder.HasKey(p => p.Id);

            //builder.Property(p => p.Conteudo)
            //    .IsRequired()
            //    .HasColumnType("text");

            //builder.Property(a => a.Titulo)
            //    .IsRequired()
            //    .HasColumnType("varchar(100)");

            //builder.Property(a => a.Descricao)
            //    .IsRequired()
            //    .HasColumnType("varchar(200)");

            //builder.HasOne(a => a.EdicaoEfetuada)
            //    .WithOne(p => p.Artigo)
            //    .HasForeignKey<Protocolo>(p => p.ArtigoId);

            builder.HasMany(a => a.Edicoes)
                .WithOne(e => e.Artigo)
                .HasForeignKey(e => e.ArtigoId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 : 1 => Fornecedor : Endereco
            //builder.HasOne(t => t.Autor)
            //    .WithOne(a => a.Artigo)
            //    .HasForeignKey<Usuario>(u => u.TemaId);

            // builder.Property(p => p.)
            //.IsRequired()
            //.HasColumnType("varchar(100)");

            // builder.Property(p => p.Titulo)
            //.IsRequired()
            //.HasColumnType("varchar(100)");

            // 1 : 1 => Fornecedor : Endereco
            //builder.HasOne(f => f.Endereco)
            //    .WithOne(e => e.Fornecedor);

            // 1 : N => Fornecedor : Produtos
            //builder.HasMany(f => f.Produtos)
            //    .WithOne(p => p.Fornecedor)
            //    .HasForeignKey(p => p.FornecedorId);

            builder.ToTable("Artigos");
        }
    }
}