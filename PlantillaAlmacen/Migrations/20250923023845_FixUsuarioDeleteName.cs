using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class FixUsuarioDeleteName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "Usuario",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
