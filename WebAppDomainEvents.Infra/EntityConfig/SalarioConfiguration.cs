using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Infra.EntityConfig
{
    public class SalarioConfiguration : IEntityTypeConfiguration<Salario>
    {
        public void Configure(EntityTypeBuilder<Salario> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Pagamento).HasColumnType("decimal(7,2)").IsRequired();
            builder.Property(c => c.Adiantamento).HasColumnType("decimal(7,2)").IsRequired();
            builder.Property(e => e.Status).HasColumnType("bit");
            builder.HasMany(x => x.DespesasMensais).WithOne(x => x.Salario).OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("Salario");
        }
    }
}
