using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Infra.EntityConfig
{
    public class DespesaMensalConfiguration : IEntityTypeConfiguration<DespesaMensal>
    {
        public void Configure(EntityTypeBuilder<DespesaMensal> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Descricao).IsRequired().HasMaxLength(30).HasColumnType("varchar(30)");
            builder.Property(e => e.Valor).IsRequired().HasColumnType("decimal(7,2)");
            builder.Property(e => e.Data).IsRequired().HasColumnType("datetime");
            builder.Property(e => e.Status).HasColumnType("bit");
            builder.HasOne(c => c.Salario).WithMany(c => c.DespesasMensais).HasForeignKey("SalarioId")
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("DespesaMensal");
        }
    }
}
