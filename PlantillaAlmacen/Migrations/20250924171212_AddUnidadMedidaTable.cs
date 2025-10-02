using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddUnidadMedidaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogoArticulo_UnidadesMedidas_IdUnidadMedida",
                table: "CatalogoArticulo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnidadesMedidas",
                table: "UnidadesMedidas");

            migrationBuilder.RenameTable(
                name: "UnidadesMedidas",
                newName: "UnidadMedida");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnidadMedida",
                table: "UnidadMedida",
                column: "IdUnidadMedida");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogoArticulo_UnidadMedida_IdUnidadMedida",
                table: "CatalogoArticulo",
                column: "IdUnidadMedida",
                principalTable: "UnidadMedida",
                principalColumn: "IdUnidadMedida");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogoArticulo_UnidadMedida_IdUnidadMedida",
                table: "CatalogoArticulo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnidadMedida",
                table: "UnidadMedida");

            migrationBuilder.RenameTable(
                name: "UnidadMedida",
                newName: "UnidadesMedidas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnidadesMedidas",
                table: "UnidadesMedidas",
                column: "IdUnidadMedida");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogoArticulo_UnidadesMedidas_IdUnidadMedida",
                table: "CatalogoArticulo",
                column: "IdUnidadMedida",
                principalTable: "UnidadesMedidas",
                principalColumn: "IdUnidadMedida");
        }
    }
}
