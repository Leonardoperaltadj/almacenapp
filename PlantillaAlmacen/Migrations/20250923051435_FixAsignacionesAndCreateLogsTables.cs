using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class FixAsignacionesAndCreateLogsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_Articulo_ArticuloIdArticulo",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_EstadoEntrega_EstadoEntregaIdEstadoEntrega",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_EstatusAsignacion_EstatusAsignacionIdEstatusAsignacion",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_Personal_PersonalIdPersonal",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_ArticuloIdArticulo",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_EstadoEntregaIdEstadoEntrega",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_EstatusAsignacionIdEstatusAsignacion",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_PersonalIdPersonal",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "ArticuloIdArticulo",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "EstadoDevolucion",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "EstadoEntregaIdEstadoEntrega",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "EstatusAsignacionIdEstatusAsignacion",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "PersonalIdPersonal",
                table: "Asignacion");

            migrationBuilder.AlterColumn<int>(
                name: "IdPersonal",
                table: "Asignacion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Asignacion",
                type: "bit",
                maxLength: 255,
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDevolucion",
                table: "Asignacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdDepartamento",
                table: "Asignacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEstadoRecepcion",
                table: "Asignacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdFrente",
                table: "Asignacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservacionesDevolucion",
                table: "Asignacion",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdArticulo",
                table: "Asignacion",
                column: "IdArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdDepartamento",
                table: "Asignacion",
                column: "IdDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdEstadoEntrega",
                table: "Asignacion",
                column: "IdEstadoEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdEstadoRecepcion",
                table: "Asignacion",
                column: "IdEstadoRecepcion");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdEstatusAsignacion",
                table: "Asignacion",
                column: "IdEstatusAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdFrente",
                table: "Asignacion",
                column: "IdFrente");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_IdPersonal",
                table: "Asignacion",
                column: "IdPersonal");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_Articulo_IdArticulo",
                table: "Asignacion",
                column: "IdArticulo",
                principalTable: "Articulo",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_Departamento_IdDepartamento",
                table: "Asignacion",
                column: "IdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_EstadoEntrega_IdEstadoEntrega",
                table: "Asignacion",
                column: "IdEstadoEntrega",
                principalTable: "EstadoEntrega",
                principalColumn: "IdEstadoEntrega",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_EstadoEntrega_IdEstadoRecepcion",
                table: "Asignacion",
                column: "IdEstadoRecepcion",
                principalTable: "EstadoEntrega",
                principalColumn: "IdEstadoEntrega");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_EstatusAsignacion_IdEstatusAsignacion",
                table: "Asignacion",
                column: "IdEstatusAsignacion",
                principalTable: "EstatusAsignacion",
                principalColumn: "IdEstatusAsignacion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_Frente_IdFrente",
                table: "Asignacion",
                column: "IdFrente",
                principalTable: "Frente",
                principalColumn: "IdFrente");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_Personal_IdPersonal",
                table: "Asignacion",
                column: "IdPersonal",
                principalTable: "Personal",
                principalColumn: "IdPersonal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_Articulo_IdArticulo",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_Departamento_IdDepartamento",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_EstadoEntrega_IdEstadoEntrega",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_EstadoEntrega_IdEstadoRecepcion",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_EstatusAsignacion_IdEstatusAsignacion",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_Frente_IdFrente",
                table: "Asignacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Asignacion_Personal_IdPersonal",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdArticulo",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdDepartamento",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdEstadoEntrega",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdEstadoRecepcion",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdEstatusAsignacion",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdFrente",
                table: "Asignacion");

            migrationBuilder.DropIndex(
                name: "IX_Asignacion_IdPersonal",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "FechaDevolucion",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "IdDepartamento",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "IdEstadoRecepcion",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "IdFrente",
                table: "Asignacion");

            migrationBuilder.DropColumn(
                name: "ObservacionesDevolucion",
                table: "Asignacion");

            migrationBuilder.AlterColumn<int>(
                name: "IdPersonal",
                table: "Asignacion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArticuloIdArticulo",
                table: "Asignacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EstadoDevolucion",
                table: "Asignacion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstadoEntregaIdEstadoEntrega",
                table: "Asignacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstatusAsignacionIdEstatusAsignacion",
                table: "Asignacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PersonalIdPersonal",
                table: "Asignacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_ArticuloIdArticulo",
                table: "Asignacion",
                column: "ArticuloIdArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_EstadoEntregaIdEstadoEntrega",
                table: "Asignacion",
                column: "EstadoEntregaIdEstadoEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_EstatusAsignacionIdEstatusAsignacion",
                table: "Asignacion",
                column: "EstatusAsignacionIdEstatusAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_PersonalIdPersonal",
                table: "Asignacion",
                column: "PersonalIdPersonal");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_Articulo_ArticuloIdArticulo",
                table: "Asignacion",
                column: "ArticuloIdArticulo",
                principalTable: "Articulo",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_EstadoEntrega_EstadoEntregaIdEstadoEntrega",
                table: "Asignacion",
                column: "EstadoEntregaIdEstadoEntrega",
                principalTable: "EstadoEntrega",
                principalColumn: "IdEstadoEntrega",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_EstatusAsignacion_EstatusAsignacionIdEstatusAsignacion",
                table: "Asignacion",
                column: "EstatusAsignacionIdEstatusAsignacion",
                principalTable: "EstatusAsignacion",
                principalColumn: "IdEstatusAsignacion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asignacion_Personal_PersonalIdPersonal",
                table: "Asignacion",
                column: "PersonalIdPersonal",
                principalTable: "Personal",
                principalColumn: "IdPersonal",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
