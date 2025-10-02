using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnUnidadCatalogoArticulo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUnidadMedida",
                table: "CatalogoArticulo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacturaArchivo",
                table: "Articulo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UnidadesMedidas",
                columns: table => new
                {
                    IdUnidadMedida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clave = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesMedidas", x => x.IdUnidadMedida);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogoArticulo_IdUnidadMedida",
                table: "CatalogoArticulo",
                column: "IdUnidadMedida");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogoArticulo_UnidadesMedidas_IdUnidadMedida",
                table: "CatalogoArticulo",
                column: "IdUnidadMedida",
                principalTable: "UnidadesMedidas",
                principalColumn: "IdUnidadMedida");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogoArticulo_UnidadesMedidas_IdUnidadMedida",
                table: "CatalogoArticulo");

            migrationBuilder.DropTable(
                name: "UnidadesMedidas");

            migrationBuilder.DropIndex(
                name: "IX_CatalogoArticulo_IdUnidadMedida",
                table: "CatalogoArticulo");

            migrationBuilder.DropColumn(
                name: "IdUnidadMedida",
                table: "CatalogoArticulo");

            migrationBuilder.DropColumn(
                name: "FacturaArchivo",
                table: "Articulo");
        }
    }
}
