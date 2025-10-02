using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddAlmacenRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "Articulos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Almacenes",
                columns: table => new
                {
                    IdAlmacen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Almacenes", x => x.IdAlmacen);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articulos_IdAlmacen",
                table: "Articulos",
                column: "IdAlmacen");

            migrationBuilder.AddForeignKey(
                name: "FK_Articulos_Almacenes_IdAlmacen",
                table: "Articulos",
                column: "IdAlmacen",
                principalTable: "Almacenes",
                principalColumn: "IdAlmacen",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articulos_Almacenes_IdAlmacen",
                table: "Articulos");

            migrationBuilder.DropTable(
                name: "Almacenes");

            migrationBuilder.DropIndex(
                name: "IX_Articulos_IdAlmacen",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "Articulos");
        }
    }
}
