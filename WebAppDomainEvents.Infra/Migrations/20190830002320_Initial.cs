using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppDomainEvents.Infra.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salario",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Pagamento = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Adiantamento = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DespesaMensal",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Descricao = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    SalarioId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DespesaMensal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DespesaMensal_Salario_SalarioId",
                        column: x => x.SalarioId,
                        principalTable: "Salario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DespesaMensal_SalarioId",
                table: "DespesaMensal",
                column: "SalarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DespesaMensal");

            migrationBuilder.DropTable(
                name: "Salario");
        }
    }
}
