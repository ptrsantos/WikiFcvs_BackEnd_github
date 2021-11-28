using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Data.Mappings
{
    public class EdicaoTemaoMapping : IEntityTypeConfiguration<EdicaoTema>
    {
        public void Configure(EntityTypeBuilder<EdicaoTema> builder)
        {

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Tema)
                .WithMany(et => et.Edicoes)
                .HasForeignKey(e => e.TemaId)
                .OnDelete(DeleteBehavior.Cascade);
                

            builder.HasOne(e => e.EdicaoEfetuada)
                .WithOne(ef => ef.EdicaoTema)
                .OnDelete(DeleteBehavior.Cascade);
 
        }

    }
}
