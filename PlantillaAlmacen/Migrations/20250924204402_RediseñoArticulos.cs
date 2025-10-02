using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class RediseñoArticulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articulo_CatalogoArticulo_IdCatalogoArticulo",
                table: "Articulo");

            migrationBuilder.DropForeignKey(
                name: "FK_Articulo_EstatusArticulo_IdEstatusArticulo",
                table: "Articulo");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticuloEstadoHistoriales_Articulo_IdArticulo",
                table: "ArticuloEstadoHistoriales");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_Articulo_IdArticulo",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoInventario_Articulo_ArticuloIdArticulo",
                table: "MovimientoInventario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articulo",
                table: "Articulo");

            migrationBuilder.DropColumn(
                name: "Caracterisiticas",
                table: "Articulo");

            migrationBuilder.RenameTable(
                name: "Articulo",
                newName: "Articulos");

            migrationBuilder.RenameIndex(
                name: "IX_Articulo_IdEstatusArticulo",
                table: "Articulos",
                newName: "IX_Articulos_IdEstatusArticulo");

            migrationBuilder.RenameIndex(
                name: "IX_Articulo_IdCatalogoArticulo",
                table: "Articulos",
                newName: "IX_Articulos_IdCatalogoArticulo");

            migrationBuilder.AlterColumn<string>(
                name: "Observacion",
                table: "Articulos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroSerie",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modelo",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticuloTecnologia_Marca",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticuloTecnologia_Modelo",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticuloTecnologia_NumeroSerie",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticuloVehiculo_Marca",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticuloVehiculo_Modelo",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticuloVehiculo_NumeroSerie",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Cantidad",
                table: "Articulos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CantidadPaquete",
                table: "Articulos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caracteristicas",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Destino",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Articulos",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCaducidad",
                table: "Articulos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCompra",
                table: "Articulos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSalida",
                table: "Articulos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lote",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Placas",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolizaSeguro",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Articulos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TarjetaCirculacion",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "Articulos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articulos",
                table: "Articulos",
                column: "IdArticulo");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticuloEstadoHistoriales_Articulos_IdArticulo",
                table: "ArticuloEstadoHistoriales",
                column: "IdArticulo",
                principalTable: "Articulos",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articulos_CatalogoArticulo_IdCatalogoArticulo",
                table: "Articulos",
                column: "IdCatalogoArticulo",
                principalTable: "CatalogoArticulo",
                principalColumn: "IdCatalogoArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articulos_EstatusArticulo_IdEstatusArticulo",
                table: "Articulos",
                column: "IdEstatusArticulo",
                principalTable: "EstatusArticulo",
                principalColumn: "IdEstatusArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_Articulos_IdArticulo",
                table: "Asignacion",
                column: "IdArticulo",
                principalTable: "Articulos",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoInventario_Articulos_ArticuloIdArticulo",
                table: "MovimientoInventario",
                column: "ArticuloIdArticulo",
                principalTable: "Articulos",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticuloEstadoHistoriales_Articulos_IdArticulo",
                table: "ArticuloEstadoHistoriales");

            migrationBuilder.DropForeignKey(
                name: "FK_Articulos_CatalogoArticulo_IdCatalogoArticulo",
                table: "Articulos");

            migrationBuilder.DropForeignKey(
                name: "FK_Articulos_EstatusArticulo_IdEstatusArticulo",
                table: "Articulos");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_Articulos_IdArticulo",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoInventario_Articulos_ArticuloIdArticulo",
                table: "MovimientoInventario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articulos",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "ArticuloTecnologia_Marca",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "ArticuloTecnologia_Modelo",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "ArticuloTecnologia_NumeroSerie",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "ArticuloVehiculo_Marca",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "ArticuloVehiculo_Modelo",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "ArticuloVehiculo_NumeroSerie",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "CantidadPaquete",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Caracteristicas",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Destino",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "FechaCaducidad",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "FechaCompra",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "FechaSalida",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Lote",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Placas",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "PolizaSeguro",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "TarjetaCirculacion",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Articulos");

            migrationBuilder.RenameTable(
                name: "Articulos",
                newName: "Articulo");

            migrationBuilder.RenameIndex(
                name: "IX_Articulos_IdEstatusArticulo",
                table: "Articulo",
                newName: "IX_Articulo_IdEstatusArticulo");

            migrationBuilder.RenameIndex(
                name: "IX_Articulos_IdCatalogoArticulo",
                table: "Articulo",
                newName: "IX_Articulo_IdCatalogoArticulo");

            migrationBuilder.AlterColumn<string>(
                name: "Observacion",
                table: "Articulo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroSerie",
                table: "Articulo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modelo",
                table: "Articulo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Articulo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caracterisiticas",
                table: "Articulo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articulo",
                table: "Articulo",
                column: "IdArticulo");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ArticuloEstadoHistoriales_Articulo_IdArticulo",
                table: "ArticuloEstadoHistoriales",
                column: "IdArticulo",
                principalTable: "Articulo",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_Articulo_IdArticulo",
                table: "Asignacion",
                column: "IdArticulo",
                principalTable: "Articulo",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoInventario_Articulo_ArticuloIdArticulo",
                table: "MovimientoInventario",
                column: "ArticuloIdArticulo",
                principalTable: "Articulo",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
