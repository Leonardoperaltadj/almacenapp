using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnArticulo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroInventario",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroInventario",
                table: "Articulos");
        }
    }
}
