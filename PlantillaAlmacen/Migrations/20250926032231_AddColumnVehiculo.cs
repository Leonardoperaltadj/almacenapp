using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnVehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Anio",
                table: "Articulos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Combustible",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Kilometraje",
                table: "Articulos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transmision",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anio",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Combustible",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Kilometraje",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Transmision",
                table: "Articulos");
        }
    }
}
