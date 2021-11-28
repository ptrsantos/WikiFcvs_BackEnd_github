using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Data.Mappings
{
    public class EdicaoArtigoMapping : IEntityTypeConfiguration<EdicaoArtigo>
    {
        public void Configure(EntityTypeBuilder<EdicaoArtigo> builder)
        {

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Conteudo)
            .IsRequired()
            .HasColumnType("text");

            builder.HasOne(ea => ea.Artigo)
                .WithMany(a => a.Edicoes)
                .HasForeignKey(ea => ea.ArtigoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.EdicaoEfetuada)
                .WithOne(eef => eef.EdicaoArtigo)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
