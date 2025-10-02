using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddFactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacturaArchivo",
                table: "Articulos");

            migrationBuilder.AddColumn<int>(
                name: "IdFactura",
                table: "Articulos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    IdFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroFactura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArchivoFactura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.IdFactura);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articulos_IdFactura",
                table: "Articulos",
                column: "IdFactura");

            migrationBuilder.AddForeignKey(
                name: "FK_Articulos_Facturas_IdFactura",
                table: "Articulos",
                column: "IdFactura",
                principalTable: "Facturas",
                principalColumn: "IdFactura",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articulos_Facturas_IdFactura",
                table: "Articulos");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Articulos_IdFactura",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "IdFactura",
                table: "Articulos");

            migrationBuilder.AddColumn<string>(
                name: "FacturaArchivo",
                table: "Articulos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
