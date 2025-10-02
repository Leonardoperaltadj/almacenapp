using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckDestinoUnicoAsignacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticuloEstadoHistoriales",
                columns: table => new
                {
                    IdHistorial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdArticulo = table.Column<int>(type: "int", nullable: false),
                    IdEstatusArticuloAnterior = table.Column<int>(type: "int", nullable: false),
                    IdEstatusArticuloNuevo = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Motivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticuloEstadoHistoriales", x => x.IdHistorial);
                    table.ForeignKey(
                        name: "FK_ArticuloEstadoHistoriales_Articulo_IdArticulo",
                        column: x => x.IdArticulo,
                        principalTable: "Articulo",
                        principalColumn: "IdArticulo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticuloEstadoHistoriales_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "AsignacionEventos",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAsignacion = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Comentario = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdEstadoEntrega = table.Column<int>(type: "int", nullable: true),
                    IdEstatusAsignacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionEventos", x => x.IdEvento);
                    table.ForeignKey(
                        name: "FK_AsignacionEventos_Asignacion_IdAsignacion",
                        column: x => x.IdAsignacion,
                        principalTable: "Asignacion",
                        principalColumn: "IdAsignacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignacionEventos_EstadoEntrega_IdEstadoEntrega",
                        column: x => x.IdEstadoEntrega,
                        principalTable: "EstadoEntrega",
                        principalColumn: "IdEstadoEntrega");
                    table.ForeignKey(
                        name: "FK_AsignacionEventos_EstatusAsignacion_IdEstatusAsignacion",
                        column: x => x.IdEstatusAsignacion,
                        principalTable: "EstatusAsignacion",
                        principalColumn: "IdEstatusAsignacion");
                    table.ForeignKey(
                        name: "FK_AsignacionEventos_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Asignacion_DestinoUnico",
                table: "Asignacion",
                sql: "((CASE WHEN [IdPersonal] IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN [IdDepartamento] IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN [IdFrente] IS NULL THEN 0 ELSE 1 END)) = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ArticuloEstadoHistoriales_IdArticulo",
                table: "ArticuloEstadoHistoriales",
                column: "IdArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_ArticuloEstadoHistoriales_IdUsuario",
                table: "ArticuloEstadoHistoriales",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionEventos_IdAsignacion",
                table: "AsignacionEventos",
                column: "IdAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionEventos_IdEstadoEntrega",
                table: "AsignacionEventos",
                column: "IdEstadoEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionEventos_IdEstatusAsignacion",
                table: "AsignacionEventos",
                column: "IdEstatusAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionEventos_IdUsuario",
                table: "AsignacionEventos",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticuloEstadoHistoriales");

            migrationBuilder.DropTable(
                name: "AsignacionEventos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Asignacion_DestinoUnico",
                table: "Asignacion");
        }
    }
}
