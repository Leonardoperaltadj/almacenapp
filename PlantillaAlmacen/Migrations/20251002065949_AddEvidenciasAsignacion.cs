using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddEvidenciasAsignacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArchivoDanios",
                table: "Asignacion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EvidenciaAsignacion",
                columns: table => new
                {
                    IdEvidencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAsignacion = table.Column<int>(type: "int", nullable: false),
                    Archivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenciaAsignacion", x => x.IdEvidencia);
                    table.ForeignKey(
                        name: "FK_EvidenciaAsignacion_Asignacion_IdAsignacion",
                        column: x => x.IdAsignacion,
                        principalTable: "Asignacion",
                        principalColumn: "IdAsignacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvidenciaAsignacion_IdAsignacion",
                table: "EvidenciaAsignacion",
                column: "IdAsignacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvidenciaAsignacion");

            migrationBuilder.DropColumn(
                name: "ArchivoDanios",
                table: "Asignacion");
        }
    }
}
