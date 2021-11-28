using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Data.Mappings
{
    public class RegistroRepositoryMapping : IEntityTypeConfiguration<RegistroUsuario>
    {
        public void Configure(EntityTypeBuilder<RegistroUsuario> builder)
        {
            builder.HasKey(r => r.Id);
        }
    }
}
