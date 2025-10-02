using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class FixArticuloForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articulo_CatalogoArticulo_CatalogoArticuloIdCatalogoArticulo",
                table: "Articulo");

            migrationBuilder.DropForeignKey(
                name: "FK_Articulo_EstatusArticulo_EstatusArticuloIdEstatusArticulo",
                table: "Articulo");

            migrationBuilder.DropIndex(
                name: "IX_Articulo_CatalogoArticuloIdCatalogoArticulo",
                table: "Articulo");

            migrationBuilder.DropIndex(
                name: "IX_Articulo_EstatusArticuloIdEstatusArticulo",
                table: "Articulo");

            migrationBuilder.DropColumn(
                name: "CatalogoArticuloIdCatalogoArticulo",
                table: "Articulo");

            migrationBuilder.DropColumn(
                name: "EstatusArticuloIdEstatusArticulo",
                table: "Articulo");

            migrationBuilder.CreateIndex(
                name: "IX_Articulo_IdCatalogoArticulo",
                table: "Articulo",
                column: "IdCatalogoArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_Articulo_IdEstatusArticulo",
                table: "Articulo",
                column: "IdEstatusArticulo");

            migrationBuilder.AddForeignKey(
                name: "FK_Articulo_CatalogoArticulo_IdCatalogoArticulo",
                table: "Articulo",
                column: "IdCatalogoArticulo",
                principalTable: "CatalogoArticulo",
                principalColumn: "IdCatalogoArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articulo_EstatusArticulo_IdEstatusArticulo",
                table: "Articulo",
                column: "IdEstatusArticulo",
                principalTable: "EstatusArticulo",
                principalColumn: "IdEstatusArticulo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articulo_CatalogoArticulo_IdCatalogoArticulo",
                table: "Articulo");

            migrationBuilder.DropForeignKey(
                name: "FK_Articulo_EstatusArticulo_IdEstatusArticulo",
                table: "Articulo");

            migrationBuilder.DropIndex(
                name: "IX_Articulo_IdCatalogoArticulo",
                table: "Articulo");

            migrationBuilder.DropIndex(
                name: "IX_Articulo_IdEstatusArticulo",
                table: "Articulo");

            migrationBuilder.AddColumn<int>(
                name: "CatalogoArticuloIdCatalogoArticulo",
                table: "Articulo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstatusArticuloIdEstatusArticulo",
                table: "Articulo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Articulo_CatalogoArticuloIdCatalogoArticulo",
                table: "Articulo",
                column: "CatalogoArticuloIdCatalogoArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_Articulo_EstatusArticuloIdEstatusArticulo",
                table: "Articulo",
                column: "EstatusArticuloIdEstatusArticulo");

            migrationBuilder.AddForeignKey(
                name: "FK_Articulo_CatalogoArticulo_CatalogoArticuloIdCatalogoArticulo",
                table: "Articulo",
                column: "CatalogoArticuloIdCatalogoArticulo",
                principalTable: "CatalogoArticulo",
                principalColumn: "IdCatalogoArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articulo_EstatusArticulo_EstatusArticuloIdEstatusArticulo",
                table: "Articulo",
                column: "EstatusArticuloIdEstatusArticulo",
                principalTable: "EstatusArticulo",
                principalColumn: "IdEstatusArticulo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
