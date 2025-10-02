using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class FixCatalogoArticuloForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogoArticulo_Categoria_CategoriaIdCategoria",
                table: "CatalogoArticulo");

            migrationBuilder.DropIndex(
                name: "IX_CatalogoArticulo_CategoriaIdCategoria",
                table: "CatalogoArticulo");

            migrationBuilder.DropColumn(
                name: "CategoriaIdCategoria",
                table: "CatalogoArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogoArticulo_IdCategoria",
                table: "CatalogoArticulo",
                column: "IdCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogoArticulo_Categoria_IdCategoria",
                table: "CatalogoArticulo",
                column: "IdCategoria",
                principalTable: "Categoria",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogoArticulo_Categoria_IdCategoria",
                table: "CatalogoArticulo");

            migrationBuilder.DropIndex(
                name: "IX_CatalogoArticulo_IdCategoria",
                table: "CatalogoArticulo");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaIdCategoria",
                table: "CatalogoArticulo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogoArticulo_CategoriaIdCategoria",
                table: "CatalogoArticulo",
                column: "CategoriaIdCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogoArticulo_Categoria_CategoriaIdCategoria",
                table: "CatalogoArticulo",
                column: "CategoriaIdCategoria",
                principalTable: "Categoria",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
