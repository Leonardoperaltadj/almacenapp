using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class FizUsuariosForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Personal_PersonalIdPersonal",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_PersonalIdPersonal",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "PersonalIdPersonal",
                table: "Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdPersonal",
                table: "Usuario",
                column: "IdPersonal");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Personal_IdPersonal",
                table: "Usuario",
                column: "IdPersonal",
                principalTable: "Personal",
                principalColumn: "IdPersonal",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Personal_IdPersonal",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_IdPersonal",
                table: "Usuario");

            migrationBuilder.AddColumn<int>(
                name: "PersonalIdPersonal",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_PersonalIdPersonal",
                table: "Usuario",
                column: "PersonalIdPersonal");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Personal_PersonalIdPersonal",
                table: "Usuario",
                column: "PersonalIdPersonal",
                principalTable: "Personal",
                principalColumn: "IdPersonal",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
