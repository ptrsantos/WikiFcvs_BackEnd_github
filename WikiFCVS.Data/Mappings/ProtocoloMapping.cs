using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Data.Mappings
{
    public class ProtocoloMapping : IEntityTypeConfiguration<Protocolo>
    {
        public void Configure(EntityTypeBuilder<Protocolo> builder)
        {
            builder.HasKey(p => p.Id);

            //builder.HasOne(p => p.Tema)
            //    .WithOne(t => t.EdicaoEfetuada)
            //    .HasForeignKey<Tema>(t => t.EdicaoEfetuadaId);

            //builder.HasOne(p => p.Artigo)
            //    .WithOne(a => a.EdicaoEfetuada)
            //    .HasForeignKey<Artigo>(a => a.EdicaoEfetuadaId);


            builder.HasOne(p => p.EdicaoArtigo)
                .WithOne(e => e.EdicaoEfetuada)
                .HasForeignKey<EdicaoArtigo>(e => e.EdicaoEfetuadaId);

            //builder.HasOne(p => p.EdicaoTema)
            //    .WithOne(e => e.EdicaoEfetuada)
            //    .HasForeignKey<EdicaoTema>(e => e.EdicaoEfetuadaId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Property(c => c.EditadoEm).HasColumnType("datetime2");

        }

    }
}
