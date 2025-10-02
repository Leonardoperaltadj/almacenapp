using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class FixAsignacionDeleteEstatusAsignacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_EstatusAsignacion_IdEstatusAsignacion",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionEventos_EstatusAsignacion_IdEstatusAsignacion",
                table: "AsignacionEventos");

            migrationBuilder.DropTable(
                name: "EstatusAsignacion");

            migrationBuilder.DropIndex(
                name: "IX_AsignacionEventos_IdEstatusAsignacion",
                table: "AsignacionEventos");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdEstatusAsignacion",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "IdEstatusAsignacion",
                table: "AsignacionEventos");

            migrationBuilder.DropColumn(
                name: "IdEstatusAsignacion",
                table: "Asignacion");

            migrationBuilder.RenameColumn(
                name: "FechaAsinacion",
                table: "Asignacion",
                newName: "FechaAsignacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaAsignacion",
                table: "Asignacion",
                newName: "FechaAsinacion");

            migrationBuilder.AddColumn<int>(
                name: "IdEstatusAsignacion",
                table: "AsignacionEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEstatusAsignacion",
                table: "Asignacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EstatusAsignacion",
                columns: table => new
                {
                    IdEstatusAsignacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estatus = table.Column<bool>(type: "bit", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusAsignacion", x => x.IdEstatusAsignacion);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionEventos_IdEstatusAsignacion",
                table: "AsignacionEventos",
                column: "IdEstatusAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdEstatusAsignacion",
                table: "Asignacion",
                column: "IdEstatusAsignacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_EstatusAsignacion_IdEstatusAsignacion",
                table: "Asignacion",
                column: "IdEstatusAsignacion",
                principalTable: "EstatusAsignacion",
                principalColumn: "IdEstatusAsignacion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionEventos_EstatusAsignacion_IdEstatusAsignacion",
                table: "AsignacionEventos",
                column: "IdEstatusAsignacion",
                principalTable: "EstatusAsignacion",
                principalColumn: "IdEstatusAsignacion");
        }
    }
}
