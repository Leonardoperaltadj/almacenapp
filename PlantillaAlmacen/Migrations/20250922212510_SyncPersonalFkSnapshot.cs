using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class SyncPersonalFkSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Puesto",
                table: "Personal");

            migrationBuilder.AddColumn<int>(
                name: "IdFrente",
                table: "Personal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdPuesto",
                table: "Personal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Frente",
                columns: table => new
                {
                    IdFrente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frente", x => x.IdFrente);
                });

            migrationBuilder.CreateTable(
                name: "Puesto",
                columns: table => new
                {
                    IdPuesto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puesto", x => x.IdPuesto);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personal_IdFrente",
                table: "Personal",
                column: "IdFrente");

            migrationBuilder.CreateIndex(
                name: "IX_Personal_IdPuesto",
                table: "Personal",
                column: "IdPuesto");

            migrationBuilder.AddForeignKey(
                name: "FK_Personal_Frente_IdFrente",
                table: "Personal",
                column: "IdFrente",
                principalTable: "Frente",
                principalColumn: "IdFrente",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Personal_Puesto_IdPuesto",
                table: "Personal",
                column: "IdPuesto",
                principalTable: "Puesto",
                principalColumn: "IdPuesto",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personal_Frente_IdFrente",
                table: "Personal");

            migrationBuilder.DropForeignKey(
                name: "FK_Personal_Puesto_IdPuesto",
                table: "Personal");

            migrationBuilder.DropTable(
                name: "Frente");

            migrationBuilder.DropTable(
                name: "Puesto");

            migrationBuilder.DropIndex(
                name: "IX_Personal_IdFrente",
                table: "Personal");

            migrationBuilder.DropIndex(
                name: "IX_Personal_IdPuesto",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "IdFrente",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "IdPuesto",
                table: "Personal");

            migrationBuilder.AddColumn<string>(
                name: "Puesto",
                table: "Personal",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
