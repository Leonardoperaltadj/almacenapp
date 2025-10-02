using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantillaAlmacen.Migrations
{
    /// <inheritdoc />
    public partial class Incial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "EstadoEntrega",
                columns: table => new
                {
                    IdEstadoEntrega = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoEntrega", x => x.IdEstadoEntrega);
                });

            migrationBuilder.CreateTable(
                name: "EstatusArticulo",
                columns: table => new
                {
                    IdEstatusArticulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusArticulo", x => x.IdEstatusArticulo);
                });

            migrationBuilder.CreateTable(
                name: "EstatusAsignacion",
                columns: table => new
                {
                    IdEstatusAsignacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusAsignacion", x => x.IdEstatusAsignacion);
                });

            migrationBuilder.CreateTable(
                name: "Permiso",
                columns: table => new
                {
                    IdPermiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permiso", x => x.IdPermiso);
                });

            migrationBuilder.CreateTable(
                name: "Personal",
                columns: table => new
                {
                    IdPersonal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Puesto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personal", x => x.IdPersonal);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "TipoMovimiento",
                columns: table => new
                {
                    IdTipoMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMovimiento", x => x.IdTipoMovimiento);
                });

            migrationBuilder.CreateTable(
                name: "CatalogoArticulo",
                columns: table => new
                {
                    IdCatalogoArticulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Estatus = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaIdCategoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogoArticulo", x => x.IdCatalogoArticulo);
                    table.ForeignKey(
                        name: "FK_CatalogoArticulo_Categoria_CategoriaIdCategoria",
                        column: x => x.CategoriaIdCategoria,
                        principalTable: "Categoria",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdPersonal = table.Column<int>(type: "int", nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonalIdPersonal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuario_Personal_PersonalIdPersonal",
                        column: x => x.PersonalIdPersonal,
                        principalTable: "Personal",
                        principalColumn: "IdPersonal",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolPermiso",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    IdPermiso = table.Column<int>(type: "int", nullable: false),
                    RolIdRol = table.Column<int>(type: "int", nullable: false),
                    PermisoIdPermiso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermiso", x => new { x.IdRol, x.IdPermiso });
                    table.ForeignKey(
                        name: "FK_RolPermiso_Permiso_PermisoIdPermiso",
                        column: x => x.PermisoIdPermiso,
                        principalTable: "Permiso",
                        principalColumn: "IdPermiso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolPermiso_Rol_RolIdRol",
                        column: x => x.RolIdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articulo",
                columns: table => new
                {
                    IdArticulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCatalogoArticulo = table.Column<int>(type: "int", nullable: false),
                    IdEstatusArticulo = table.Column<int>(type: "int", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroSerie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Caracterisiticas = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Observacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CatalogoArticuloIdCatalogoArticulo = table.Column<int>(type: "int", nullable: false),
                    EstatusArticuloIdEstatusArticulo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articulo", x => x.IdArticulo);
                    table.ForeignKey(
                        name: "FK_Articulo_CatalogoArticulo_CatalogoArticuloIdCatalogoArticulo",
                        column: x => x.CatalogoArticuloIdCatalogoArticulo,
                        principalTable: "CatalogoArticulo",
                        principalColumn: "IdCatalogoArticulo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articulo_EstatusArticulo_EstatusArticuloIdEstatusArticulo",
                        column: x => x.EstatusArticuloIdEstatusArticulo,
                        principalTable: "EstatusArticulo",
                        principalColumn: "IdEstatusArticulo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    UsuarioIdUsuario = table.Column<int>(type: "int", nullable: false),
                    RolIdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRol", x => new { x.IdUsuario, x.IdRol });
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Rol_RolIdRol",
                        column: x => x.RolIdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Usuario_UsuarioIdUsuario",
                        column: x => x.UsuarioIdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asignacion",
                columns: table => new
                {
                    IdAsignacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdArticulo = table.Column<int>(type: "int", nullable: false),
                    IdPersonal = table.Column<int>(type: "int", nullable: false),
                    IdEstadoEntrega = table.Column<int>(type: "int", nullable: false),
                    IdEstatusAsignacion = table.Column<int>(type: "int", nullable: false),
                    FechaAsinacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoDevolucion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ArticuloIdArticulo = table.Column<int>(type: "int", nullable: false),
                    PersonalIdPersonal = table.Column<int>(type: "int", nullable: false),
                    EstadoEntregaIdEstadoEntrega = table.Column<int>(type: "int", nullable: false),
                    EstatusAsignacionIdEstatusAsignacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignacion", x => x.IdAsignacion);
                    table.ForeignKey(
                        name: "FK_Asignacion_Articulo_ArticuloIdArticulo",
                        column: x => x.ArticuloIdArticulo,
                        principalTable: "Articulo",
                        principalColumn: "IdArticulo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asignacion_EstadoEntrega_EstadoEntregaIdEstadoEntrega",
                        column: x => x.EstadoEntregaIdEstadoEntrega,
                        principalTable: "EstadoEntrega",
                        principalColumn: "IdEstadoEntrega",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asignacion_EstatusAsignacion_EstatusAsignacionIdEstatusAsignacion",
                        column: x => x.EstatusAsignacionIdEstatusAsignacion,
                        principalTable: "EstatusAsignacion",
                        principalColumn: "IdEstatusAsignacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asignacion_Personal_PersonalIdPersonal",
                        column: x => x.PersonalIdPersonal,
                        principalTable: "Personal",
                        principalColumn: "IdPersonal",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoInventario",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdArticulo = table.Column<int>(type: "int", nullable: false),
                    IdTipoMovimiento = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ArticuloIdArticulo = table.Column<int>(type: "int", nullable: false),
                    TipoMovimientoIdTipoMovimiento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoInventario", x => x.IdMovimiento);
                    table.ForeignKey(
                        name: "FK_MovimientoInventario_Articulo_ArticuloIdArticulo",
                        column: x => x.ArticuloIdArticulo,
                        principalTable: "Articulo",
                        principalColumn: "IdArticulo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientoInventario_TipoMovimiento_TipoMovimientoIdTipoMovimiento",
                        column: x => x.TipoMovimientoIdTipoMovimiento,
                        principalTable: "TipoMovimiento",
                        principalColumn: "IdTipoMovimiento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articulo_CatalogoArticuloIdCatalogoArticulo",
                table: "Articulo",
                column: "CatalogoArticuloIdCatalogoArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_Articulo_EstatusArticuloIdEstatusArticulo",
                table: "Articulo",
                column: "EstatusArticuloIdEstatusArticulo");

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

            migrationBuilder.CreateIndex(
                name: "IX_CatalogoArticulo_CategoriaIdCategoria",
                table: "CatalogoArticulo",
                column: "CategoriaIdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventario_ArticuloIdArticulo",
                table: "MovimientoInventario",
                column: "ArticuloIdArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventario_TipoMovimientoIdTipoMovimiento",
                table: "MovimientoInventario",
                column: "TipoMovimientoIdTipoMovimiento");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_PermisoIdPermiso",
                table: "RolPermiso",
                column: "PermisoIdPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_RolIdRol",
                table: "RolPermiso",
                column: "RolIdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_PersonalIdPersonal",
                table: "Usuario",
                column: "PersonalIdPersonal");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_RolIdRol",
                table: "UsuarioRol",
                column: "RolIdRol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_UsuarioIdUsuario",
                table: "UsuarioRol",
                column: "UsuarioIdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asignacion");

            migrationBuilder.DropTable(
                name: "MovimientoInventario");

            migrationBuilder.DropTable(
                name: "RolPermiso");

            migrationBuilder.DropTable(
                name: "UsuarioRol");

            migrationBuilder.DropTable(
                name: "EstadoEntrega");

            migrationBuilder.DropTable(
                name: "EstatusAsignacion");

            migrationBuilder.DropTable(
                name: "Articulo");

            migrationBuilder.DropTable(
                name: "TipoMovimiento");

            migrationBuilder.DropTable(
                name: "Permiso");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "CatalogoArticulo");

            migrationBuilder.DropTable(
                name: "EstatusArticulo");

            migrationBuilder.DropTable(
                name: "Personal");

            migrationBuilder.DropTable(
                name: "Categoria");
        }
    }
}
