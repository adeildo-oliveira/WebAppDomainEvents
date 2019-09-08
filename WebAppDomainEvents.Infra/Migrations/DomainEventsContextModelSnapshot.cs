﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAppDomainEvents.Infra.Context;

namespace WebAppDomainEvents.Infra.Migrations
{
    [DbContext(typeof(DomainEventsContext))]
    partial class DomainEventsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebAppDomainEvents.Domain.Models.DespesaMensal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<Guid?>("SalarioId");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(7,2)");

                    b.HasKey("Id");

                    b.HasIndex("SalarioId");

                    b.ToTable("DespesaMensal");
                });

            modelBuilder.Entity("WebAppDomainEvents.Domain.Models.Salario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Adiantamento")
                        .HasColumnType("decimal(7,2)");

                    b.Property<decimal>("Pagamento")
                        .HasColumnType("decimal(7,2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Salario");
                });

            modelBuilder.Entity("WebAppDomainEvents.Domain.Models.DespesaMensal", b =>
                {
                    b.HasOne("WebAppDomainEvents.Domain.Models.Salario", "Salario")
                        .WithMany("DespesasMensais")
                        .HasForeignKey("SalarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
