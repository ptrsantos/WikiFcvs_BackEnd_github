using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Data.Mappings
{
    public class TemaMapping : IEntityTypeConfiguration<Tema>
    {
        public void Configure(EntityTypeBuilder<Tema> builder)
        {
            builder.HasKey(t => t.Id);

            //builder.Property(t => t.Titulo)
            //    .IsRequired()
            //    .HasColumnType("varchar(100)");

            //builder.HasOne(t => t.EdicaoEfetuada)
            //.WithOne(p => p.Tema)
            //.HasForeignKey<Protocolo>(p => p.TemaId);

            // 1 : 1 => Fornecedor : Endereco
            //builder.HasOne(t => t.Autor)
            //    .WithOne(a => a.Tema)
            //    .HasForeignKey<Usuario>(a => a.TemaId);

            // 1 : N => Fornecedor : Produtos
            builder.HasMany(t => t.Artigos)
                .WithOne(a => a.Tema);
                
            //.HasForeignKey(a => a.TemaId) ;

            builder.HasMany(a => a.Edicoes)
                .WithOne(e => e.Tema)
                .HasForeignKey(e => e.TemaId)
                .OnDelete(DeleteBehavior.Cascade); ;
                //.HasForeignKey(e => e.TemaId);

            builder.ToTable("Temas");
        }
    }
}