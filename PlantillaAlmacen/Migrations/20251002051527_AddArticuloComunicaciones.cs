using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddArticuloComunicaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Articulos",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<string>(
                name: "ArticuloHerramienta_Marca",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticuloHerramienta_Modelo",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticuloHerramienta_NumeroSerie",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticuloHerramienta_Marca",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "ArticuloHerramienta_Modelo",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "ArticuloHerramienta_NumeroSerie",
                table: "Articulos");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Articulos",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);
        }
    }
}
